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

        private Brick Brick { get; set; }
        private int[,] FilledFields { get; set; }

        public Game()
        {
            this.DrawBoard();
            this.DrawScore();
            this.DrawLevel();
            this.FilledFields = new int[WIDTH, HEIGHT];
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
            Console.SetCursorPosition(SCORE_X, SCORE_Y);
            Console.Write($"SCORE: {Score}");
        }

        /// <summary>
        /// Writes current level to the game board
        /// </summary>
        private void DrawLevel()
        {
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
            this.Brick = new Brick(X: WIDTH/2, Y: 1);

            while (this.Brick != null)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.DownArrow:
                        if(this.Brick.LocationY != HEIGHT && FilledFields[this.Brick.LocationX - 1, this.Brick.LocationY] == 0)
                        {
                            this.Brick.Move(0,1);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if(this.Brick.LocationX < WIDTH && FilledFields[this.Brick.LocationX, this.Brick.LocationY - 1] == 0)
                        {
                            this.Brick.Move(1, 0);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if(this.Brick.LocationX > 1 && FilledFields[this.Brick.LocationX - 2, this.Brick.LocationY - 1] == 0)
                        {
                            this.Brick.Move(-1, 0);
                        }
                        break;
                    default:
                        break;
                }
                RepairRightEdge();
                if(this.Brick.LocationY == HEIGHT || FilledFields[this.Brick.LocationX - 1, this.Brick.LocationY] == 1)
                {
                    this.FilledFields[this.Brick.LocationX - 1, this.Brick.LocationY - 1] = 1;
                    this.Brick = new Brick(X: WIDTH / 2, Y: 1);
                }
                
            }
        }
    }
}
