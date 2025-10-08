using System.Linq;
using QFramework;

/// <summary>
/// 井字棋游戏数据模型
/// 管理游戏状态、玩家信息、棋盘数据和统计信息
/// </summary>
public class GameModel : AbstractModel, IGameModel
{
    public BindableProperty<CellState[]> Board { get; } = new BindableProperty<CellState[]>();
    public BindableProperty<Player> CurrentPlayer { get; } = new BindableProperty<Player>();
    public BindableProperty<Player> AIPlayer { get; } = new BindableProperty<Player>();
    public BindableProperty<GameResult> Result { get; } = new BindableProperty<GameResult>();
    public BindableProperty<int> PlayerScore { get; } = new BindableProperty<int>();
    public BindableProperty<int> AIScore { get; } = new BindableProperty<int>();
    public BindableProperty<int> DrawCount { get; } = new BindableProperty<int>();

    protected override void OnInit()
    {
        var storage = this.GetUtility<IStorage>();
        
        CellState[] initialBoard = new CellState[GameConst.BoardSize];

        for (int i = 0; i < GameConst.BoardSize; i++)
        {
            initialBoard[i] = CellState.Empty;
        }
        
        // 设置初始值（不触发事件）
        Board.SetValueWithoutEvent(initialBoard);
        CurrentPlayer.SetValueWithoutEvent(Player.X);
        Result.SetValueWithoutEvent(GameResult.InProgress);
        PlayerScore.SetValueWithoutEvent(storage.LoadInt(nameof(PlayerScore)));
        AIScore.SetValueWithoutEvent(storage.LoadInt(nameof(AIScore)));
        DrawCount.SetValueWithoutEvent(storage.LoadInt(nameof(DrawCount)));
        AIPlayer.SetValueWithoutEvent(Player.O);
        
        // 当数据变更时，存储数据
        PlayerScore.Register(newScore =>
        {
            storage.SaveInt(nameof(PlayerScore), newScore);
        });
        AIScore.Register(newScore =>
        {
            storage.SaveInt(nameof(AIScore), newScore);
        });
        DrawCount.Register(newCount =>
        {
            storage.SaveInt(nameof(DrawCount), newCount);
        });
    }
    
    // internal helper
    public void SetCell(int index, CellState state)
    {
        var oldBoard = Board.Value;
        var newBoard = (CellState[])oldBoard.Clone();
        newBoard[index] = state;
        Board.Value = newBoard;
    }

    public void Reset()
    {
        Board.Value = Enumerable.Repeat(CellState.Empty, GameConst.BoardSize).ToArray();
        CurrentPlayer.Value = Player.X; // X goes first
        Result.Value = GameResult.InProgress;
    }

    public void SwitchPlayer()
    {
        CurrentPlayer.Value = CurrentPlayer.Value == Player.X ? Player.O : Player.X;
    }
}
