using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    SudokuBoardGameLogic m_GameLogic;
    [SerializeField]
    ViewManager view;

    private bool gameStarted = false;
    private float timeElapsed;

    void Start()
    {
        startGame();
    }

    private void startGame()
    {
        m_GameLogic = new SudokuBoardGameLogic();
        m_GameLogic.StartGame();
        gameStarted = true;
        view.setUpBoard(m_GameLogic.GetBoard());
    }


    public void updateCell(int row, int column, int tileValue)
    {
        m_GameLogic.GetBoard().SetSudokuValue(row, column, tileValue);
    }

    public bool validateCell(int row, int column, int tileValue)
    {
        return m_GameLogic.GetBoard().TestSudokuValueValidity(row, column, tileValue);
    }

    private void Update()
    {
        if (gameStarted)
            refreshTimer();
    }

    private void refreshTimer()
    {
        timeElapsed += Time.deltaTime;
        view.DisplayTime(timeElapsed);
    }
}
