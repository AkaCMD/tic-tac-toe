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
        resetButton.onClick.AddListener(() =>
        {
            this.SendCommand<ResetGameCommand>(new ResetGameCommand());
        });

        for (int i = 0; i < cellButtons.Length; i++)
        {
            int idx = i;
            cellButtons[i].onClick.AddListener(() =>
            {
                this.SendCommand(new PlaceMarkCommand(idx));
            });
        }

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
}
