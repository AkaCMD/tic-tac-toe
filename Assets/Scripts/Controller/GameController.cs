using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;

public class GameController : MonoBehaviour, IController
{
    // View
    public Button[] cellButtons;
    public Button resetButton;
    public TMP_Text xScoreText, oScoreText, drawCountText;

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
        
        resetButton = transform.Find("Reset").GetComponent<Button>();
        
        // Add listener
        resetButton.onClick.AddListener(() =>
        {
            gameModel.XScore.Value = 0;
            gameModel.OScore.Value = 0;
            gameModel.DrawCount.Value = 0;
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
                this.SendCommand(new PlaceMarkCommand(idx));
            });
        }

        // Update view
        gameModel.Board.RegisterWithInitValue(board =>
        {
            UpdateBoardView();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        gameModel.XScore.RegisterWithInitValue(value =>
        {
            xScoreText.text = value.ToString();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        gameModel.OScore.RegisterWithInitValue(value =>
        {
            oScoreText.text = value.ToString();
        });
        gameModel.DrawCount.RegisterWithInitValue(value =>
        {
            drawCountText.text = value.ToString();
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
