using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    class ZShapedBrick : BrickBase
    {
        public ZShapedBrick(ConsoleColor color)
        {
            this.Color = color;
            this._RotationAngle = 0;
            this.Pixels = new int[4][]
            {
                new int[2] { 0, 0 },
                new int[2] { 1, 0 },
                new int[2] { 1, 1 },
                new int[2] { 2, 1 }
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

            // First and second pixels are not moved.
            // Third pixel is shifted by vector +/- [1,0]
            // Fourth pixel is shifted by vector +/- [1,2]
            switch (_RotationAngle)
            {
                case 0:
                case 180:
                    this.Pixels[2][0] += 1;
                    this.Pixels[3][0] += 1;
                    this.Pixels[3][1] += 2;
                    break;
                case 90:
                case 270:
                    this.Pixels[2][0] -= 1;
                    this.Pixels[3][0] -= 1;
                    this.Pixels[3][1] -= 2;
                    break;
                default:
                    break;
            } 
        }
    }
}
