using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;

/// <summary>
/// 井字棋游戏的主要控制器
/// 管理 UI 交互、处理用户输入并更新游戏视图
/// </summary>
public class GameController : MonoBehaviour, IController
{
    // View
    public Button[] cellButtons;
    public Button resetButton, reverseButton;
    public TMP_Text playerScoreText, aiScoreText, drawCountText;
    public TMP_Text playerInfoText, aiInfoText;

    private Sprite spriteX, spriteO;
    // Model
    private IGameModel gameModel;

    private void Start()
    {
        gameModel = this.GetModel<IGameModel>();
        var loader = this.GetUtility<IResourceLoader>();
        
        // Load assets
        loader.LoadSprite("x", sprite => spriteX = sprite);
        loader.LoadSprite("o", sprite => spriteO = sprite);
        
        resetButton = transform.Find("ResetBtn").GetComponent<Button>();
        reverseButton = transform.Find("ReverseBtn").GetComponent<Button>();
        
        // Add listener
        resetButton.onClick.AddListener(() =>
        {
            this.SendCommand<ResetAndClearGameCommand>(new ResetAndClearGameCommand());
        });
        
        reverseButton.onClick.AddListener(() =>
        {
            this.SendCommand<SwitchSidesCommand>(new SwitchSidesCommand());
            this.SendCommand<ResetGameCommand>(new ResetGameCommand());
        });

        for (int i = 0; i < cellButtons.Length; i++)
        {
            int idx = i;
            cellButtons[i].onClick.AddListener(() =>
            {
                // 已经下完一局后点击棋盘就自动开始下一局
                if (gameModel.Result.Value != GameResult.InProgress)
                {
                    this.SendCommand<ResetGameCommand>(new ResetGameCommand());
                    return;
                }
                if (gameModel.CurrentPlayer.Value == gameModel.AIPlayer.Value)
                {
                    Debug.LogWarning("It's AI's turn!");
                    return;
                }
                this.SendCommand(new PlaceMarkCommand(idx));
            });
        }

        // Update view
        gameModel.Board.RegisterWithInitValue(board =>
        {
            UpdateBoardView();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        // Update scores
        gameModel.PlayerScore.RegisterWithInitValue(value =>
        {
            playerScoreText.text = value.ToString();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        gameModel.AIScore.RegisterWithInitValue(value =>
        {
            aiScoreText.text = value.ToString();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        gameModel.DrawCount.RegisterWithInitValue(value =>
        {
            drawCountText.text = value.ToString();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        gameModel.AIPlayer.RegisterWithInitValue(ai =>
        {
            var aiMark = gameModel.AIPlayer.Value;
            var playerMark = aiMark == Player.O ? Player.X : Player.O;
            playerInfoText.text = $"{playerMark}:Player";
            aiInfoText.text = $"{aiMark}:Computer";
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<GameRulesSystem.AIMoveEvent>(e =>
        {
            Debug.Log("AI Move Event");
            this.SendCommand<PlaceMarkCommand>(new PlaceMarkCommand(this.GetSystem<IGameRulesSystem>().GetAIMove()));
            UpdateBoardView();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    void UpdateBoardView()
    {
        for (int i = 0; i < cellButtons.Length; i++)
        {
            if (cellButtons[i] != null)
            {
                var image = cellButtons[i].transform.GetChild(0).GetComponent<Image>();
                if (image != null)
                {
                    var sprite = gameModel.Board.Value[i] switch
                    {
                        CellState.X => spriteX,
                        CellState.O => spriteO,
                        _ => null
                    };
                    image.sprite = sprite;
                    image.enabled = (sprite != null);
                }
            }
        }
    }

    public IArchitecture GetArchitecture()
    {
        return TicTacToeApp.Interface;
    }

    private void Destroy()
    {
        gameModel = null;
    }
}
