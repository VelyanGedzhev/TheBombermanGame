using System;
using System.Text;
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
                Console.WriteLine(PrintGrid(grid, seconds));
                return;
            }

            //On even seconds the grid is completely filled with bombs
            if (seconds % 2 == 0)
            {
                PlantBombs(grid);
                PrepareGrid(grid);
                Console.WriteLine(PrintGrid(grid, seconds));
                return;
            }

            //Gives only the final state of the grid at the given second without performing all cycles
            GetGridStateAtGivenSecond();
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
            Console.WriteLine(PrintGrid(grid, seconds));
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

        private static string PrintGrid(char[][] grid, int seconds)
        {
            var secondsText = seconds == 1 ? "second" : "seconds";

            var result = new StringBuilder();
            result
                .AppendLine()
                .AppendLine($"Grid state at '{seconds}' {secondsText}:");

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
    }
}
