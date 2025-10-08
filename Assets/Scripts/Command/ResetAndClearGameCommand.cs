using QFramework;

public class ResetAndClearGameCommand : AbstractCommand
{
    protected override void OnExecute()
    {
       IGameModel model = this.GetModel<IGameModel>();
       model.PlayerScore.Value = 0;
       model.AIScore.Value = 0;
       model.DrawCount.Value = 0;
       model.AIPlayer.Value = Player.O;
       this.SendCommand<ResetGameCommand>();
    }
}