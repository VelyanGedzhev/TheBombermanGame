namespace TheBombermanGame
{
    public static class Constants
    {
        public const string Rules = @"1.Initially, Bomberman arbitrarily plants bombs in some of the cells, the initial state.
2.After one second, Bomberman does nothing.
3.After one more second, Bomberman plants bombs in all cells without bombs, thus filling the whole grid with bombs. No bombs detonate at this point.
4.After one more second, any bombs planted exactly three seconds ago will detonate. Here, Bomberman stands back and observes.
5.Bomberman then repeats steps 3 and 4 indefinitely.";

        public const string InitialConfigurationMessage = "Please enter rows, columns and second. The input format is a single line with a single space between each value.";
        public const string InitialGridMessage = "Please enter your initial grid, which must comply with the chosen rows and columns count.";
        public const string InvalidUserInput = "Invalid game arguments. You should provide number of rows, columns and second in the following format - 'r c s'. Only integer numbers are allowed.";

        public const char BombSymbol = 'O';
        public const char OneSecondToBlowBomb = '1';
        public const char TwoSecondToBlowBomb = '2';
        public const char ThreeSecondsToBlowBomb = '3';
        public const char FreeSpaceSymbol = '.';

        public const string ResultFilePath = "../../../Result.txt";
    }
}
