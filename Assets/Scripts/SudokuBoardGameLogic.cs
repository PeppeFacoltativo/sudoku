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
        List<int> result = getRandomEmptyCell();
        result.Add(solvedBoard.GetSudokuTileValue(result[0], result[1]));
        return result;
    }

    /// <summary>
    /// Looks for an empty cell in the Board
    /// </summary>
    /// <returns>A list of 2 elements containing in order Row and Column</returns>
    private List<int> getRandomEmptyCell()
    {
        List<int> coords = new List<int>();

        //If the board is full we can't ask for hints
        if (m_Board.IsBoardFull())
            throw new Exception("Board already Full");

        do
        {
            coords.Add(UnityEngine.Random.Range(0, 9));
            coords.Add(UnityEngine.Random.Range(0, 9));
            if (m_Board.GetSudokuTileValue(coords[0], coords[1]) == 0)
                return coords;
            coords.Clear();
        }
        while (true); //Infinite loop is not possible because if the board is not full we are gonna get the empty cell in a reasonable time
    }
}
