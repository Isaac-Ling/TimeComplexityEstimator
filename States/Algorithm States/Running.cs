using System;
using System.Threading;

namespace Project
{
    class Running : State
    {

        private readonly Algorithm algorithm;
        private int count = 0;

        public Running(Algorithm runningAlgorithm)
        {
            algorithm = runningAlgorithm;
            Console.Clear();
            Graphics.PrintLine("*** Time Complexity Estimator ***", ConsoleColor.DarkCyan);
            Console.WriteLine();
            Graphics.Print("Running Algorithm:", ConsoleColor.Cyan);
            Console.WriteLine("\n");
        }

        public override void Draw()
        {
            if (algorithm.HasCalibratedData)
            {
                Graphics.Print($"Progress: │{new string('█', algorithm.PercentComplete / 5)}{new string('░', 20 - algorithm.PercentComplete / 5)}│ {algorithm.PercentComplete}%\r", ConsoleColor.White);
            }
            else
            {
                Graphics.Print($"Calibrating Inputs{new string('.', count)}{new string(' ', 3 - count)}\r", ConsoleColor.White);
            }
        }

        public override void Update()
        {
            base.Update();

            count++;
            count %= 4;

            if (algorithm.IsComplete)
            {
                TimeComplexityEstimator getTimeComplexity = new TimeComplexityEstimator(algorithm);
                StateManager.ChangeState(new Calulating(getTimeComplexity));
                Thread calculateTimeComplexity = new Thread(() => getTimeComplexity.Calculate());
                calculateTimeComplexity.Start();
            }
            if (algorithm.Abort)
            {
                Console.WriteLine("\n");
                Graphics.Print($"\n{algorithm.ExitException.Message}", ConsoleColor.Red);
                Console.ReadKey();
                StateManager.ChangeState(new GetAlgorithm());
            }

            Thread.Sleep(300);
        }
    }
}
