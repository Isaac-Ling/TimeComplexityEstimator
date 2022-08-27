using System;

namespace Project
{
    class Menu : IDrawable, IUpdateable
    {

        private readonly string banner;
        private readonly MenuOption[] actions;

        public Menu(string banner, params MenuOption[] actions)
        {
            this.banner = banner;
            this.actions = actions;
        }

        public void Draw()
        {
            Graphics.PrintLine(banner, ConsoleColor.Cyan);

            for (int i = 0; i < actions.Length; i++)
            {
                Graphics.PrintLine($"{i + 1} - {actions[i].description}", ConsoleColor.White);
            }
        }

        public void Update()
        {
            int key = Input.KeyToInt(0, actions.Length);

            if (key != -1)
            {
                try
                {
                    actions[key - 1].action.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Graphics.PrintLine(ex.Message, ConsoleColor.Red);
                    Console.ReadKey();
                }
            }
        }
    }

    struct MenuOption
    {

        public readonly string description;
        public readonly Action action;

        public MenuOption(string description, Action action)
        {
            this.description = description;
            this.action = action;
        }
    }
}