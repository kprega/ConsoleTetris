using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    public class Brick : IBrick
    {
        private ConsoleColor Color { get; set; }
        private char Pixel { get; set; }
        private int LocationX { get; set; }
        private int LocationY { get; set; }


        public Brick()
        {
            this.Color = ConsoleColor.Magenta;
            this.Pixel = '█';
            this.LocationX = 0;
            this.LocationY = 0;
            Console.ForegroundColor = this.Color;
            Console.Write(this.Pixel);
        }

        public void MoveDown()
        {
            Console.Clear();
            Console.SetCursorPosition(this.LocationX, ++this.LocationY);
            Console.Write(this.Pixel);
        }

        public void MoveRight()
        {
            Console.Clear();
            Console.SetCursorPosition(++this.LocationX, this.LocationY);
            Console.Write(this.Pixel);
        }

        public void MoveLeft()
        {
            Console.Clear();
            Console.SetCursorPosition(--this.LocationX, this.LocationY);
            Console.Write(this.Pixel);
        }

        public void MoveDownFast()
        {
            Console.Clear();
            Console.SetCursorPosition(this.LocationX, this.LocationY += 2);
            Console.Write(this.Pixel);
        }

        public void Rotate()
        {
            throw new NotImplementedException();
        }
    }
}
