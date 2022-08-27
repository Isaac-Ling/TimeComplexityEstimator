using System;
using System.Collections.Generic;

namespace Project
{
    class DisplayResults : GraphState
    {

        private readonly Menu menu;
        private readonly List<Result> results = new List<Result>();

        /// <param name="doFirstDraw"> Whether to draw the template for the graph or not </param>
        /// <param name="graph"> Optional paramter to pass an already instantiated graph to avoid a new one being made </param>
        public DisplayResults(List<Result> results, bool doFirstDraw, Graph graph = null) : base(graph ?? new Graph(results))
        {
            this.results = results;
            SavedResults.SaveResults(results);
            menu = new Menu(
                "Please select your choice:",
                new MenuOption("Interrogate graph", Interrogate),
                new MenuOption("Extrapolate graph", ExtrapolateGraph),
                new MenuOption("Find intersection", FindIntersection),
                new MenuOption("Change the axis size or scale", ChangeAxis),
                new MenuOption("Add algorithms to this graph", AddAlgorithm),
                new MenuOption("Back to main menu", MainMenu)
            );

            if (doFirstDraw)
                FirstDraw();
        }

        /// <summary>
        /// Draws the necessary templates so that the entire screen does not need to be cleared
        /// </summary>
        public void FirstDraw()
        {
            DrawBanner();
            if (results.Count > 1)
            {
                Graphics.PrintLine($"Your algorithms are likely to be:", ConsoleColor.White);
                for (int i = 0; i < results.Count; i++)
                {
                    Graphics.Print($"{results[i].complexity} - {results[i].bigO}{(i == results.Count - 1 ? "" : ",")} ", Graph.Colors[i]);
                }
            }
            else
            {
                Graphics.PrintLine($"Your algorithm is likely to be {results[0].complexity} - {results[0].bigO}", ConsoleColor.White);
            }
            Console.WriteLine("\n");
            graph.Draw();
        }

        /// <exception cref="GraphLimitReachedException"> 
        /// Thrown when the user attempts to add more than 5 algorithms to the graph
        /// </exception>
        public void AddAlgorithm()
        {
            if (results.Count >= 5)
            {
                throw new GraphLimitReachedException();
            }
            else
            {
                StateManager.ChangeState(new GetAlgorithm());
            }
        }

        public void Interrogate()
        {
            StateManager.ChangeState(new Interrogate(graph));
        }

        public override void Draw()
        {   
            base.Draw();
            menu.Draw();
        }

        public override void Update()
        {
            menu.Update();
            base.Update();
        }

        public void Quit()
        {
            StateManager.ChangeState(new Quit());
        }

        public void ChangeAxis()
        {

            ClearBottomOfScreen();
            Graphics.PrintLine("Press 1 to change the axis scale, or 2 to change the axis size", ConsoleColor.Cyan);

            int key = Input.KeyToInt(1, 2);

            if (key == 1)
            {
                StateManager.ChangeState(new ChangeAxisScale(graph, Graph.Axis.input));
            }
            else
            {
                StateManager.ChangeState(new ChangeAxisSize(graph, Graph.Axis.input));
            }
        }

        public void ExtrapolateGraph()
        {
            if (graph.numOfResults == 1)
            {
                StateManager.ChangeState(new GetAxis(graph, 0));
            }
            else
            {
                StateManager.ChangeState(new GetAxis(graph, SelectResult()));
            }
        }

        public void FindIntersection()
        {
            if (graph.numOfResults == 1)
            {
                throw new NotEnoughResultsException();
            }
            else
            {
                ClearBottomOfScreen();
                int equation1Index = SelectResult();
                Equation equation1 = graph.Equations[equation1Index];
                ClearBottomOfScreen();
                Equation equation2 = graph.Equations[SelectResult(equation1Index)];
                StateManager.ChangeState(new FindIntersection(graph, equation1, equation2));
            }
        }

        public void MainMenu()
        {
            SavedResults.RemoveResults();
            StateManager.ChangeState(new Welcome());
        }
    }
}