using System.Linq;
using QFramework;

public class GameModel : AbstractModel, IGameModel
{
    public BindableProperty<CellState[]> Board { get; } = new BindableProperty<CellState[]>();
    public BindableProperty<Player> CurrentPlayer { get; } = new BindableProperty<Player>();
    public BindableProperty<GameResult> Result { get; } = new BindableProperty<GameResult>();
    
    protected override void OnInit()
    {
        CellState[] initialBoard = new CellState[GameConst.BoardSize];

        for (int i = 0; i < GameConst.BoardSize; i++)
        {
            initialBoard[i] = CellState.Empty;
        }
        
        Board.SetValueWithoutEvent(initialBoard);
        CurrentPlayer.SetValueWithoutEvent(Player.X);
        Result.SetValueWithoutEvent(GameResult.InProgress);
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
