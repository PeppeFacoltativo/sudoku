using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    private const int NUM_OF_LINES = 9;

    [SerializeField]
    private GameObject boardContainer;

    private CellController[,] viewCells;

    private void Awake()
    {
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

    public void setUpBoard(SudokuBoard board)
    {
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
}
