using QFramework;
using UnityEngine;

public class GameModel : AbstractModel, IGameModel
{
    public BindableProperty<CellState[]> Board { get; } = new BindableProperty<CellState[]>();
    public BindableProperty<Player> CurrentPlayer { get; } = new BindableProperty<Player>();
    public BindableProperty<GameResult> Result { get; } = new BindableProperty<GameResult>();
    
    protected override void OnInit()
    {
        int boardSize = 9;
        CellState[] initialBoard = new CellState[boardSize];

        for (int i = 0; i < boardSize; i++)
        {
            initialBoard[i] = CellState.Empty;
        }
        
        Board.SetValueWithoutEvent(initialBoard);
        CurrentPlayer.SetValueWithoutEvent(Player.X);
        Result.SetValueWithoutEvent(GameResult.InProgress);

        Board.Register(board =>
        {
            
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
        Board.Value = new CellState[9];
        CurrentPlayer.Value = Player.X;
        Result.Value = GameResult.InProgress;
    }

    public void SwitchPlayer()
    {
        CurrentPlayer.Value = CurrentPlayer.Value == Player.X ? Player.O : Player.X;
    }
}
