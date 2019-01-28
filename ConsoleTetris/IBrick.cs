using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    interface IBrick
    {
        void MoveDown();
        void MoveRight();
        void MoveLeft();
        void MoveDownFast();
        void Rotate();
    }
}
