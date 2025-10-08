using QFramework;

/// <summary>
/// 重置当前游戏状态的命令
/// 清空棋盘并重新开始游戏，如果 AI 是先手则触发 AI 移动
/// </summary>
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
