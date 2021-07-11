using System;
using System.Text;
using static TheBombermanGame.Constants;

namespace TheBombermanGame
{
    public class Solution
    {
        private readonly int second;
        private readonly char[][] grid;

        public Solution(int second, char[][] grid)
        {
            this.second = second;
            this.grid = grid;
        }

        public void Predict()
        {
            //Return the inital grid, because in the first second Bomberman is standing still
            if (second == OneSecond)
            {
                PrepareGrid(grid);
                PrintGrid(grid, second);
                return;
            }

            //On even seconds the grid is completely filled with bombs
            if (second % TwoSeconds == ZeroRemainder)
            {
                PlantBombs(grid);
                PrepareGrid(grid);
                PrintGrid(grid, second);
                return;
            }

            //Gives only the final state of the grid at the given second without performing all cycles
            GetGridStateAtGivenSecond();
        }

        //Blow up the bombs in [row +-1, col] and [row, col +-1] of the currently detonated bomb
        private static void BlowBombs(char[][] grid, int row, int col)
        {
            BlowCell(grid, row + NeighbouringCellIndex, col);
            BlowCell(grid, row - NeighbouringCellIndex, col);
            BlowCell(grid, row, col + NeighbouringCellIndex);
            BlowCell(grid, row, col - NeighbouringCellIndex);
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
            //returns the grid state for seconds in 'pattern3' - 3s, 7s, 11s and etc
            PlantBombs(grid);
            DefuseBombs(grid);

            //returns the grid state for seconds in 'pattern5' - 5s, 9s, 13s and etc
            if ((second - FifthSecond) % FourSeconds == ZeroRemainder)
            {
                PlantBombs(grid);
                DefuseBombs(grid);
            }

            PrepareGrid(grid);
            PrintGrid(grid, second);
        }

        //New bombs with 3sec fuse are planted on free cells and the ones on the grid lose 1sec from their fuse
        private static void PlantBombs(char[][] grid)
        {
            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[row].Length; col++)
                {
                    if (char.IsDigit(grid[row][col]))
                    {
                        //decrease fuse by 1sec
                        grid[row][col]--;
                    }
                    else
                    {
                        //plant new bomb
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

        private static void PrintGrid(char[][] grid, int seconds)
        {
            var secondsText = seconds == OneSecond ? SingleSecondText : MultipleSeconds;

            var result = new StringBuilder();
            result
                .AppendLine()
                .AppendLine($"Grid state at '{seconds}' {secondsText}:");

            for (int row = 0; row < grid.Length; row++)
            {
                result.AppendLine(string.Join("", grid[row]));
            }

            Console.WriteLine(result.ToString());
        }

        //Checks if the needed cell is in the grid
        private static bool IsValidCell(char[][] grid, int row, int col)
        {
            if (row < ZeroIndex || row >= grid.Length)
            {
                return false;
            }

            if (col < ZeroIndex || col >= grid[row].Length)
            {
                return false;
            }

            return true;
        }
    }
}
