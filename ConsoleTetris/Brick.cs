using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    class Brick
    {
        public readonly char Pixel = '█';

        public ConsoleColor Color { get; private set; }
        public int[][] Pixels { get; set; }
        private Shape Shape { get; set; }

        /// <summary>
        /// Creates new instance of Brick
        /// </summary>
        /// <param name="X">Left offset</param>
        /// <param name="Y">Top offset</param>
        public Brick(int X, int Y)
        {
            this.Color = GetRandomColor();
            this.Shape = new Shape();
            this.Pixels = CalculateBrickPosition(X, Y);
            this.Write();
        }

        /// <summary>
        /// Chooses a random color for the brick.
        /// </summary>
        /// <returns>A random ConsoleColor, except black and white.</returns>
        private ConsoleColor GetRandomColor()
        {
            var numberOfAvailableColors = Enum.GetValues(typeof(ConsoleColor)).Length;
            var randomIndex = new Random().Next(1, numberOfAvailableColors - 2);
            return (ConsoleColor)randomIndex;
        }

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
                Console.Write(this.Pixel);
            }
        }

        /// <summary>
        /// Calculates position of the brick on the board.
        /// </summary>
        /// <param name="x">Displacement on horizontal direction.</param>
        /// <param name="y">Displacement on vertival direction.</param>
        /// <returns>Array of pixel positions.</returns>
        private int[][] CalculateBrickPosition(int x, int y)
        {
            var result = new int[4][];
            result = this.Shape.Pixels.Clone() as int[][];

            foreach (int[] point in result)
            {
                point[0] += x;
                point[1] += y;
            }

            return result;
        }

        /// <summary>
        /// Rotates brick shape 90° counterclockwise.
        /// </summary>
        public void Rotate()
        {
            this.Shape.ToggleRotation();
            var maxY = 0;
            var maxX = 0;

            foreach (var point in this.Shape.Pixels)
            {
                if (maxX < point[0]) maxX = point[0];
                if (maxY < point[1]) maxY = point[1];
            }

            this.Pixels = this.Shape.Pixels;

            foreach (var point in this.Pixels)
            {
                point[0] += maxX;
                point[1] += maxY;
            }
        }

    }
}
