using QFramework;

public class ResetGameCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        var model = this.GetModel<IGameModel>(); 
        model.Reset();
        if (model.CurrentPlayer.Value == model.AIPlayer.Value)
        {
            this.SendEvent<GameRulesSystem.AIMoveEvent>();
        }
    }
}
