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
            Console.CursorVisible = false;
            
            var brick = new Brick();
            while(brick != null)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.DownArrow:
                        brick.MoveDown();
                        break;
                    case ConsoleKey.RightArrow:
                        brick.MoveRight();
                        break;
                    case ConsoleKey.LeftArrow:
                        brick.MoveLeft();
                        break;
                    case ConsoleKey.Spacebar:
                        brick.MoveDownFast();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
