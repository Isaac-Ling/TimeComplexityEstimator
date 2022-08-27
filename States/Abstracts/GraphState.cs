using System;
using System.Threading;

namespace Project
{
    class GraphState : State
    {

        protected Graph graph;

        public GraphState(Graph graph)
        {
            this.graph = graph;
        }

        public override void Draw()
        {
            ClearBottomOfScreen();
        }

        public override void Update()
        {

        }
        
        public void ClearBottomOfScreen()
        {
            Console.SetCursorPosition(0, 10 + graph.YAxisLength);
            for (int i = 0; i < 9; i++)
            {
                Console.WriteLine(new string(' ', Console.BufferWidth));
            }
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, 10 + graph.YAxisLength);
        }

        /// <summary>
        /// Will allow the user to select a result
        /// </summary>
        /// <returns>
        /// The integer index of the seleted result
        /// </returns>
        public int SelectResult(int removeIndex = -1)
        {
            SelectResult selectResult = new SelectResult(graph, removeIndex);

            while (!selectResult.HasFinished)
            {
                selectResult.Draw();
                selectResult.Update();
            }

            return selectResult.ResultIndex;
        }
    }
}