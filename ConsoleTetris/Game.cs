using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    /// <summary>
    /// Game class
    /// </summary>
    class Game
    {
        private Random _Random = new Random();
        private System.Diagnostics.Stopwatch _Stopwatch = new System.Diagnostics.Stopwatch();

        public int Score { get; private set; }
        public int Level { get; private set; }
        public int LinesCleared { get; private set; }

        private Brick ActiveBrick { get; set; }
        private Brick NextBrick { get; set; }
        private Board GameBoard { get; set; }
        private bool IsPaused { get; set; }
        public bool IsOver { get; private set; }

        public Game()
        {
            Level = 1;
            Score = 0;
            LinesCleared = 0;
            GameBoard = new Board(new int[] { 10, 20 }, new int[] { 2, 1 });
            GameBoard.LineCleared += GameBoard_LineCleared;

            UpdateInfo(0, Score.ToString());
            UpdateInfo(1, Level.ToString());
            UpdateInfo(2, LinesCleared.ToString());

            IsOver = false;
        }

        private void GameBoard_LineCleared(object sender, EventArgs e)
        {
            LinesCleared += 1;
            Score += Level * 10;
            UpdateInfo(0, Score.ToString());
            UpdateInfo(2, LinesCleared.ToString());
            if (LinesCleared % 10 == 0)
            {
                Level += 1;
                UpdateInfo(1, Level.ToString());
            }
        }

        private void UpdateInfo(int labelIndex, string text)
        {
            var pair = GameBoard.Labels.ElementAt(labelIndex);
            var x = pair.Value[0] + pair.Key.Length + 1;
            var y = pair.Value[1];
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text);
        }

        /// <summary>
        /// Writes next brick to the game board
        /// </summary>
        private void DrawNextBrick()
        {
            var pair = GameBoard.Labels.ElementAt(3);
            var x = pair.Value[0] + pair.Key.Length / 2;
            var y = pair.Value[1] + 1;

            NextBrick.Move(x / GameBoard.BoardScaling[0], y / GameBoard.BoardScaling[1]);
            NextBrick.Write();
        }


        public void Start()
        {
            _Stopwatch.Start();
            ActiveBrick = new Brick(GetRandomShape(), GetRandomColor(), new int[] { 2, 1 });
            ActiveBrick.Move(GameBoard.TheoreticalSize[0] / 2 - 1, 0);
            ActiveBrick.Write();

            NextBrick = new Brick(GetRandomShape(), GetRandomColor(), new int[] { 2, 1 });
            DrawNextBrick();

            while (!IsOver)
            {
                if (Console.KeyAvailable)
                {
                    ProcessUserInput(Console.ReadKey(true).Key);
                }

                if (IsPaused)
                {
                    GameBoard.DisplayBlinkingPauseMessage((_Stopwatch.ElapsedMilliseconds / 750 ) % 2 == 1); // blink interval = 750ms
                }
                else
                {
                    GameBoard.ClearPauseMessage();
                }

                if (!IsPaused && _Stopwatch.ElapsedMilliseconds > 1000 - Level * 100) // falling down
                {
                    if (IsNewPositionValid(ActiveBrick.GetMovedPixels(0, 1)))
                    {
                        ActiveBrick.Erase();
                        ActiveBrick.Move(0, 1);
                        ActiveBrick.Write();
                    }
                    else
                    {
                        SettleActiveBrick();
                        GameBoard.ClearFilledRows();
                        MakeNextBrickActive();
                        NextBrick = new Brick(GetRandomShape(), GetRandomColor(), new int[] { 2, 1 });
                        DrawNextBrick();
                        if (!IsNewPositionValid(ActiveBrick.Pixels))
                        {
                            IsOver = !IsOver;
                        }
                    }
                    _Stopwatch.Restart();
                }
            }

            Console.SetCursorPosition(GameBoard.Labels.Last().Value[0], GameBoard.Labels.Last().Value[1] + 6);
            Console.ResetColor();
            Console.Write("GAME OVER!");
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

        private void ProcessUserInput(ConsoleKey key)
        {
            var newPixels = new int[ActiveBrick.Pixels.GetLength(0)][];
            Action action = null;
            switch (key)
            {
                case ConsoleKey.DownArrow:
                    action = new Action(() => ActiveBrick.Move(0, 1));
                    newPixels = ActiveBrick.GetMovedPixels(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    action = new Action(() => ActiveBrick.Move(-1, 0));
                    newPixels = ActiveBrick.GetMovedPixels(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    action = new Action(() => ActiveBrick.Move(1, 0));
                    newPixels = ActiveBrick.GetMovedPixels(1, 0);
                    break;
                case ConsoleKey.Spacebar:
                    action = new Action(() => ActiveBrick.Rotate());
                    newPixels = ActiveBrick.GetRotatedPixels();
                    break;
                case ConsoleKey.P:
                    IsPaused = !IsPaused;
                    break;
                default:
                    break;
            }

            if (action != null && !IsPaused)
            {
                if (IsNewPositionValid(newPixels))
                {
                    ActiveBrick.Erase();
                    action.Invoke();
                    ActiveBrick.Write();
                    GameBoard.DrawFrame();
                }
            }
        }

        private void SettleActiveBrick()
        {
            for (int i = 0; i < ActiveBrick.Pixels.Count(); i++)
            {
                var x = ActiveBrick.Pixels[i][0] / GameBoard.BoardScaling[0];
                var y = ActiveBrick.Pixels[i][1] / GameBoard.BoardScaling[1] - 1;
                GameBoard.FilledArea[x, y] = ActiveBrick.Color;
            }
        }

        private void MakeNextBrickActive()
        {
            NextBrick.Erase();
            ActiveBrick = NextBrick;
            var pair = GameBoard.Labels.ElementAt(3);
            var x = pair.Value[0] + pair.Key.Length / 2;
            var y = pair.Value[1] + 1;

            ActiveBrick.Move(-x / GameBoard.BoardScaling[0], -y / GameBoard.BoardScaling[1]); // move to original position
            ActiveBrick.Move(GameBoard.TheoreticalSize[0] / 2 - 1, 0);                        // move to middle top of the board
            
            ActiveBrick.Write();
        }

        private bool IsNewPositionValid(int[][] newPixels)
        {
            var rightLimit = GameBoard.TheoreticalSize[0] * GameBoard.BoardScaling[0];
            var bottomLimit = GameBoard.TheoreticalSize[1] * GameBoard.BoardScaling[1] + 1;
            var leftLimit = 0;
            var upperLimit = 0;

            var boundaryClearance = newPixels.All(pixel => pixel[0] > leftLimit && pixel[0] < rightLimit && pixel[1] > upperLimit && pixel[1] < bottomLimit);
            var filledAreaClearance = false;
            if (boundaryClearance)
            {
                filledAreaClearance = newPixels.All(pixel =>
                {
                    var xLocation = (pixel[0] - 1) / GameBoard.BoardScaling[0];
                    var yLocation = (pixel[1] - 1) / GameBoard.BoardScaling[1];
                    return GameBoard.FilledArea[xLocation, yLocation] == Console.BackgroundColor;
                });
            }

            return boundaryClearance && filledAreaClearance;
        }
    }
}
