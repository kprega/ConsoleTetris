using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    class Shape
    {
        private Random _Random;

        public ShapeEnum Type { get; private set; }
        public int Rotation { get; private set; }
        public int[][] Pixels { get; private set; }

        /// <summary>
        /// The shape of a brick.
        /// </summary>
        public Shape()
        {
            _Random = new Random();
            this.Type = this.GetRandomShape();
            this.Rotation = this.GetRandomRotation();
            this.Pixels = this.GetPixelsDefinition();
        }

        /// <summary>
        /// Chooses a random shape for the brick.
        /// </summary>
        /// <returns>One of available shapes.</returns>
        private ShapeEnum GetRandomShape()
        {
            var numberOfAvailableShapes = Enum.GetValues(typeof(ShapeEnum)).Length;
            var randomIndex = _Random.Next(0, numberOfAvailableShapes - 1);
            return (ShapeEnum)randomIndex;
        }

        /// <summary>
        /// Chooses a random rotation angle for the brick.
        /// </summary>
        /// <returns>One of following angles: 0, 90, 180, 270.</returns>
        private int GetRandomRotation()
        {
            return _Random.Next(0, 3) * 90;
        }

        /// <summary>
        /// Defines pixels layout for given shape.
        /// </summary>
        /// <returns>Array of pixels to be written.</returns>
        private int[][] GetPixelsDefinition()
        {
            var pixels = new int[4][];
            switch (this.Type)
            {
                case ShapeEnum.Square:
                    pixels[0] = new int[2] { 0, 0 };
                    pixels[1] = new int[2] { 1, 0 };
                    pixels[2] = new int[2] { 0, 1 };
                    pixels[3] = new int[2] { 1, 1 };
                    break;
                case ShapeEnum.LetterS:
                    switch (this.Rotation)
                    {
                        case 0:
                        case 180:
                            pixels[0] = new int[2] { 1, 0 };
                            pixels[1] = new int[2] { 2, 0 };
                            pixels[2] = new int[2] { 0, 1 };
                            pixels[3] = new int[2] { 1, 1 };
                            break;
                        case 90:
                        case 270:
                            pixels[0] = new int[2] { 0, 0 };
                            pixels[1] = new int[2] { 0, 1 };
                            pixels[2] = new int[2] { 1, 1 };
                            pixels[3] = new int[2] { 1, 2 };
                            break;
                        default:
                            break;
                    }
                    break;
                case ShapeEnum.LetterZ:
                    switch (this.Rotation)
                    {
                        case 0:
                        case 180:
                            pixels[0] = new int[2] { 0, 0 };
                            pixels[1] = new int[2] { 1, 0 };
                            pixels[2] = new int[2] { 1, 1 };
                            pixels[3] = new int[2] { 2, 1 };
                            break;
                        case 90:
                        case 270:
                            pixels[0] = new int[2] { 1, 0 };
                            pixels[1] = new int[2] { 0, 1 };
                            pixels[2] = new int[2] { 1, 1 };
                            pixels[3] = new int[2] { 0, 2 };
                            break;
                        default:
                            break;
                    }
                    break;
                case ShapeEnum.LetterT:
                    switch (this.Rotation)
                    {
                        case 0:
                            pixels[0] = new int[2] { 1, 0 };
                            pixels[1] = new int[2] { 0, 1 };
                            pixels[2] = new int[2] { 1, 1 };
                            pixels[3] = new int[2] { 2, 1 };
                            break;
                        case 90:
                            pixels[0] = new int[2] { 1, 0 };
                            pixels[1] = new int[2] { 0, 1 };
                            pixels[2] = new int[2] { 1, 1 };
                            pixels[3] = new int[2] { 1, 2 };
                            break;
                        case 180:
                            pixels[0] = new int[2] { 0, 0 };
                            pixels[1] = new int[2] { 1, 0 };
                            pixels[2] = new int[2] { 2, 0 };
                            pixels[3] = new int[2] { 1, 1 };
                            break;
                        case 270:
                            pixels[0] = new int[2] { 0, 0 };
                            pixels[1] = new int[2] { 0, 1 };
                            pixels[2] = new int[2] { 1, 1 };
                            pixels[3] = new int[2] { 0, 2 };
                            break;
                        default:
                            break;
                    }
                    break;
                case ShapeEnum.LetterI:
                    switch (this.Rotation)
                    {
                        case 0:
                        case 180:
                            pixels[0] = new int[2] { 0, 0 };
                            pixels[1] = new int[2] { 1, 0 };
                            pixels[2] = new int[2] { 2, 0 };
                            pixels[3] = new int[2] { 3, 0 };
                            break;
                        case 90:
                        case 270:
                            pixels[0] = new int[2] { 0, 0 };
                            pixels[1] = new int[2] { 0, 1 };
                            pixels[2] = new int[2] { 0, 2 };
                            pixels[3] = new int[2] { 0, 3 };
                            break;
                        default:
                            break;
                    }
                    break;
                case ShapeEnum.LetterJ:
                    switch (this.Rotation)
                    {
                        case 0:
                            pixels[0] = new int[2] { 1, 0 };
                            pixels[1] = new int[2] { 1, 1 };
                            pixels[2] = new int[2] { 0, 2 };
                            pixels[3] = new int[2] { 1, 2 };
                            break;
                        case 90:
                            pixels[0] = new int[2] { 0, 0 };
                            pixels[1] = new int[2] { 0, 1 };
                            pixels[2] = new int[2] { 1, 1 };
                            pixels[3] = new int[2] { 2, 1 };
                            break;
                        case 180:
                            pixels[0] = new int[2] { 0, 0 };
                            pixels[1] = new int[2] { 1, 0 };
                            pixels[2] = new int[2] { 0, 1 };
                            pixels[3] = new int[2] { 0, 2 };
                            break;
                        case 270:
                            pixels[0] = new int[2] { 0, 0 };
                            pixels[1] = new int[2] { 1, 0 };
                            pixels[2] = new int[2] { 2, 0 };
                            pixels[3] = new int[2] { 2, 1 };
                            break;
                        default:
                            break;
                    }
                    break;
                case ShapeEnum.LetterL:
                    switch (this.Rotation)
                    {
                        case 0:
                            pixels[0] = new int[2] { 0, 0 };
                            pixels[1] = new int[2] { 0, 1 };
                            pixels[2] = new int[2] { 0, 2 };
                            pixels[3] = new int[2] { 1, 2 };
                            break;
                        case 90:
                            pixels[0] = new int[2] { 2, 0 };
                            pixels[1] = new int[2] { 0, 1 };
                            pixels[2] = new int[2] { 1, 1 };
                            pixels[3] = new int[2] { 2, 1 };
                            break;
                        case 180:
                            pixels[0] = new int[2] { 0, 0 };
                            pixels[1] = new int[2] { 1, 0 };
                            pixels[2] = new int[2] { 1, 1 };
                            pixels[3] = new int[2] { 1, 2 };
                            break;
                        case 270:
                            pixels[0] = new int[2] { 0, 0 };
                            pixels[1] = new int[2] { 1, 0 };
                            pixels[2] = new int[2] { 2, 0 };
                            pixels[3] = new int[2] { 0, 1 };
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return pixels;
        }
    }
}
