using UnityEngine;
using QFramework;

public class GameRulesSystem : AbstractSystem, IGameRulesSystem
{
    protected override void OnInit()
    {
        throw new System.NotImplementedException();
    }

    public GameResult CheckResult(CellState[] board)
    {
        throw new System.NotImplementedException();
    }

    public bool IsCellEmpty(CellState[] board, int index)
    {
        throw new System.NotImplementedException();
    }

    public int GetAIMove(CellState[] board, Player aiPlayer)
    {
        throw new System.NotImplementedException();
    }
}
