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
        /// Defines location of score, level and count of lines cleared info 
        /// </summary>
        private const byte NEXT_BRICK_X = 15;
        private const byte NEXT_BRICK_Y = 2;
        private const byte SCORE_X = 15;
        private const byte SCORE_Y = 9;
        private const byte LEVEL_X = 15;
        private const byte LEVEL_Y = 11;
        private const byte LINES_CLEARED_X = 15;
        private const byte LINES_CLEARED_Y = 13;

        private Random _Random = new Random();
        private System.Diagnostics.Stopwatch _Stopwatch = new System.Diagnostics.Stopwatch();
        private bool IsGameOver = false;

        public int Score { get; private set; }
        public int Level { get; private set; }
        public int LinesCleared { get; private set; }

        private BrickBase ActiveBrick { get; set; }
        private BrickBase NextBrick { get; set; }
        private int[,] FieldStatus { get; set; }
        private ConsoleColor[,] FieldColor { get; set; } 

        public Game()
        {
            this.Level = 1;
            this.DrawBoard();
            this.DrawScore();
            this.DrawLevel();
            this.DrawLinesCleared();
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
            Console.Write($"SCORE: {this.Score}");
        }

        /// <summary>
        /// Writes current level to the game board
        /// </summary>
        private void DrawLevel()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(LEVEL_X, LEVEL_Y);
            Console.Write($"LEVEL: {this.Level}");
        }

        /// <summary>
        /// Writes count of cleared lines to the game board
        /// </summary>
        private void DrawLinesCleared()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(LINES_CLEARED_X, LINES_CLEARED_Y);
            Console.Write($"LINES CLEARED: {this.LinesCleared}");
        }

        /// <summary>
        /// Writes next brick to the game board
        /// </summary>
        private void DrawNextBrick()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(NEXT_BRICK_X, NEXT_BRICK_Y);
            Console.WriteLine("NEXT BRICK:");
            Console.WriteLine();
            this.NextBrick.Move(NEXT_BRICK_X + 2, NEXT_BRICK_Y + 2);
            this.NextBrick.Write();
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
            this._Stopwatch.Start();
            this.ActiveBrick = GetRandomBrick();
            this.ActiveBrick.Move(WIDTH / 2, 1);
            this.ActiveBrick.Write();

            this.NextBrick = GetRandomBrick();
            DrawNextBrick();

            while (!IsGameOver)
            {
                if (this.ActiveBrick.Pixels.Any(p => FieldStatus[p[0] - 1, p[1]] == 1))
                {
                    IsGameOver = true;
                    Console.WriteLine("GAME OVER");
                    Console.ReadKey();
                    break;
                }

                ConsoleKey keyPressed;
                if (_Stopwatch.ElapsedMilliseconds > 1000 - this.Level * 100)
                {
                    this.ActiveBrick.Erase();
                    this.ActiveBrick.Move(0, 1);
                    this.ActiveBrick.Write();
                    _Stopwatch.Restart();
                }
                if(Console.KeyAvailable)
                {
                    keyPressed = Console.ReadKey().Key;
                    switch (keyPressed)
                    {
                        case ConsoleKey.DownArrow:
                            if (this.ActiveBrick.Pixels.All(p => p[1] != HEIGHT) &&
                               this.ActiveBrick.Pixels.All(p => FieldStatus[p[0] - 1, p[1]] == 0))
                            {
                                this.ActiveBrick.Erase();
                                this.ActiveBrick.Move(0, 1);
                                this.ActiveBrick.Write();
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            if (this.ActiveBrick.Pixels.All(p => p[0] < WIDTH) &&
                               this.ActiveBrick.Pixels.All(p => FieldStatus[p[0], p[1] - 1] == 0))
                            {
                                this.ActiveBrick.Erase();
                                this.ActiveBrick.Move(1, 0);
                                this.ActiveBrick.Write();
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            if (this.ActiveBrick.Pixels.All(p => p[0] > 1) &&
                               this.ActiveBrick.Pixels.All(p => FieldStatus[p[0] - 2, p[1] - 1] == 0))
                            {
                                this.ActiveBrick.Erase();
                                this.ActiveBrick.Move(-1, 0);
                                this.ActiveBrick.Write();
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            RotateActiveBrick();
                            break;
                        default:
                            break;
                    }
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
                            this.IncreaseLinesCleared();
                            if (this.LinesCleared % 10 == 0)
                            {
                                this.IncreaseLevel();
                            }
                        }
                    }
                    this.NextBrick.Erase();
                    this.ActiveBrick = this.NextBrick;
                    this.ActiveBrick.Move(-NEXT_BRICK_X - 2 + WIDTH /2 , -NEXT_BRICK_Y - 2 + 1);
                    //this.ActiveBrick.Move(WIDTH / 2, 1);
                    this.ActiveBrick.Write();
                    this.NextBrick = GetRandomBrick();
                    this.DrawNextBrick();
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
                    Console.Write('█');
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
        /// Increases count of lines cleared
        /// </summary>
        private void IncreaseLinesCleared()
        {
            this.LinesCleared += 1;
            this.DrawLinesCleared();
        }

        /// <summary>
        /// Rotates current brick 90° counterclockwise.
        /// </summary>
        private void RotateActiveBrick()
        {
            this.ActiveBrick.Erase();
            this.ActiveBrick.Rotate();

            //check and move brick if rotation pushed it out the boundary
            if (this.ActiveBrick.Pixels.Max(p => p[0]) > WIDTH)
            {
                this.ActiveBrick.Move(WIDTH - this.ActiveBrick.Pixels.Max(p => p[0]), 0);
            }
            if (this.ActiveBrick.Pixels.Min(p => p[0]) < 1)
            {
                this.ActiveBrick.Move(-this.ActiveBrick.Pixels.Min(p => p[0])+1, 0);
            }
            if (this.ActiveBrick.Pixels.Min(p => p[1]) < 1)
            {
                this.ActiveBrick.Move(0, -this.ActiveBrick.Pixels.Min(p => p[1]) + 1);
            }

            // before writing brick back to the board it is necessary to check if it fits in new position
            if (this.ActiveBrick.Pixels.Any(p => FieldStatus[p[0] - 1, p[1] - 1] == 1))
            {
                // brick after rotation does collide => move it to avoid collision
                var collidingPixels = this.ActiveBrick.Pixels.Where(p => FieldStatus[p[0] - 1, p[1] - 1] == 1).ToList();
                var xcoords = new List<int>();
                var ycoords = new List<int>();
                foreach (var point in collidingPixels)
                {
                    xcoords.Add(point[0]);
                    ycoords.Add(point[1]);
                }
                if(this.ActiveBrick.Pixels.Max(p => p[0]) + xcoords.Count < WIDTH)
                {
                    this.ActiveBrick.Move(xcoords.Count, 0);
                    // test if this position is not in another collision
                    if (this.ActiveBrick.Pixels.Any(p => FieldStatus[p[0] - 1, p[1] - 1] == 1))
                    {
                        if (this.ActiveBrick.Pixels.Min(p => p[0]) - xcoords.Count > 1)
                        {
                            this.ActiveBrick.Move(-2 * xcoords.Count, 0);
                            //check once again
                            if (this.ActiveBrick.Pixels.Any(p => FieldStatus[p[0] - 1, p[1] - 1] == 1))
                            {
                                // if no position is free, then rotate back to initial rotation
                                this.ActiveBrick.Rotate(); // +90
                                this.ActiveBrick.Rotate(); // +180
                                this.ActiveBrick.Rotate(); // +270
                            }
                        }
                    }
                }
            }
            this.ActiveBrick.Write();
        }

        /// <summary>
        /// Gets a brick with random shape and color.
        /// </summary>
        /// <returns>New brick.</returns>
        private BrickBase GetRandomBrick()
        {
            BrickBase result = null;

            var shape = GetRandomShape();
            var color = GetRandomColor();

            switch (shape)
            {
                case ShapeEnum.Square:
                    result = new SquareBrick(color);
                    break;
                case ShapeEnum.S:
                    result = new SShapedBrick(color);
                    break;
                case ShapeEnum.Z:
                    result = new ZShapedBrick(color);
                    break;
                case ShapeEnum.T:
                    result = new TShapedBrick(color);
                    break;
                case ShapeEnum.Long:
                    result = new LongBrick(color);
                    break;
                case ShapeEnum.J:
                    result = new JShapedBrick(color);
                    break;
                case ShapeEnum.L:
                    result = new LShapedBrick(color);
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// Chooses a random shape for the brick
        /// </summary>
        /// <returns>Random shape.</returns>
        private ShapeEnum GetRandomShape()
        {
            var numberOfAvailableShapes = Enum.GetValues(typeof(ShapeEnum)).Length;
            var randomIndex = _Random.Next(0, numberOfAvailableShapes - 1);
            return (ShapeEnum)randomIndex;
        }

        /// <summary>
        /// Chooses a random color for the brick.
        /// </summary>
        /// <returns>A random ConsoleColor, except black and white.</returns>
        private ConsoleColor GetRandomColor()
        {
            var numberOfAvailableColors = Enum.GetValues(typeof(ConsoleColor)).Length;
            var randomIndex = _Random.Next(1, numberOfAvailableColors - 2);
            return (ConsoleColor)randomIndex;
        }
    }
}
