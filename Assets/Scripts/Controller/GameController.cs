using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;

public class GameController : MonoBehaviour, IController
{
    // View
    public Button[] cellButtons;
    public Button resetButton;
    public TMP_Text xScoreText;
    public TMP_Text oScoreText;
    public TMP_Text drawCountText;
    
    // Model
    private IGameModel gameModel;

    private void Start()
    {
        gameModel = this.GetModel<IGameModel>();
        
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
                var text = cellButtons[i].GetComponentInChildren<TMP_Text>();
                if (text != null)
                {
                    text.text = gameModel.Board.Value[i] switch
                    {
                        CellState.X => "X",
                        CellState.O => "O",
                        _ => ""
                    };
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
