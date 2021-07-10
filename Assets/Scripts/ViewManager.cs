using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    private const int NUM_OF_LINES = 9;

    [SerializeField] private GameObject canvasContainer;

    [Header("Main Menu Canvas")]
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Text highScore;

    [Header("Board Canvas")]
    [SerializeField] private Canvas boardCanvas;
    [SerializeField] private GameObject boardContainer;
    [SerializeField] private Animator quitTextAnimator;
    [SerializeField] private Text timer;

    [Header("Game Over Canvas")]
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Text score;
    [SerializeField] private Text quote;


    private CellController[,] viewCells;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        viewCells = new CellController[NUM_OF_LINES, NUM_OF_LINES];
        for (int i = 0; i < NUM_OF_LINES; i++)
        {
            Transform innerBox = boardContainer.transform.GetChild(i);
            for (int j = 0; j < NUM_OF_LINES; j++)
            {
                CellController cc = innerBox.GetChild(j).GetComponent<CellController>();
                viewCells[cc.getRow(), cc.getColumn()] = cc;
            }
        }

        refreshHighscore();
    }

    public void refreshHighscore()
    {
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    /// <summary>
    /// Calculates the cell coordinates given the inner box and the cell position inside the Inner Box
    /// </summary>
    /// <param name="innerBox">Inner box identifier, value can be from 0-8</param>
    /// <param name="cellNo">cell position inside the inner box, value can be from 0-8</param>
    [Obsolete("Cells are now managed through CellController.cs")]
    private int[] getCellFromInnerBox(int innerBox, int cellNo)
    {
        int column = cellNo % 3 + innerBox % 3 * 3;
        int row = Mathf.Min(innerBox / 3) * 3 + cellNo / 3;
        return new int[2] { row, column };
    }

    public void initializeBoard(SudokuBoard board)
    {
        animator.Play("fadeToBoard");

        for (int i = 0; i < NUM_OF_LINES; i++)
        {
            for (int j = 0; j < NUM_OF_LINES; j++)
            {
                int tileValue = board.GetSudokuTileValue(i, j);
                if (tileValue > 0) //not empty by default
                {
                    viewCells[i, j].setFixedValue(tileValue);
                }
            }
        }
    }

    public void gameOver(int score)
    {
        //boardContainer.GetComponent<Animator>().enabled = true;
        //boardContainer.GetComponent<Animator>().SetTrigger("win");
        this.score.text = "Score: " + score.ToString();
        quote.text = pickRandomQuote();
        canvasContainer.GetComponent<Animator>().SetBool("showResults", true);
    }

    public void quit()
    {
        canvasContainer.GetComponent<Animator>().SetBool("showResults", false);
        animator.Play("fadeToMenu");
    }

    public void displayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void quitButtonMouseEnter(Text text)
    {
        quitTextAnimator.SetBool("quitHover", true);
    }

    public void quitButtonMouseExit(Text text)
    {
        quitTextAnimator.SetBool("quitHover", false);
    }

    public void showHint(int row, int col, int value)
    {
        viewCells[row, col].setHintedCell(value);
    }

    private string pickRandomQuote()
    {
        string[] quotes = File.ReadAllLines(Application.dataPath + "/Files/Quotes.txt");
        string chosenQuote = quotes[UnityEngine.Random.Range(0, quotes.Length)];
        chosenQuote = chosenQuote.Replace("-", "\r\n-");
        return chosenQuote;
    }

    private void clearBoard()
    {
        foreach (CellController c in viewCells)
            c.resetCell();
    }

    private void switchToMainMenuCanva()
    {
        boardCanvas.enabled = false;
        mainMenuCanvas.enabled = true;
        gameOverCanvas.enabled = false;
    }

    private void switchToBoardCanva()
    {
        boardCanvas.enabled = true;
        mainMenuCanvas.enabled = false;
        gameOverCanvas.enabled = true;
    }

    private void switchToGameOverCanva()
    {
        boardCanvas.enabled = false;
        mainMenuCanvas.enabled = false;
        gameOverCanvas.enabled = true;
    }
}
