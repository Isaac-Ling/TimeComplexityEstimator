using System;

namespace Project
{
    class ChangeAxisSize : GraphState
    {

        private readonly Graph.Axis axis;

        public ChangeAxisSize(Graph graph, Graph.Axis axis) : base(graph)
        {
            this.axis = axis;
        }

        public override void Draw()
        {
            base.Draw();
            Graphics.PrintLine($"Enter the length of the {axis} axis in {(axis == Graph.Axis.input ? "columns" : "rows")}, 'r' for the recommended or 'c' for the current", ConsoleColor.White);
        }

        public override void Update()
        {

            try
            {

                string input = Console.ReadLine();
                int length = 0;

                if (input.ToLower().Trim() != "c")
                {
                    if (input.ToLower().Trim() == "r")
                    {
                        length = axis == Graph.Axis.input ? 50 : 20;
                    }
                    else
                    {
                        if (!int.TryParse(input, out length) || length < 15 || length > Console.BufferWidth)
                        {
                            throw new InvalidInputException();
                        }
                    }

                    if (axis == Graph.Axis.input)
                    {
                        graph.SetXAxisLength(length);
                    }
                    else
                    {
                        graph.SetYAxisLength(length);
                    }
                }

                Continue();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Graphics.PrintLine(ex.Message, ConsoleColor.Red);
                Console.ReadKey();
            }
        }

        private void Continue()
        {
            if (axis == Graph.Axis.input)
            {
                StateManager.ChangeState(new ChangeAxisSize(graph, Graph.Axis.time));
            }
            else
            {
                SavedResults.XAxisLength = graph.XAxisLength;
                SavedResults.YAxisLength = graph.YAxisLength;
                StateManager.ChangeState(new DisplayResults(SavedResults.GetResults(), true, graph));
            }
        }
    }
}