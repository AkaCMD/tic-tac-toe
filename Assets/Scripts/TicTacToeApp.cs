using QFramework;

public class TicTacToeApp : Architecture<TicTacToeApp>
{
    protected override void Init()
    {
        // 注册 System
        
        // 注册 Model
        this.RegisterModel<IGameModel>(new GameModel());    
        
        // 注册工具
    }
}
