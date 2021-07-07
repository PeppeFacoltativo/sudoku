using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    private const int NUM_OF_LINES = 9;

    [SerializeField]
    private GameObject boardContainer;

    private InputField[,] viewCells;

    private void Start()
    {
        viewCells = new InputField[NUM_OF_LINES, NUM_OF_LINES];

        for (int i = 0; i < NUM_OF_LINES; i++)
        {
            for (int j = 0; j < NUM_OF_LINES; j++)
                viewCells[i,j] = boardContainer.transform.GetChild(j + i * NUM_OF_LINES).GetComponent<InputField>();
        }
    }

    public void setUpBoard(SudokuBoard board)
    {
        for (int i = 0; i < NUM_OF_LINES; i++)
        {
            for (int j = 0; j < NUM_OF_LINES; j++)
            {
                int tileValue = board.GetSudokuTileValue(i, j);
                if (tileValue > 0) //not empty by default
                {
                    viewCells[i, j].text = tileValue.ToString();
                    viewCells[i, j].readOnly = true;
                }
            }
        }
    }
}
