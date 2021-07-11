using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    private const int NUM_OF_LINES = 9;
    private const string quotesPath = "Files/Quotes";
    private const string instructionsURL = "https://sudoku.com/how-to-play/sudoku-rules-for-complete-beginners/";

    [SerializeField] private GameObject canvasContainer;

    [Header("Main Menu Canvas")]
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Text highScore;

    [Header("Board Canvas")]
    [SerializeField] private Canvas boardCanvas;
    [SerializeField] private GameObject boardContainer;
    [SerializeField] private Text timer;
    [SerializeField] private Text hintsLeft;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button hintButton;


    [Header("Game Over Canvas")]
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Text score;
    [SerializeField] private Text quote;


    private CellController[,] viewCells;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        //Add all the CellControllers in the respective position of the 2D Array ViewCells
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

        //Show latest High Score
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

    /// <summary>
    /// Builds the board with its locked cells in the BoardCanva
    /// </summary>
    /// <param name="board">The board to show on screen</param>
    public void initializeBoard(SudokuBoard board)
    {
        enableBoardUI();
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

    /// <summary>
    /// Disables the BoardCanvas UI and shows the animation to GameOverCanva
    /// </summary>
    /// <param name="score">The player score</param>
    public void gameOver(int score)
    {
        disableBoardUI();

        this.score.text = "Score: " + score.ToString();
        quote.text = pickRandomQuote();

        //GameOver animation
        canvasContainer.GetComponent<Animator>().SetBool("showResults", true);
    }

    public void quit()
    {
        //Stop Chillin' animation
        animator.Play("fadeToMenu");
    }

    /// <summary>
    /// Converts the time elapsed to a time format
    /// </summary>
    /// <param name="timeElapsed">The time elapsed from the game beginning</param>
    public void displayTime(float timeElapsed)
    {
        float minutes = Mathf.FloorToInt(timeElapsed / 60);
        float seconds = Mathf.FloorToInt(timeElapsed % 60);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void quitButtonMouseEnter(GameObject trigger)
    {
        //Shows "Stop chillin" text
        trigger.GetComponent<Animator>().SetBool("quitHover", true);
    }

    public void quitButtonMouseExit(GameObject trigger)
    {
        //Hides "Stop chillin" text
        trigger.GetComponent<Animator>().SetBool("quitHover", false);
    }

    public void exitButtonMouseEnter(GameObject trigger)
    {
        //Shows "See Ya" text
        trigger.GetComponent<Animator>().SetBool("exitHover", true);
    }

    public void exitButtonMouseExit(GameObject trigger)
    {
        //Hides "See Ya" text
        trigger.GetComponent<Animator>().SetBool("exitHover", false);
    }

    /// <summary>
    /// Shows a (locked) hint cell in the view
    /// </summary>
    /// <param name="row">The row of the hint cell</param>
    /// <param name="col">The column of the hint cell</param>
    /// <param name="value">The value of the hint cell</param>
    public void showHint(int row, int col, int value)
    {
        viewCells[row, col].setHintedCell(value);
    }

    /// <summary>
    /// Shows random quote from the file specified in quotesPath under the score in GameOverCanvas
    /// </summary>
    private string pickRandomQuote()
    {
        TextAsset file = Resources.Load<TextAsset>(quotesPath);
        string[] quotes = file.text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        string chosenQuote = quotes[UnityEngine.Random.Range(0, quotes.Length)];
        chosenQuote = chosenQuote.Replace("-", "\r\n-");
        return chosenQuote;
    }

    /// <summary>
    /// Resets the board view
    /// </summary>
    private void clearBoard()
    {
        foreach (CellController c in viewCells)
            c.resetCell();
    }

    public void clearCell(int row, int column)
    {
        viewCells[row, column].resetCell();
    }

    /// <summary>
    /// hides BoardCanvas and GameOverCanvas and shows Main Menu
    /// </summary>
    private void switchToMainMenuCanva()
    {
        boardCanvas.enabled = false;
        mainMenuCanvas.enabled = true;
        gameOverCanvas.enabled = false;

        //Sets GameOverCanvas back on top
        canvasContainer.GetComponent<Animator>().SetBool("showResults", false);
    }

    /// <summary>
    /// Hides the Main manu and shows the board
    /// </summary>
    private void switchToBoardCanva()
    {
        boardCanvas.enabled = true;
        mainMenuCanvas.enabled = false;
        gameOverCanvas.enabled = true;
    }

    /// <summary>
    /// Disables the buttons in BoardCanvas (Board Cells too)
    /// </summary>
    private void disableBoardUI()
    {
        foreach (CellController c in viewCells)
            c.setReadOnly(true);
        quitButton.interactable = false;
        hintButton.interactable = false;
    }

    /// <summary>
    /// Enables the buttons in BoardCanvas (doesn't affect Board Cells)
    /// </summary>
    private void enableBoardUI()
    {
        quitButton.interactable = true;
        hintButton.interactable = true;
    }

    /// <summary>
    /// Disables the button Hint
    /// </summary>
    public void disableHint()
    {
        hintButton.interactable = false;
    }

    /// <summary>
    /// Updates the amount of hints left
    /// </summary>
    public void refreshHintsLeft(int nHintsLeft)
    {
        hintsLeft.text = nHintsLeft.ToString() + " hints left";
    }

    public void exit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Opens the link for instructions
    /// </summary>
    public void goToInstruction()
    {
        Application.OpenURL(instructionsURL);
    }
}
