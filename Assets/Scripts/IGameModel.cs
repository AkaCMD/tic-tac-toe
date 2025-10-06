using QFramework;

public interface IGameModel : IModel 
{ 
    BindableProperty<CellState[]> Board { get; }
    BindableProperty<Player> CurrentPlayer { get; }
    BindableProperty<GameResult> Result { get; }
}
