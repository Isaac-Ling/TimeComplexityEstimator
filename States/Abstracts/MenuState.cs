namespace Project
{
    abstract class MenuState : State
    {

        protected bool ShouldDrawBanner { private get; set; } = true;
        protected Menu menu;

        public MenuState()
        {

        }

        public override void Draw()
        {
            if (ShouldDrawBanner)
            {
                DrawBanner();
            }
            menu.Draw();
        }

        public override void Update()
        {
            base.Update();
            menu.Update();
        }
    }
}