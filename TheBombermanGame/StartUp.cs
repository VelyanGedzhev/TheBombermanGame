using System;
using System.Linq;
using static TheBombermanGame.Constants;

namespace TheBombermanGame
{
    class StartUp
    {
        public static void Main(string[] args)
        {
            string[] userInput = GetInitialConfiguration();

            int rows = int.Parse(userInput[0]);
            int cols = int.Parse(userInput[1]);
            int seconds = int.Parse(userInput[2]);
            char[][] grid = new char[rows][];

            try
            {
                grid = PrepareGrid(rows, cols);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Console.WriteLine(exception.Message);
            }

            Solution solution = new(seconds, grid);
            solution.Predict();
        }

        private static string[] GetInitialConfiguration()
        {
            Console.WriteLine(Rules + Environment.NewLine);
            Console.WriteLine(InitialConfigurationMessage);

            var userInput = Console.ReadLine()
                .TrimEnd()
                .Split();

            while (!ValidateInitialConfiguration(userInput))
            {
                Console.WriteLine(InvalidUserInput);

                userInput = Console.ReadLine()
                    .TrimEnd()
                    .Split();
            }

            return userInput;
        }

        private static char[][] PrepareGrid(int rows, int cols)
        {
            char[][] grid = new char[rows][];

            Console.WriteLine(InitialGridMessage);

            for (int row = 0; row < rows; row++)
            {
                var currentRow = Console.ReadLine().Replace(BombSymbol, TwoSecondToBlowBomb).ToCharArray();

                if (IsRowLengthInvalid(cols, currentRow))
                {
                    throw new ArgumentOutOfRangeException($"{nameof(currentRow)} size invalid! It must be exactly {cols} long.");
                }

                grid[row] = currentRow;
            }

            return grid;
        }

        //Checks if a row from the grid is longer than the length provided by the user
        private static bool IsRowLengthInvalid(int cols, char[] currentRow)
            => currentRow.Length < 0 || currentRow.Length > cols;

        //Checks if the rows, columns and second provided by the user are in correct format '0 0 0'
        private static bool ValidateInitialConfiguration(string[] userInput)
            => userInput.Length == 3 && userInput.All(x => int.TryParse(x, out _));
    }
}

