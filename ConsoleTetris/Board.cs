using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    class Board
    {       
        /// <summary>
        /// Labels providing some information about current game state and their locations in the console window.
        /// </summary>
        public Dictionary<string, int[]> Labels { get; private set; }

        /// <summary>
        /// Defines size of Tetris game board.
        /// </summary>
        public int[] TheoreticalSize { get; private set; }

        /// <summary>
        /// Introduced to manipulate board width in order to make bricks look like they were built from square blocks.
        /// Still, display is dependent from console's font, so results may vary.
        /// </summary>
        public int[] BoardScaling { get; private set; }

        /// <summary>
        /// Area taken by fallen bricks.
        /// </summary>
        public ConsoleColor[,] FilledArea { get; private set; }

        public Board(int[] theoreticalSize, int[] boardScaling)
        {
            TheoreticalSize = theoreticalSize;
            BoardScaling = boardScaling;

            Labels = new Dictionary<string, int[]>()
            {
                { "SCORE:",         new int[]{ TheoreticalSize[0] * BoardScaling[0] + 5, 1 } },
                { "LEVEL:",         new int[]{ TheoreticalSize[0] * BoardScaling[0] + 5, 3 } },
                { "LINES CLEARED:", new int[]{ TheoreticalSize[0] * BoardScaling[0] + 5, 5 } },
                { "NEXT BRICK:",    new int[]{ TheoreticalSize[0] * BoardScaling[0] + 5, 7 } }
            };

            DrawFrame();
            DrawLabels();
            FilledArea = new ConsoleColor[TheoreticalSize[0], TheoreticalSize[1]];
        }

        /// <summary>
        /// Draws a frame around game area.
        /// </summary>
        public void DrawFrame()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            var width  = BoardScaling[0] * (TheoreticalSize[0] - 1) + 1;
            var height = BoardScaling[1] * (TheoreticalSize[1] - 1) + 1;

            var firstLine = "╔" + new String('═', width) + "╗";
            var lastLine  = "╚" + new String('═', width) + "╝";

            Console.Write(firstLine);
            for (int i = 1; i < height + 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("║");
                Console.SetCursorPosition(width + 1, i);
                Console.Write("║");
            }
            Console.SetCursorPosition(0, height + 1);
            Console.Write(lastLine);
        }

        /// <summary>
        /// Writes labels to the game board.
        /// </summary>
        private void DrawLabels()
        {
            Console.ForegroundColor = ConsoleColor.White;
            foreach (var label in Labels)
            {
                Console.SetCursorPosition(label.Value[0], label.Value[1]);
                Console.Write(label.Key);
            }
        }

        /// <summary>
        /// Starting from lowest line, checks row by row if it is completely filled up.
        /// If such row is found, it is cleared and all rows above fall down one level.
        /// </summary>
        public void ClearFilledRows()
        {
            for (int i = TheoreticalSize[1] - 1; i > 0; i--)
            {
                var checksum = 0;
                for (int j = 0; j < TheoreticalSize[0]; j++)
                {
                    if (FilledArea[j, i] != Console.BackgroundColor) checksum++;
                }
                if (checksum == TheoreticalSize[0])
                {
                    OnLineCleared(EventArgs.Empty);
                    ShiftFilledAreaDownStartingFromRow(i);
                    i++;
                }
            }
        }

        private void ShiftFilledAreaDownStartingFromRow(int rowIndex)
        {
            for (int i = rowIndex; i > 0; i--)
            {
                for (int x = 0; x < TheoreticalSize[0]; x++)
                {
                    FilledArea[x, i] = FilledArea[x, i - 1];
                }
            }
            DrawFilledArea();
        }

        public event EventHandler LineCleared;
        private void OnLineCleared(EventArgs e)
        {
            LineCleared?.Invoke(this, e);
        }

        public void DrawFilledArea()
        {
            for (int i = 0; i < TheoreticalSize[0]; i++)
            {
                for (int j = 0; j < TheoreticalSize[1]; j++)
                {
                    Console.SetCursorPosition(i * BoardScaling[0] + 1, j * BoardScaling[1] + 1);
                    Console.ForegroundColor = FilledArea[i, j];
                    Console.Write(Brick.Character);
                }
            }
        }
    }
}
