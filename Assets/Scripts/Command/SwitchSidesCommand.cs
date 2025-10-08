using QFramework;

public class SwitchSidesCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        var model = this.GetModel<IGameModel>();
        model.AIPlayer.Value = model.AIPlayer.Value == Player.X ? Player.O : Player.X;
    }
}