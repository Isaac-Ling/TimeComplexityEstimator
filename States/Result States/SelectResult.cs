using System;
using System.Collections.Generic;

namespace Project
{
    class SelectResult : GraphState
    {

        private List<Result> results = new List<Result>();
        private int removeIndex;

        public int ResultIndex { get; private set; }
        public bool HasFinished { get; private set; } = false;

        /// <param name="removeThisIndex">
        /// Optional parameter to remove a result from the options to choose from
        /// </param>
        public SelectResult(Graph graph, int removeThisIndex = -1) : base(graph)
        {
            for (int i = 0; i < graph.Results.Count; i++)
            {
                if (i != removeThisIndex)
                    results.Add(graph.Results[i]);
            }
            removeIndex = removeThisIndex;
            ClearBottomOfScreen();
            PrintResults();
            Console.WriteLine();
            Graphics.PrintLine("Use up and down to select a result,\nusing enter to select it", ConsoleColor.Cyan);
        }

        public override void Draw()
        {
            Graphics.Print(" ", ConsoleColor.Red);
            Console.SetCursorPosition(results[ResultIndex].complexity.ToString().Length + 1, graph.YAxisLength + 10 + ResultIndex);
            Graphics.Print("*", ConsoleColor.Red);
        }

        public override void Update()
        {

            Console.SetCursorPosition(results[ResultIndex].complexity.ToString().Length + 1, graph.YAxisLength + 10 + ResultIndex);

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    ResultIndex = ((ResultIndex - 1) % results.Count < 0) ? results.Count - 1 : ((ResultIndex - 1) % results.Count);
                    break;
                case ConsoleKey.DownArrow:
                    ResultIndex = ((ResultIndex + 1) % results.Count < 0) ? results.Count + 1 : ((ResultIndex + 1) % results.Count);
                    break;
                case ConsoleKey.Enter:
                    ClearBottomOfScreen();
                    ResultIndex = graph.Results.FindIndex(0, x => x.Equals(results[ResultIndex]));
                    HasFinished = true;
                    break;
            }
        }

        public void PrintResults()
        {
            for (int i = 0; i < graph.Equations.Count; i++)
            {
                if (i != removeIndex)
                    Graphics.PrintLine($"{graph.Results[i].complexity}", Graph.Colors[i]);
            }
        }
    }
}