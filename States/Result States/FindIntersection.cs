using System;
using System.Collections.Generic;

namespace Project
{
    class FindIntersection : GraphState
    {

        Graph.Vector intersect;

        public FindIntersection(Graph graph, Equation equation1, Equation equation2) : base(graph)
        {
            intersect = graph.FindIntersection(equation1, equation2);
        }

        public override void Draw()
        {
            base.Draw();
            if (intersect.X < 0 || intersect.Y < 0)
            {
                Graphics.Print("No intersections found", ConsoleColor.Red);
            }
            else
            {
                Graphics.PrintLine($"These graphs intersect at:", ConsoleColor.Cyan);
                Graphics.PrintLine($"Input: {intersect.X}\nTime:  {intersect.Y}", ConsoleColor.White);
            } 
        }

        public override void Update()
        {
            base.Update();
            Console.ReadKey();
            StateManager.ReturnToLastState();
        }
    }
}