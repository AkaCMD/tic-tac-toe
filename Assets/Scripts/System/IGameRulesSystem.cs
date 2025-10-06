using QFramework;

public interface IGameRulesSystem : ISystem 
{
    GameResult CheckResult();
    bool IsCellEmpty(int index);
    int GetAIMove(Player aiPlayer);
}
