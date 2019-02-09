using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    class JShapedBrick : BrickBase
    {
        public JShapedBrick(ConsoleColor color)
        {
            this.Color = color;
            this._RotationAngle = 0;
            this.Pixels = new int[4][]
            {
                new int[2] { 1, 0 },
                new int[2] { 1, 1 },
                new int[2] { 0, 2 },
                new int[2] { 1, 2 }
            };
        }

        private int _RotationAngle;

        public override void Rotate()
        {
            if (_RotationAngle < 270)
            {
                _RotationAngle += 90;
            }
            else
            {
                _RotationAngle = 0;
            }

            // All pixels move around depending from current rotation angle.
            switch (_RotationAngle)
            {
                case 0:
                    this.Pixels[0][1] -= 1; 
                    this.Pixels[1][0] += 1;
                    this.Pixels[2][1] += 2;
                    this.Pixels[3][0] -= 1;
                    this.Pixels[3][1] += 1;
                    break;
                case 90:
                    this.Pixels[1][0] += 1;
                    this.Pixels[1][1] -= 1;
                    this.Pixels[2][1] -= 2;
                    this.Pixels[3][0] += 1;
                    this.Pixels[3][1] -= 1;
                    break;
                case 180:
                    this.Pixels[1][0] -= 2;
                    this.Pixels[1][1] += 1;
                    this.Pixels[3][0] -= 2;
                    this.Pixels[3][1] += 1;
                    break;
                case 270:
                    this.Pixels[0][1] += 1;
                    this.Pixels[3][0] += 2;
                    this.Pixels[3][1] -= 1;
                    break;
                default:
                    break;
            } 
        }
    }
}
