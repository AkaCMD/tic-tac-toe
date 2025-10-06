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

        CurrentPlayer.Register(player =>
        {
            Debug.Log("Current Player: " + player.ToString());
        });
    }
}
