using System;

namespace ConsoleTetris
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game;
            ConsoleKey key;
            
            do
            {
                Console.Clear();
                game = new Game();
                game.Start();

                Console.Write(" Do you want to play again? Y/N");
                key = Console.ReadKey().Key;
            } while (game.IsOver && key == ConsoleKey.Y);
        }
    }
}
