using QFramework;

/// <summary>
/// 处理玩家或 AI 在棋盘上放置棋子的命令
/// 验证位置是否可用并更新游戏状态
/// </summary>
public class PlaceMarkCommand : AbstractCommand
{
    private readonly int mIndex;

    public PlaceMarkCommand(int index)
    {
        mIndex = index;
    }
    
    protected override void OnExecute()
    {
        IGameModel model = this.GetModel<IGameModel>();
        IGameRulesSystem system = this.GetSystem<IGameRulesSystem>();

        if (!system.IsCellEmpty(mIndex) || model.Result.Value != GameResult.InProgress) return;
        model.SetCell(mIndex, model.CurrentPlayer.Value == Player.X ? CellState.X : CellState.O);

        if (model.Result.Value == GameResult.InProgress)
        {
            model.SwitchPlayer();
        }
    }
}
