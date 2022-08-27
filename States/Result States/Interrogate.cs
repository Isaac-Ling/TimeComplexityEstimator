using System;

namespace Project
{
    class Interrogate : GraphState
    {

        int resultIndex;
        int nodeIndex;

        public Interrogate(Graph graph) : base(graph)
        {
            resultIndex = graph.numOfResults - 1;
            nodeIndex = 0;
            graph.ChangeSelectedNode(resultIndex, nodeIndex);
            ClearBottomOfScreen();
            ShowNodeInfoTemplate();
        }

        public override void Draw()
        {
            graph.RefreshInterrogateDraw();
            Graphics.PrintLine("Use left and right to change node,\nup and down to change algorithm\nor 'q' to return to the menu", ConsoleColor.Cyan);
        }

        public override void Update()
        {

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    nodeIndex = ((nodeIndex - 1) % Graph.NumOfNodes < 0) ? Graph.NumOfNodes - 1 : ((nodeIndex - 1) % Graph.NumOfNodes);

                    if (!graph.IsNodeVisible(resultIndex, nodeIndex))
                    {
                        nodeIndex = Graph.NumOfNodes;
                        do
                        {
                            nodeIndex--;
                            nodeIndex = Math.Max(0, nodeIndex);
                        } while (!graph.IsNodeVisible(resultIndex, nodeIndex) && nodeIndex > 0);
                    }

                    graph.ChangeSelectedNode(resultIndex, nodeIndex);
                    break;
                case ConsoleKey.RightArrow:
                    nodeIndex = (nodeIndex + 1) % Graph.NumOfNodes;

                    if (!graph.IsNodeVisible(resultIndex, nodeIndex))
                    {
                        nodeIndex = 0;
                    }

                    graph.ChangeSelectedNode(resultIndex, nodeIndex);
                    break;
                case ConsoleKey.DownArrow:
                    graph.ChangeSelectedNode(-1);
                    resultIndex = ((resultIndex - 1) % graph.numOfResults < 0) ? graph.numOfResults - 1 : ((resultIndex - 1) % graph.numOfResults);

                    if (!graph.IsNodeVisible(resultIndex, nodeIndex))
                    {
                        nodeIndex = Graph.NumOfNodes;
                        do
                        {
                            nodeIndex--;
                            nodeIndex = Math.Max(0, nodeIndex);
                        } while (!graph.IsNodeVisible(resultIndex, nodeIndex) && nodeIndex > 0);
                    }

                    graph.ChangeSelectedNode(resultIndex, nodeIndex);
                    break;
                case ConsoleKey.UpArrow:
                    graph.ChangeSelectedNode(-1);
                    resultIndex = ((resultIndex + 1) % graph.numOfResults < 0) ? graph.numOfResults + 1 : ((resultIndex + 1) % graph.numOfResults);

                    if (!graph.IsNodeVisible(resultIndex, nodeIndex))
                    {
                        nodeIndex = Graph.NumOfNodes;
                        do
                        {
                            nodeIndex--;
                            nodeIndex = Math.Max(0, nodeIndex);
                        } while (!graph.IsNodeVisible(resultIndex, nodeIndex) && nodeIndex > 0);
                    }

                    graph.ChangeSelectedNode(resultIndex, nodeIndex);
                    break;
                case ConsoleKey.Q:
                    graph.ChangeSelectedNode(-1);
                    ClearBottomOfScreen();
                    StateManager.ReturnToLastState();
                    break;
            }
        }

        public void ShowNodeInfoTemplate()
        {
            Graphics.PrintLine("Selected Node:", ConsoleColor.Red);
            Graphics.PrintLine("Time:", ConsoleColor.Cyan);
            Graphics.PrintLine("Input Size:", ConsoleColor.Cyan);
            Console.WriteLine();
        }
    }
}