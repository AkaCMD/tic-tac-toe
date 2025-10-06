public enum CellState
{
    Empty = 0, 
    X = 1, 
    O = 2
}

public enum Player
{
    X = 1, 
    O = 2
}

public enum GameResult
{
    None,
    X_Win,
    O_Win,
    Draw,
    InProgress
}

public static class GameConst
{
    public const int BoardSize = 9;
}