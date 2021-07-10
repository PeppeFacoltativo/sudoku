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

    private List<int> getRandomEmptyCell()
    {
        List<int> coords = new List<int>();
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
        while (true);
    }
}
