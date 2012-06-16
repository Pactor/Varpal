using System;

namespace Varpal
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Varpal game = new Varpal())
            {
                game.Run();
            }
        }
    }
#endif
}

