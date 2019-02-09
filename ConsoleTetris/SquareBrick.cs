using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    class SquareBrick : BrickBase
    {
        public SquareBrick(ConsoleColor color)
        {
            this.Color = color;
            this.Pixels = new int[4][]
            {
                new int[2] { 0, 0 },
                new int[2] { 1, 0 },
                new int[2] { 0, 1 },
                new int[2] { 1, 1 }
            };
        }

        public override void Rotate()
        {
            // Result of rotating a square is exactly the same square.
        }
    }
}
