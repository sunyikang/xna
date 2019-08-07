using System;

namespace WellnessGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (WellnessGame game = new WellnessGame())
            {
                game.Run();
            }
        }
    }
}

