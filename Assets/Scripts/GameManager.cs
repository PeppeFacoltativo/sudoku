using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    SudokuBoardGameLogic m_GameLogic;

    [SerializeField] private ViewManager view;
    [SerializeField] private List<string> jsonBoardNames; //I preferred to put a list here so it will be easier to choose among a wider range of boards

    private bool gameStarted = false;
    private float timeElapsed;

    public void startGame(int boardNo)
    {
        string path =  Application.dataPath + "/Boards/" + jsonBoardNames[boardNo];

        m_GameLogic = new SudokuBoardGameLogic();
        m_GameLogic.StartGame(path);
        gameStarted = true;
        view.initializeGame(m_GameLogic.GetBoard());
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

    public void checkGameOver()
    {
        if(m_GameLogic.GetBoard().IsBoardFull())
        {
            gameStarted = false;

            int score;
            if (timeElapsed < 100000) //min score is 1
                score = 100000 - Mathf.RoundToInt(timeElapsed);
            else
                score = 1;
            view.gameOver(score);

            if (score > PlayerPrefs.GetInt("HighScore"));
                PlayerPrefs.SetInt("HighScore", score);
        }
    }

    public void quit()
    {
        gameStarted = false;
        timeElapsed = 0;
        view.quit();
    }
}
