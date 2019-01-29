using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    public class Brick : IBrick
    {
        private readonly char Pixel = '█';

        private ConsoleColor Color { get; set; }
        private int LocationX { get; set; }
        private int LocationY { get; set; }

        public Brick()
        {
            this.Color = GetRandomColor();
            this.LocationX = 0;
            this.LocationY = 0;
            Console.ForegroundColor = this.Color;
            Console.Write(this.Pixel);
        }

        public void MoveDown()
        {
            this.Move(0, 1);
        }

        public void MoveRight()
        {
            this.Move(1, 0);
        }

        public void MoveLeft()
        {
            this.Move(-1, 0);
        }

        public void MoveDownFast()
        {
            this.Move(0, 2);
        }

        public void Rotate()
        {
            throw new NotImplementedException();
        }

        private ConsoleColor GetRandomColor()
        {
            var numberOfAvailableColors = Enum.GetValues(typeof(ConsoleColor)).Length - 1;
            var randomIndex = Math.Floor(new Random().NextDouble() * numberOfAvailableColors + 1);
            return (ConsoleColor)randomIndex;
        }

        private void Move(int dX, int dY)
        {
            if(this.LocationX == 0 && dX < 0)
            {
                return;
            }
            Console.SetCursorPosition(this.LocationX, this.LocationY);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(' ');
            Console.ForegroundColor = this.Color;
            Console.SetCursorPosition(this.LocationX += dX, this.LocationY += dY);
            Console.Write(this.Pixel);
        }
    }
}
