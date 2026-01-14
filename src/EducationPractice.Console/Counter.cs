public class Counter
{
    public event Action<string>? OnNumIncreased;
    public event Action<string>? OnNumDecreased;
    public event Action<string>? OnCounterReset;
    public event Action<string>? OnGoalAchieved;
    public event Action<string>? OnGoalLost;
    public const string Quit = "q";
    public const string Reset = "00";
    int goal = 0;
    bool isGoalReached = false;

    public async Task Start(IConsumer<byte[]> consumer)
    {
        bool isEnd = false;
        int resultSum = 0;
        int previusSum = 0;

        Console.WriteLine("Enter [q] to end up a program; [00] to reset a counter");

        goal = SetGoal();

        Console.WriteLine("Enter a number: ");

        while (isEnd is false)
        {
            var message = await consumer.ReceiveAsync();
            var messageData = Encoding.UTF8.GetString(message.Data);

            var input = messageData;

            Console.ForegroundColor = ConsoleColor.White;

            if (input == Quit)
            {
                isEnd = true;
                break;
            }

            ResetCounterToZero(resultSum, input);

            if (int.TryParse(input, out int number) is false)
            {
                Console.WriteLine($"[INVALID TYPE] - {input}. Please enter a number!");
                continue;
            }

            resultSum += number;
            Compare(previusSum, resultSum, input);
            previusSum += number;

            await consumer.AcknowledgeAsync(message.MessageId);
            // await consumer.NegativeAcknowledge(message.MessageId);
        }

        Console.WriteLine($"GENERAL SUM: {resultSum}");
    }

    private int SetGoal()
    {
        bool isGoalSet = false;
        while (isGoalSet is not true)
        {
            Console.Write("Enter the goal to achieve: ");
            string goalToAchieve = Console.ReadLine() ?? string.Empty;

            if (int.TryParse(goalToAchieve, out int goal) is false)
            {
                Console.WriteLine($"[INVALID INPUT] - {goalToAchieve}. Please enter a number!");
                continue;
            }
            return goal;
        }
        return 0;
    }

    private void ResetCounterToZero(int resultSum, string input)
    {
        if (input == Reset)
        {
            OnCounterReset?.Invoke($"SUM reset to '0'. Common SUM was [{resultSum}]");
            resultSum = 0;
            isGoalReached = false;
        }
    }

    private void IsGaolAchieved(int resultSum)
    {
        if (resultSum >= goal && isGoalReached == false)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            OnGoalAchieved?.Invoke($"SUM exeed a goal [{goal}]. General SUM is [{resultSum}]");
            isGoalReached = true;
        }
    }

    private void IsGoalLost(int resultSum)
    {
        if (resultSum < goal && isGoalReached == true)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            OnGoalLost?.Invoke($"SUM lost a goal [{goal}]. General SUM is [{resultSum}]");
            isGoalReached = false;
        }
    }

    private void Compare(int previousSum, int resultSum, string input)
    {
        if (resultSum > previousSum)
        {
            OnNumIncreased?.Invoke($"Sum incresed by: {input}. [GENERAL: {resultSum}]");

            IsGaolAchieved(resultSum);
        }
        else if (resultSum <= previousSum)
        {
            OnNumDecreased?.Invoke($"Sum decreased by: {input}. [GENERAL: {resultSum}]");
            IsGoalLost(resultSum);
        }
    }
}
