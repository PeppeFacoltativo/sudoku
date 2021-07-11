using System;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class SudokuBoardGameLogic
{
    private SudokuBoard m_Board;
    SudokuBoard solvedBoard; // Generate a solved board on startup to avoid lag

    public SudokuBoardGameLogic()
    {
    }

    public void StartGame(string jsonPath)
    {
        m_Board = LoadSudokuBoard(jsonPath);

        //Calculate a solved board at the beginning in a different thread to save time when the user asks for a hint
        solvedBoard = new SudokuBoard(m_Board);
        Thread t = new Thread(() => SolveBoard(solvedBoard));
        t.Start();
    }

    public SudokuBoard GetBoard()
    {
        return m_Board;
    }

    public SudokuBoard LoadSudokuBoard(string jsonPath)
    {
        SudokuBoard board = new SudokuBoard();

        TextAsset file = Resources.Load<TextAsset>(jsonPath);
        board = JsonConvert.DeserializeObject<SudokuBoard>(file.text);

        return board;
    }


    private static bool SolveBoard(SudokuBoard board)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board.GetSudokuTileValue(i,j) == 0)
                {
                    for (int c = 1; c <= 9; c++)
                    {
                        if (board.TestSudokuValueValidity(i,j,c))
                        {
                            board.SetSudokuValue(i, j, c);
                            board.PrintBoard();
                            if (SolveBoard(board))
                                return true;
                            else
                                board.ResetSudokuValue(i, j);
                        }
                    }
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Return row, column and value of the hinted cell
    /// </summary>
    /// <returns>A list of 3 elements containing in order Row, Column and Value</returns>
    public List<int> calculateHint()
    {
        List<int> result = new List<int>();

        int[] coords = getRandomEmptyCell();
        result.Add(coords[0]);
        result.Add(coords[1]);
        result.Add(solvedBoard.GetSudokuTileValue(coords[0], coords[1]));

        /* The hint cell is 100% correct because sudoku ha only 1 solution, but if the user has already put the same
        number in the same row, column or innerbox as the int cell it is necessary to remove it (both in View and in Model) */
        int hintCellInnerBox = m_Board.GetSudokuInnerBoxValue(result[0], result[1]);
        for (int i = 0; i < m_Board.getLength(); i++)
        {
            for (int j = 0; j < m_Board.getLength(); j++)
            {
                if (result[2] == m_Board.GetSudokuTileValue(i, j))
                {
                    if (hintCellInnerBox == m_Board.GetSudokuInnerBoxValue(i, j)) //Same inner box
                    {
                        m_Board.SetSudokuValue(i, j, 0);
                    }
                    if (result[0] == i) //Same Row
                    {
                        m_Board.SetSudokuValue(i, j, 0);
                    }
                    if (result[1] == j) //Same Column
                    {
                        m_Board.SetSudokuValue(i, j, 0);
                    }
                }

            }
        }

        return result;
    }

    /// <summary>
    /// Looks for an empty cell in the Board
    /// </summary>
    /// <returns>Retuns an array of 2 elements containing in order Row and Column of the random empty cell</returns>
    private int[] getRandomEmptyCell()
    {
        List<int> coords = new List<int>();

        //If the board is full we can't ask for hints
        if (m_Board.IsBoardFull())
            throw new Exception("Board already Full");

        List<int[]> emptyCellsCoordinates = getEmptyCells();

        return emptyCellsCoordinates[UnityEngine.Random.Range(0, emptyCellsCoordinates.Count)]; //Choose a random coordinate from the list
    }

    public List<int[]> getEmptyCells()
    {
        return m_Board.getEmptyCells();
    }
}
