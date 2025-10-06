using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using UnityEngine.PlayerLoop;

public class GameController : MonoBehaviour, IController
{
    // View
    public Button[] cellButtons;
    public Text statusText;
    public Button resetButton;
    
    // Model
    private IGameModel gameModel;

    private void Start()
    {
        gameModel = this.GetModel<IGameModel>();

        for (int i = 0; i < cellButtons.Length; i++)
        {
            int idx = i;
            cellButtons[i].onClick.AddListener(() =>
            {
                this.SendCommand(new PlaceMarkCommand(idx, gameModel.CurrentPlayer.Value));
            });
        }

        gameModel.Board.RegisterWithInitValue(board =>
        {
            UpdateView();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    void UpdateView()
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
