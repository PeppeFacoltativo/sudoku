using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxHints;
    private int hintsUsed;

    SudokuBoardGameLogic m_GameLogic;

    [SerializeField] private ViewManager view;
    [SerializeField] private List<string> jsonBoardNames; //keeping a list here will make it easier to choose among a wider range of boards

    private bool timerStarted = false;
    private float timeElapsed;

    /// <summary>
    /// Opens the link for instructions
    /// </summary>
    /// <param name="boardNo">The ID number of the board to show</param>
    public void startGame(int boardNo)
    {
        string path = "Boards/" + jsonBoardNames[boardNo];
        hintsUsed = 0;

        //Initialize the board
        m_GameLogic = new SudokuBoardGameLogic();
        m_GameLogic.StartGame(path);

        //Initialize the view
        view.initializeBoard(m_GameLogic.GetBoard());
        view.refreshHintsLeft(maxHints);
    }

    /// <summary>
    /// Starts the game timer
    /// </summary>
    public void startTimer()
    {
        timerStarted = true;
    }

    /// <summary>
    /// Updates the timer both in game logic and in view
    /// </summary>
    private void refreshTimer()
    {
        timeElapsed += Time.deltaTime;
        view.displayTime(timeElapsed);
    }

    /// <summary>
    /// Updates a cell value in the Model
    /// </summary>
    public void updateCell(int row, int column, int tileValue)
    {
        m_GameLogic.GetBoard().SetSudokuValue(row, column, tileValue);
    }

    /// <summary>
    /// Checks if a cell value inserted is valid
    /// </summary>
    /// <returns>The validity of the insterted number</returns>
    public bool validateCell(int row, int column, int tileValue)
    {
        return m_GameLogic.GetBoard().TestSudokuValueValidity(row, column, tileValue);
    }

    private void Update()
    {
        if (timerStarted)
            refreshTimer();
    }

    /// <summary>
    /// Calculates a hint and shows it in view
    /// </summary>
    public void showHint()
    {
        if (hintsUsed < maxHints)
        {
            hintsUsed++;
            List<int> hint = m_GameLogic.calculateHint();

            //Update view
            updateCell(hint[0], hint[1], hint[2]);
            view.showHint(hint[0], hint[1], hint[2]);
            view.refreshHintsLeft(maxHints - hintsUsed);

            //When a hint is calculated some inputs by the user might be removed and the Model is already up to date, but the view has to be updated
            refreshRemovedCells();

            //Disable hint button if no hints are left
            if (hintsUsed == maxHints)
                view.disableHint();
        }
    }

    /// <summary>
    /// Refreshes the cells that might have been updated by the function showHint()
    /// </summary>
    private void refreshRemovedCells()
    {
        List<int[]> emptyCells = m_GameLogic.getEmptyCells();

        foreach (int[] cellCoords in emptyCells)
            view.clearCell(cellCoords[0], cellCoords[1]);
    }

    /// <summary>
    /// Check if the board in the Model is full: if it has no empty cells it means that the game is over
    /// </summary>
    public void checkGameOver()
    {
        if(m_GameLogic.GetBoard().IsBoardFull())
        {
            //Stop Timer
            timerStarted = false;

            //Calculate Score
            int score;
            if (timeElapsed < 100000) //min score is 1
                score = 100000 - Mathf.RoundToInt(timeElapsed);
            else
                score = 1;
            view.gameOver(score);

            //Set new Highscore if necessary
            if (score > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", score);
                view.refreshHighscore();
            }
        }
    }

    /// <summary>
    /// Go back to Main Menu
    /// </summary>
    public void quit()
    {
        timerStarted = false;
        timeElapsed = 0;
        view.quit();
    }
}
