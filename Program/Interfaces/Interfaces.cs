namespace Project
{
    /// <summary>
    /// This object can be drawn to the screen using the Draw method
    /// </summary>
    interface IDrawable
    {
        public void Draw();
    }

    /// <summary>
    /// This object can be updated using the Update method
    /// </summary>
    interface IUpdateable
    {
        public void Update();
    }
}