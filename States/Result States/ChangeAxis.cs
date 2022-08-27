using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    class ChangeAxis : GraphState
    {

        private readonly Graph.Axis axis;

        public ChangeAxis(Graph graph, Graph.Axis axis) : base(graph)
        {
            this.axis = axis;
        }

        public override void Draw()
        {
            base.Draw();
            Graphics.PrintLine($"Enter the maximum {axis}, 'm' for the maximum or 'c' for the current", ConsoleColor.White);
        }

        public override void Update()
        {

            try
            {

                string input = Console.ReadLine();
                int maxSize = 0;

                if (input.ToLower().Trim() != "c")
                {
                    if (input.ToLower().Trim() == "m")
                    {
                        maxSize = axis == Graph.Axis.input ? graph.GreatestX() : graph.GreatestY();
                    }
                    else
                    {
                        if (!int.TryParse(input, out maxSize) || maxSize <= 0)
                        {
                            throw new InvalidInputException();
                        }
                    }

                    if (axis == Graph.Axis.input)
                    {
                        graph.SetXAxis(maxSize);
                    }
                    else
                    {
                        graph.SetYAxis(maxSize);
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
                StateManager.ChangeState(new ChangeAxis(graph, Graph.Axis.time));
            }
            else
            {
                StateManager.ChangeState(new DisplayResults(SavedResults.GetResults(), true, graph));
            }
        }
    }
}