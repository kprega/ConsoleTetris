using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    class Program
    {
        static void Main(string[] args)
        {
            var klocek = new Brick();
            while(klocek != null)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.DownArrow:
                        klocek.MoveDown();
                        break;
                    case ConsoleKey.RightArrow:
                        klocek.MoveRight();
                        break;
                    case ConsoleKey.LeftArrow:
                        klocek.MoveLeft();
                        break;
                    case ConsoleKey.Spacebar:
                        klocek.MoveDown();
                        klocek.MoveDown();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
