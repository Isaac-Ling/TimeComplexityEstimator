namespace Project
{
    static class StateManager
    {

        private static State currentState;
        private static State lastState;
        private static bool quit;

        public static void ChangeState(State newState)
        {
            lastState = currentState;
            currentState = newState;
        }

        public static void DrawCurrentState()
        {
            currentState.Draw();
        }

        public static void UpdateCurrentState()
        {
            currentState.Update();
        }

        public static void ReturnToLastState()
        {
            currentState = lastState;
        }

        public static void Quit()
        {
            quit = true;
        }

        public static bool ShouldQuit()
        {
            return quit;
        }
    }
}