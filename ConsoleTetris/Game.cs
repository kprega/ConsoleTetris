using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    class Game
    {
        /// <summary>
        /// Defines size of Tetris game board
        /// </summary>
        private const byte WIDTH = 10;
        private const byte HEIGHT = 20;

        /// <summary>
        /// Defines location of score and level info 
        /// </summary>
        private const byte SCORE_X = 15;
        private const byte SCORE_Y = 9;
        private const byte LEVEL_X = 15;
        private const byte LEVEL_Y = 11;

        public int Score { get; private set; }
        public int Level { get; private set; }

        private Brick ActiveBrick { get; set; }
        private int[,] FieldStatus { get; set; }
        private ConsoleColor[,] FieldColor { get; set; } 

        public Game()
        {
            this.Level = 1;
            this.DrawBoard();
            this.DrawScore();
            this.DrawLevel();
            this.FieldStatus = new int[WIDTH, HEIGHT];
            this.FieldColor = new ConsoleColor[WIDTH, HEIGHT];
            this.Start();
        }

        /// <summary>
        /// Draws a frame around game area
        /// </summary>
        private void DrawBoard()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            var board = new StringBuilder();
            board.Insert(0, "╔").Append('═', WIDTH).Append('╗').AppendLine();
            for (int i = 0; i < HEIGHT; i++)
            {
                board.Append('║').Append(' ', WIDTH).Append('║').AppendLine();
            }
            board.Append('╚').Append('═', WIDTH).Append('╝');
            Console.Write(board);
        }

        /// <summary>
        /// Writes current score to the game board
        /// </summary>
        private void DrawScore()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(SCORE_X, SCORE_Y);
            Console.Write($"SCORE: {Score}");
        }

        /// <summary>
        /// Writes current level to the game board
        /// </summary>
        private void DrawLevel()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(LEVEL_X, LEVEL_Y);
            Console.Write($"LEVEL: {Level}");
        }

        /// <summary>
        /// Redraws right edge of board
        /// </summary>
        private void RepairRightEdge()
        {
            for (int i = 0; i < HEIGHT - 2; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(WIDTH + 1, i + 1);
                Console.Write('║');
            }
        }

        public void Start()
        {
            this.ActiveBrick = new Brick(X: WIDTH/2, Y: 1);

            while (this.ActiveBrick != null)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.DownArrow:
                        if(this.ActiveBrick.Pixels.All(p => p[1] != HEIGHT) && 
                           this.ActiveBrick.Pixels.All(p => FieldStatus[p[0] - 1, p[1]] == 0))
                        {
                            this.MoveActiveBrick(0,1);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if(this.ActiveBrick.Pixels.All(p => p[0] < WIDTH) &&
                           this.ActiveBrick.Pixels.All(p => FieldStatus[p[0], p[1] - 1] == 0))
                        {
                            this.MoveActiveBrick(1, 0);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if(this.ActiveBrick.Pixels.All(p => p[0] > 1) &&
                           this.ActiveBrick.Pixels.All(p => FieldStatus[p[0] - 2, p[1] - 1] == 0))
                        {
                            this.MoveActiveBrick(-1, 0);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        RotateActiveBrick();
                        break;
                    default:
                        break;
                }

                RepairRightEdge();

                if (this.ActiveBrick.Pixels.Any(p => p[1] == HEIGHT) ||
                    this.ActiveBrick.Pixels.Any(p => FieldStatus[p[0] - 1, p[1]] == 1))
                {
                    foreach (int[] point in this.ActiveBrick.Pixels)
                    {
                        this.FieldStatus[point[0] - 1, point[1] - 1] = 1;
                        this.FieldColor[point[0] - 1, point[1] - 1] = this.ActiveBrick.Color;
                    }
                    
                    for (int y = 0; y < HEIGHT; y++)
                    {
                        if (this.CheckLine(y))
                        {
                            this.ClearLine(y);
                            this.ShiftBricks(y);
                            this.IncreaseScore();
                            if (this.Score % (this.Level * 100) == 0)
                            {
                                this.IncreaseLevel();
                            }
                        }
                    }
                    this.ActiveBrick = new Brick(X: WIDTH / 2, Y: 1);
                }
            }
        }

        /// <summary>
        /// Checks if given line is complete
        /// </summary>
        /// <param name="y">Index of line to be checked</param>
        /// <returns>True if line is complete</returns>
        private bool CheckLine(int y)
        {
            var rowSum = 0;
            for (int x = 0; x < WIDTH; x++)
            {
                rowSum += this.FieldStatus[x, y];
            }
            return rowSum == WIDTH;
        }

        /// <summary>
        /// Clears given line
        /// </summary>
        /// <param name="y">Index of line to be cleared</param>
        private void ClearLine(int y)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                this.FieldStatus[x, y] = 0;
                Console.SetCursorPosition(x + 1, y + 1);
                Console.Write(' ');
            }
        }

        /// <summary>
        /// Shifts bricks remaining on board, when a line is cleared. 
        /// </summary>
        /// <param name="y">Index of cleared line.</param>
        private void ShiftBricks(int y)
        {
            for (int i = y; i > 0; i--)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    this.FieldStatus[x, i] = this.FieldStatus[x, i - 1];
                    this.FieldColor[x, i] = this.FieldColor[x, i - 1];
                    Console.SetCursorPosition(x + 1, i + 1);
                    Console.ForegroundColor = this.FieldColor[x, i];
                    Console.Write(this.ActiveBrick.Pixel);
                }
            }
        }

        /// <summary>
        /// Increases score
        /// </summary>
        private void IncreaseScore()
        {
            this.Score += this.Level * 10;
            this.DrawScore();
        }

        /// <summary>
        /// Increases level
        /// </summary>
        private void IncreaseLevel()
        {
            this.Level += 1;
            this.DrawLevel();
        }

        /// <summary>
        /// Moves current brick using given displacement parameters.
        /// </summary>
        /// <param name="dX">Number of pixels to move right.</param>
        /// <param name="dY">Number of pixels to move down.</param>
        private void MoveActiveBrick(int dX, int dY)
        {
            this.ActiveBrick.Erase();
            foreach (int[] point in this.ActiveBrick.Pixels)
            {
                point[0] += dX;
                point[1] += dY;
            }
            this.ActiveBrick.Write();
        }

        /// <summary>
        /// Rotates current brick 90° counterclockwise.
        /// </summary>
        private void RotateActiveBrick()
        {
            this.ActiveBrick.Erase();
            this.ActiveBrick.Rotate();
            this.ActiveBrick.Write();
        }
    }
}
