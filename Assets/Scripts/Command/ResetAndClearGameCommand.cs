using QFramework;

/// <summary>
/// 重置并清除游戏统计数据的命令
/// 将所有分数和计数器重置为零，并重新开始新游戏
/// </summary>
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