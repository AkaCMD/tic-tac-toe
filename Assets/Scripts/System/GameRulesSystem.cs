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
        
        // 3. 策略选择：按优先级选择位置
        // 优先选择中心
        if (board[4] == CellState.Empty)
        {
            return 4;
        } 
        
        // 其次选择角落
        int[] corners = {0, 4, 6, 8};
        var availableCorners = corners.Where(corner => board[corner] == CellState.Empty).ToList();
        if (availableCorners.Count > 0)
        {
            return availableCorners[UnityEngine.Random.Range(0, availableCorners.Count)];
        }
        
        // 最后选择边
        int[] sides = {1, 3, 5, 7};
        var availableSides = sides.Where(side => board[side] == CellState.Empty).ToList();
        if (availableSides.Count > 0)
        {
            return availableSides[UnityEngine.Random.Range(0, availableSides.Count)];
        }

        return -1;  // inaccessible
    }
}
