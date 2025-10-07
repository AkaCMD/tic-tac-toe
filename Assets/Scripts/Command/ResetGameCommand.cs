using QFramework;

public class ResetGameCommand : AbstractCommand
{
    protected override void OnExecute()
    {
       this.GetModel<IGameModel>().Reset();
    }
}
