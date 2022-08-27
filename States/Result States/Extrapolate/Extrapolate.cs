using System;

namespace Project
{
    class Extrapolate : GraphState
    {

        private readonly Graph.Axis extrapolatedAxis;
        private readonly Graph.Axis resultAxis;
        private readonly int resultIndex;

        public Extrapolate(Graph graph, Graph.Axis axis, int resultIndex) : base(graph)
        {
            extrapolatedAxis = axis;
            resultAxis = extrapolatedAxis == Graph.Axis.input ? Graph.Axis.time : Graph.Axis.input;
            this.resultIndex = resultIndex;
        }

        public override void Draw()
        {
            base.Draw();
            Graphics.Print($"Enter the extrapolated {extrapolatedAxis} from ", ConsoleColor.White);
            Graphics.Print($"'{graph.Results[resultIndex].complexity}':\n", Graph.Colors[resultIndex]);
        }

        public override void Update()
        {
            double num = Input.GetInput<double>();

            if (num != default && num > 0)
            {
                Console.WriteLine();
                Graphics.PrintLine($"The estimated {resultAxis} for the given {extrapolatedAxis} {num}{(extrapolatedAxis == Graph.Axis.input ? "" : "ms")} is {Math.Max(Math.Round(extrapolatedAxis == Graph.Axis.input ? graph.Equations[resultIndex].SolveForY(num) : graph.Equations[resultIndex].SolveForX(num)), 0)}{(resultAxis == Graph.Axis.input ? "" : "ms")},\nusing {graph.Equations[resultIndex].Print()} as the generalised function", ConsoleColor.Cyan);
                Console.ReadKey();

                StateManager.ChangeState(new DisplayResults(SavedResults.GetResults(), false));
            }
        }
    }
}