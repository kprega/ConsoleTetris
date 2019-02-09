using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    abstract class BrickBase
    {
        protected readonly char Character = '█';

        public ConsoleColor Color { get; protected set; }
        public int[][] Pixels { get; protected set; }

        /// <summary>
        /// Erases brick from the board.
        /// </summary>
        public void Erase()
        {
            Console.ResetColor();
            foreach (int[] point in this.Pixels)
            {
                Console.SetCursorPosition(point[0], point[1]);
                Console.Write(' ');
            }
        }

        /// <summary>
        /// Writes brick into the board.
        /// </summary>
        public void Write()
        {
            Console.ForegroundColor = this.Color;
            foreach (int[] point in this.Pixels)
            {
                Console.SetCursorPosition(point[0], point[1]);
                Console.Write(this.Character);
            }
        }

        /// <summary>
        /// Moves brick on the board.
        /// </summary>
        /// <param name="x">Displacement on horizontal direction.</param>
        /// <param name="y">Displacement on vertival direction.</param>
        public void Move(int x, int y)
        {
            foreach (int[] point in this.Pixels)
            {
                point[0] += x;
                point[1] += y;
            }
        }

        /// <summary>
        /// Rotates brick 90° counterclockwise.
        /// </summary>
        public virtual void Rotate()
        {

        }
    }
}
