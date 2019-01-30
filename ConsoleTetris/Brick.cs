using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    public class Brick
    {
        private readonly char Pixel = '█';

        public ConsoleColor Color { get; private set; }
        public int LocationX { get; private set; }
        public int LocationY { get; private set; }

        /// <summary>
        /// Creates new instance of Brick
        /// </summary>
        /// <param name="X">Left offset</param>
        /// <param name="Y">Top offset</param>
        public Brick(int X, int Y)
        {
            this.Color = GetRandomColor();
            this.LocationX = X;
            this.LocationY = Y;
            Console.SetCursorPosition(this.LocationX, this.LocationY);
            Console.ForegroundColor = this.Color;
            Console.Write(this.Pixel);
        }

        private ConsoleColor GetRandomColor()
        {
            var numberOfAvailableColors = Enum.GetValues(typeof(ConsoleColor)).Length - 1;
            var randomIndex = Math.Floor(new Random().NextDouble() * numberOfAvailableColors + 1);
            return (ConsoleColor)randomIndex;
        }

        public void Move(int dX, int dY)
        {
            Console.SetCursorPosition(this.LocationX, this.LocationY);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(' ');
            Console.ForegroundColor = this.Color;
            Console.SetCursorPosition(this.LocationX += dX, this.LocationY += dY);
            Console.Write(this.Pixel);
        }
    }
}
