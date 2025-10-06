using QFramework;

public interface IGameRulesSystem : ISystem 
{
    GameResult CheckResult(CellState[] board);
    bool IsCellEmpty(CellState[] board, int index);
    int GetAIMove(CellState[] board, Player aiPlayer);
}
