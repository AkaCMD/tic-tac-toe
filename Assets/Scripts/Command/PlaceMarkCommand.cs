using QFramework;

public class PlaceMarkCommand : AbstractCommand
{
    private readonly int mIndex;
    private readonly Player mPlayer;

    public PlaceMarkCommand(int index, Player player)
    {
        mIndex = index;
        mPlayer = player;
    }
    
    protected override void OnExecute()
    {
        IGameModel model = this.GetModel<IGameModel>();
        IGameRulesSystem system = this.GetSystem<IGameRulesSystem>();

        if (!system.IsCellEmpty(mIndex) || model.Result.Value != GameResult.InProgress) return;
        model.SetCell(mIndex, mPlayer == Player.X ? CellState.X : CellState.O);

        if (model.Result.Value == GameResult.InProgress)
        {
            model.SwitchPlayer();
        }
    }
}
