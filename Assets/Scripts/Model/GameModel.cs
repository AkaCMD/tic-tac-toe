using System.Linq;
using QFramework;

public class GameModel : AbstractModel, IGameModel
{
    public BindableProperty<CellState[]> Board { get; } = new BindableProperty<CellState[]>();
    public BindableProperty<Player> CurrentPlayer { get; } = new BindableProperty<Player>();
    public BindableProperty<GameResult> Result { get; } = new BindableProperty<GameResult>();
    public BindableProperty<int> XScore { get; } = new BindableProperty<int>();
    public BindableProperty<int> OScore { get; } = new BindableProperty<int>();
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
        XScore.SetValueWithoutEvent(storage.LoadInt(nameof(XScore)));
        OScore.SetValueWithoutEvent(storage.LoadInt(nameof(OScore)));
        DrawCount.SetValueWithoutEvent(storage.LoadInt(nameof(DrawCount)));
        
        // 当数据变更时，存储数据
        XScore.Register(newScore =>
        {
            storage.SaveInt(nameof(XScore), newScore);
        });
        OScore.Register(newScore =>
        {
            storage.SaveInt(nameof(OScore), newScore);
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
        CurrentPlayer.Value = Player.X;
        Result.Value = GameResult.InProgress;
    }

    public void SwitchPlayer()
    {
        CurrentPlayer.Value = CurrentPlayer.Value == Player.X ? Player.O : Player.X;
    }
}
