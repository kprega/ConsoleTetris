using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    public class Brick
    {
        static Dictionary<ShapeEnum, int[][]> ShapeDefinitions = new Dictionary<ShapeEnum, int[][]>()
        {
            { ShapeEnum.Z,      new int[][] { new int[2] { 0, 0 }, new int[2] { 1, 0 }, new int[2] { 1, 1 }, new int[2] { 2, 1 } } },
            { ShapeEnum.T,      new int[][] { new int[2] { 1, 0 }, new int[2] { 1, 1 }, new int[2] { 0, 1 }, new int[2] { 2, 1 } } },
            { ShapeEnum.Square, new int[][] { new int[2] { 0, 0 }, new int[2] { 1, 0 }, new int[2] { 0, 1 }, new int[2] { 1, 1 } } },
            { ShapeEnum.S,      new int[][] { new int[2] { 1, 0 }, new int[2] { 2, 0 }, new int[2] { 1, 1 }, new int[2] { 0, 1 } } },
            { ShapeEnum.Long,   new int[][] { new int[2] { 0, 0 }, new int[2] { 1, 0 }, new int[2] { 2, 0 }, new int[2] { 3, 0 } } },
            { ShapeEnum.L,      new int[][] { new int[2] { 0, 0 }, new int[2] { 0, 1 }, new int[2] { 0, 2 }, new int[2] { 1, 2 } } },
            { ShapeEnum.J,      new int[][] { new int[2] { 1, 0 }, new int[2] { 1, 1 }, new int[2] { 0, 2 }, new int[2] { 1, 2 } } }
        };
        readonly char Character = '█';

        public ConsoleColor Color { get; private set; }
        public int[][] Pixels { get; private set; }
        public ShapeEnum Shape { get; private set; }

        public Brick(ShapeEnum shape, ConsoleColor color)
        {
            Shape = shape;
            Pixels = ShapeDefinitions[shape];
            Color = color;
        }

        /// <summary>
        /// Erases brick from the board.
        /// </summary>
        public void Erase()
        {
            Console.ResetColor();
            foreach (int[] point in Pixels)
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
            Console.ForegroundColor = Color;
            foreach (int[] point in Pixels)
            {
                Console.SetCursorPosition(point[0], point[1]);
                Console.Write(Character);
            }
        }

        /// <summary>
        /// Moves brick on the board.
        /// </summary>
        /// <param name="dX">Displacement on horizontal direction.</param>
        /// <param name="dY">Displacement on vertival direction.</param>
        public void Move(int dX, int dY)
        {
            foreach (int[] point in Pixels)
            {
                point[0] += dX;
                point[1] += dY;
            }
        }

        /// <summary>
        /// Rotates brick 90° counterclockwise.
        /// </summary>
        public void Rotate()
        {
            // Rotation is achieved by transposing point matrix and negating its Y values
            foreach (int[] point in Pixels)
            {
                var temp = point[0];
                point[0] = point[1];
                point[1] = -temp;
            }
        }
    }
}
