using QFramework;

public interface IGameModel : IModel 
{ 
    BindableProperty<CellState[]> Board { get; }
    BindableProperty<Player> CurrentPlayer { get; }
    BindableProperty<GameResult> Result { get; }

    void SetCell(int index, CellState state);
    void Reset();
    void SwitchPlayer();
}
