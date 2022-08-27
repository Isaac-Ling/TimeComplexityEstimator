using System;

namespace Project
{
    abstract class State : IDrawable, IUpdateable
    {

        public State()
        {

        }

        public virtual void Draw()
        {
            DrawBanner();
        }

        public virtual void Update()
        {

        }

        protected void DrawBanner()
        {
            Console.Clear();
            Graphics.PrintLine("*** Time Complexity Estimator ***", ConsoleColor.DarkCyan);
            Console.WriteLine();
        }
    }
}
