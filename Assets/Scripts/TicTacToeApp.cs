using QFramework;
using UnityEngine;

public class TicTacToeApp : Architecture<TicTacToeApp>
{
    protected override void Init()
    {
        // 注册 System
        this.RegisterSystem<IGameRulesSystem>(new GameRulesSystem());
        
        // 注册 Model
        this.RegisterModel<IGameModel>(new GameModel());    
        
        // 注册 Utility
        this.RegisterUtility<IStorage>(new Storage());
        this.RegisterUtility<IResourceLoader>(new ResourceLoader());
    }

    // Command 拦截，打印日志
    protected override void ExecuteCommand(ICommand command)
    {
        base.ExecuteCommand(command);
        Debug.Log(command.GetType() + " Executed");
    }
}
