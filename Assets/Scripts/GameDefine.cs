/// <summary>
/// 定义井字棋游戏中的基本枚举类型和常量
/// 包括棋盘状态、玩家类型、游戏结果等核心数据结构
/// </summary>

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
    X_Win,
    O_Win,
    Draw,
    InProgress
}

public static class GameConst
{
    public const int BoardSize = 9;
}