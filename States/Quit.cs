namespace Project
{
    class Quit : MenuState
    {

        public Quit()
        {

            menu = new Menu(
                "Are you sure you want to quit?",
                new MenuOption("Yes", QuitProgram),
                new MenuOption("No", ResumeProgram)
            );
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Update()
        {
            base.Update();
        }

        public void QuitProgram()
        {
            StateManager.Quit();
        }

        public void ResumeProgram()
        {
            StateManager.ReturnToLastState();
        }
    }
}
