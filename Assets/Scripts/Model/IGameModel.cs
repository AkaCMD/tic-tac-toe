using QFramework;

public interface IGameModel : IModel 
{ 
    BindableProperty<CellState[]> Board { get; }
    BindableProperty<Player> CurrentPlayer { get; }
    BindableProperty<Player> AIPlayer { get; }
    BindableProperty<GameResult> Result { get; }
    BindableProperty<int> PlayerScore { get; }
    BindableProperty<int> AIScore { get; }
    BindableProperty<int> DrawCount { get; }

    void SetCell(int index, CellState state);
    void Reset();
    void SwitchPlayer();
}
