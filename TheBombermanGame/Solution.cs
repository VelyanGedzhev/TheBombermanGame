using System;
using System.IO;
using System.Text;
using System.Threading;
using static TheBombermanGame.Constants;

namespace TheBombermanGame
{
    class Solution
    {
        private readonly int seconds;
        private readonly char[][] grid;

        public Solution(int seconds, char[][] grid)
        {
            this.seconds = seconds;
            this.grid = grid;
        }

        public void Predict()
        {
            //Return the inital grid, because in the first second Bomberman is standing still
            if (seconds == 1)
            {
                PrepareGrid(grid);
                Console.WriteLine(PrintGrid(grid));
                return;
            }

            //On even seconds the grid is completely filled with bombs
            if (seconds % 2 == 0)
            {
                PlantBombs(grid);
                PrepareGrid(grid);
                Console.WriteLine(PrintGrid(grid));
                return;
            }
            //Simulate all cycles up to a given second and gives the grid state at every cycle
            SimulateAllCyclesUpToGivenSecond();

            //Gives only the final state of the grid at the given second without performing all cycles
            //GetGridStateAtGivenSecond();
        }

        //Blow up the bombs in [row +-1, col] and [row, col +-1] of the currently detonated bomb
        private static void BlowBombs(char[][] grid, int row, int col)
        {
            BlowCell(grid, row + 1, col);
            BlowCell(grid, row - 1, col);
            BlowCell(grid, row, col + 1);
            BlowCell(grid, row, col - 1);
            grid[row][col] = FreeSpaceSymbol;
        }

        private static void BlowCell(char[][] grid, int row, int col)
        {
            //Since bombs are blowing all at once, if there's a bomb about to blow up in the neighbouring cell we skip it until we reach it's cell
            if (!IsValidCell(grid, row, col) || grid[row][col] == OneSecondToBlowBomb)
            {
                return;
            }

            grid[row][col] = FreeSpaceSymbol;
        }

        //Bombs have 3sec fuse by default and will explode after that
        private static void DefuseBombs(char[][] grid)
        {
            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[row].Length; col++)
                {
                    if (char.IsDigit(grid[row][col]))
                    {
                        if (grid[row][col] == OneSecondToBlowBomb)
                        {
                            BlowBombs(grid, row, col);
                            continue;
                        }

                        grid[row][col]--;
                    }
                }
            }
        }

        private void GetGridStateAtGivenSecond()
        {
            PlantBombs(grid);
            DefuseBombs(grid);

            if ((seconds - 5) % 4 == 0)
            {
                PlantBombs(grid);
                DefuseBombs(grid);
            }

            PrepareGrid(grid);
            Console.WriteLine(PrintGrid(grid));
        }

        //New bombs are planted on free cells and the ones on the grid lose 1sec from their fuse
        private static void PlantBombs(char[][] grid)
        {
            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[row].Length; col++)
                {

                    if (char.IsDigit(grid[row][col]))
                    {
                        grid[row][col]--;
                    }
                    else
                    {
                        grid[row][col] = ThreeSecondsToBlowBomb;
                    }
                }
            }
        }

        //Only needed because in HackerRank the output must be in format: [Bomb - O], [FreeSpaceCell - .]
        private static void PrepareGrid(char[][] grid)
        {
            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[row].Length; col++)
                {
                    if (char.IsDigit(grid[row][col]))
                    {
                        grid[row][col] = BombSymbol;
                    }
                }
            }
        }

        private static string PrintGrid(char[][] grid)
        {
            var result = new StringBuilder();
            result.AppendLine();

            for (int row = 0; row < grid.Length; row++)
            {
                result.AppendLine(string.Join("", grid[row]));
            }

            return result.ToString();
        }

        //Checks if the needed cell is in the grid
        private static bool IsValidCell(char[][] grid, int row, int col)
        {
            if (row < 0 || row >= grid.Length)
            {
                return false;
            }

            if (col < 0 || col >= grid[row].Length)
            {
                return false;
            }

            return true;
        }

        private void SimulateAllCyclesUpToGivenSecond()
        {
            File.WriteAllText(ResultFilePath, $"Initial grid:");
            File.AppendAllText(ResultFilePath, PrintGrid(grid));

            for (int s = 2; s <= seconds; s++)
            {
                Console.WriteLine();

                if (s % 2 == 0)
                {
                    PlantBombs(grid);

                    Console.Write($"Grid at {s} seconds:");
                    Console.WriteLine(PrintGrid(grid));
                    File.AppendAllText(ResultFilePath, $"{Environment.NewLine}Grid at {s} seconds:");
                    File.AppendAllText(ResultFilePath, PrintGrid(grid));
                }
                else
                {
                    DefuseBombs(grid);
                    Console.Write($"Grid at {s} seconds:");
                    Console.WriteLine(PrintGrid(grid));
                    File.AppendAllText(ResultFilePath, $"{Environment.NewLine}Grid at {s} seconds:");
                    File.AppendAllText(ResultFilePath, PrintGrid(grid));
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            PrepareGrid(grid);
            Console.WriteLine();
            Console.Write($"Final grid at {seconds} seconds:");
            Console.WriteLine(PrintGrid(grid));
            File.AppendAllText(ResultFilePath, $"{Environment.NewLine}Final grid at {seconds} seconds:");
            File.AppendAllText(ResultFilePath, PrintGrid(grid));
        }
    }
}
