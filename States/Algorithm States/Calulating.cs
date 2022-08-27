using System;
using System.Collections.Generic;
using System.Threading;

namespace Project
{
    class Calulating : State
    {

        private readonly TimeComplexityEstimator timeComplexityEstimator;
        private int count = 0;

        public Calulating(TimeComplexityEstimator timeComplexityEstimator)
        {
            this.timeComplexityEstimator = timeComplexityEstimator;
            Console.Clear();
            Graphics.PrintLine("*** Time Complexity Estimator ***", ConsoleColor.DarkCyan);
            Console.WriteLine();
            Graphics.Print("Calculating Time Complexity:", ConsoleColor.Cyan);
            Console.WriteLine("\n");
        }

        public override void Draw()
        {
            Graphics.Print($"Loading{new string('.', count)}{new string(' ', Console.WindowWidth - 7 - count)}\r", ConsoleColor.White);
        }

        public override void Update()
        {
            base.Update();

            count++;

            count %= 4;

            if (timeComplexityEstimator.IsComplete && count % 4 == 0)
            {
                if (SavedResults.AreResultsSaved())
                {
                    SavedResults.AddResult(timeComplexityEstimator.ResultOfAlgorithm);
                    StateManager.ChangeState(new DisplayResults(SavedResults.GetResults(), true, new Graph(SavedResults.GetResults(), SavedResults.XAxisLength, SavedResults.YAxisLength)));
                }
                else
                {
                    StateManager.ChangeState(new DisplayResults(new List<Result> { timeComplexityEstimator.ResultOfAlgorithm }, true));
                }
            }

            Thread.Sleep(300);
        }
    }
}