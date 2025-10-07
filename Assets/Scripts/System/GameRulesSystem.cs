using System.Linq;
using QFramework;
using UnityEngine;

public class GameRulesSystem : AbstractSystem, IGameRulesSystem, ICanSendCommand
{
    private IGameModel model;
    
    // 所有可能的胜利情况（行、列、对角线）
    private readonly int[,] winLines = new int[,]
    {
        {0, 1, 2}, {3, 4, 5}, {6, 7, 8},    // 行
        {0, 3, 6}, {1, 4, 7}, {2, 5, 8},    // 列
        {0, 4, 8}, {2, 4, 6}                // 对角线
    };
    
    protected override void OnInit()
    {
        model = this.GetModel<IGameModel>();
        
        model.Board.Register(board =>
        {
            GameResult result = CheckResult();
            if (result != GameResult.InProgress)
            {
                Debug.Log("It's over, " + "Result: " + result);
                switch(result)
                {
                    case GameResult.Draw:
                        model.DrawCount.Value++;
                        break;
                    case GameResult.X_Win:
                        model.XScore.Value++;
                        break;
                    case GameResult.O_Win:
                        model.OScore.Value++;
                        break;
                    default:
                        Debug.LogWarning("Unknown game result: " + result);
                        break;
                }
            }
            model.Result.Value = result;
        });
        
        model.CurrentPlayer.Register(cur =>
        {
            if (cur == Player.O && model.Result.Value == GameResult.InProgress)
            {
                this.SendCommand<PlaceMarkCommand>(new PlaceMarkCommand(GetAIMove(cur))); 
            }
        });
    }

    public GameResult CheckResult()
    {
        var board = model.Board.Value;
        // 有人胜利
        for (int i = 0; i < winLines.GetLength(0); i++)
        {
            int a = winLines[i, 0], b = winLines[i, 1], c = winLines[i, 2];
            if (board[a] != CellState.Empty && board[a] == board[b] && board[b] == board[c])
            {
                return board[a] == CellState.X ? GameResult.X_Win : GameResult.O_Win;
            }
        }
        
        // 平局
        if (board.All(cell => cell != CellState.Empty)) return GameResult.Draw;
        return GameResult.InProgress;
    }

    public bool IsCellEmpty(int index)
    {
        return model.Board.Value[index] == CellState.Empty;
    }

    // AI：优先能赢的一步，其次阻挡对手，其次随机
    public int GetAIMove(Player aiPlayer)
    {
        var board = model.Board.Value;
        CellState myMark = (aiPlayer == Player.X) ? CellState.X : CellState.O;
        CellState oppMark = (aiPlayer == Player.X) ? CellState.O : CellState.X;
        
        // 1. 尝试获胜：检查每个空位是否能立即获胜
        for (int i = 0; i < GameConst.BoardSize; i++)
        {
            if (board[i] == CellState.Empty)
            {
                // 模拟放置棋子
                board[i] = myMark;
                var result = CheckResult();
                board[i] = CellState.Empty;
                
                // 如果这一步能获胜，直接选择
                if ((result == GameResult.X_Win && aiPlayer == Player.X) || 
                    (result == GameResult.O_Win && aiPlayer == Player.O))
                {
                    return i;
                }
            }
        }
        
        // 2. 尝试阻挡：检查对手下一步能否获胜
        for (int i = 0; i < GameConst.BoardSize; i++)
        {
            if (board[i] == CellState.Empty)
            {
                // 模拟对手放置棋子
                board[i] = oppMark;
                var result = CheckResult();
                board[i] = CellState.Empty;
                
                // 如果对手这一步能获胜，必须阻挡
                if ((result == GameResult.X_Win && aiPlayer == Player.O) ||
                    (result == GameResult.O_Win && aiPlayer == Player.X))
                {
                    return i;
                }
            }
        }
        
        // 3. 随机选择其他位置（并非最佳策略，应该优先中心、角落、边，但是需要平衡游戏）
        int[] prefer = {4, 0, 2, 6, 8, 1, 3, 5, 7};
        // foreach (var idx in prefer)
        // {
        //     if (board[idx] == CellState.Empty)
        //         return idx;
        // }
        var availableCells = prefer.Where(cellIdx => board[cellIdx] == CellState.Empty).ToList();
        if (availableCells.Count > 0)
        {
            return availableCells[Random.Range(0, availableCells.Count)];
        }

        return -1;  // inaccessible
    }
}
