using QFramework;

public class ResetAndClearGameCommand : AbstractCommand
{
    protected override void OnExecute()
    {
       IGameModel model = this.GetModel<IGameModel>();
       model.XScore.Value = 0;
       model.OScore.Value = 0;
       model.DrawCount.Value = 0;
       this.SendCommand<ResetGameCommand>();
    }
}