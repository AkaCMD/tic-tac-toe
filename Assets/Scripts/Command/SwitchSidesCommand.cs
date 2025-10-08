using QFramework;

/// <summary>
/// 切换玩家和 AI 角色的命令
/// 交换玩家和 AI 控制的棋子(X 或 O)
/// </summary>
public class SwitchSidesCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        var model = this.GetModel<IGameModel>();
        model.AIPlayer.Value = model.AIPlayer.Value == Player.X ? Player.O : Player.X;
    }
}