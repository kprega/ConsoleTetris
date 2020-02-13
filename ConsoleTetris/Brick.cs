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
        public static char Character = '■';

        public ConsoleColor Color { get; private set; }
        public int[][] Pixels { get; private set; }
        public ShapeEnum Shape { get; private set; }
        private int[] _boardScaling;

        public Brick(ShapeEnum shape, ConsoleColor color, int[] boardScaling)
        {
            _boardScaling = boardScaling;
            Shape = shape;
            Pixels = new int[ShapeDefinitions[shape].GetLength(0)][];
            for (int i = 0; i < Pixels.GetLength(0); i++)
            {
                Pixels[i] = new int[] { _boardScaling[0] * ShapeDefinitions[shape][i][0] + 1, _boardScaling[1] * ShapeDefinitions[shape][i][1] + 1};
            }
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
            Pixels = GetMovedPixels(dX, dY);
        }

        /// <summary>
        /// Creates an array representing brick's shape after moving by specified values.
        /// </summary>
        /// <param name="dX">Displacement on horizontal direction.</param>
        /// <param name="dY">Displacement on vertival direction.</param>
        public int[][] GetMovedPixels(int dX, int dY)
        {
            var moved = new int[Pixels.GetLength(0)][];
            for (int i = 0; i < moved.GetLength(0); i++)
            {
                moved[i] = new int[] { Pixels[i][0] + dX * _boardScaling[0], Pixels[i][1] + dY * _boardScaling[1] };
            }
            return moved;
        }

        /// <summary>
        /// Rotates brick 90° counterclockwise.
        /// </summary>
        public void Rotate()
        {
            Pixels = GetRotatedPixels();
        }

        /// <summary>
        /// Creates an array representing brick's shape after rotation.
        /// </summary>
        /// <returns>Copy of Pixels array after rotation.</returns>
        public int[][] GetRotatedPixels()
        {
            var rotated = new int[Pixels.GetLength(0)][];
            for (int i = 0; i < rotated.GetLength(0); i++)
            {
                rotated[i] = new int[] { Pixels[i][0], Pixels[i][1] };
            }
            var rotationCenter = Pixels[1];

            // Rotating square-shaped brick is pointless, preventing it
            if (Shape == ShapeEnum.Square) return rotated;

            // Rotating around center point
            foreach (int[] point in rotated)
            {
                var distances = new int[] { point[0] - rotationCenter[0], point[1] - rotationCenter[1] };
                point[0] = -distances[1] * _boardScaling[0] / _boardScaling[1] + rotationCenter[0]; // X = rotation center X - distance Y * X / Y scale ratio
                point[1] =  distances[0] * _boardScaling[1] / _boardScaling[0] + rotationCenter[1]; // Y = rotation center Y + distance X * Y / X scale ratio
            }
            return rotated;
        }
    }
}
