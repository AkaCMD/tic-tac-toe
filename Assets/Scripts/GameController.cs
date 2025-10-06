using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

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
    }

    public IArchitecture GetArchitecture()
    {
        return TicTacToeApp.Interface;
    }
}
