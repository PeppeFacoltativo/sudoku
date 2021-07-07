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
        view.displayTime(timeElapsed);
    }

    public void showHint()
    {
        List<int> hint = m_GameLogic.calculateHint();
        updateCell(hint[0], hint[1], hint[2]);
        view.showHint(hint[0], hint[1], hint[2]);
    }

    public bool isGameOver()
    {
        return m_GameLogic.GetBoard().IsBoardFull();
    }
}
