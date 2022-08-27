using System;

namespace Project
{
    class GetAxis : GraphState
    {

        private readonly int resultIndex = 0;

        public GetAxis(Graph graph, int resultIndex) : base(graph)
        {
            this.resultIndex = resultIndex;
        }

        public override void Draw()
        {
            ClearBottomOfScreen();
            Graphics.PrintLine("Enter 1 to extrapolate input, or 2 to extrapolated times", ConsoleColor.White);
        }

        public override void Update()
        {
            int key = Input.KeyToInt(0, 3);

            if (key != -1)
            {
                StateManager.ChangeState(new Extrapolate(graph, key == 1 ? Graph.Axis.input : Graph.Axis.time, resultIndex));
            }
        }
    }
}