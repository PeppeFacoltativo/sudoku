using System;
using UnityEngine;
using Newtonsoft.Json;
public class SudokuBoardGameLogic
{
    private SudokuBoard m_Board;
    public SudokuBoardGameLogic()
    {
    }

    public void StartGame()
    {
      m_Board = LoadSudokuBoard();
    }

    public SudokuBoard GetBoard()
    {
        return m_Board;
    }

    public SudokuBoard LoadSudokuBoard()
    {
        throw new NotImplementedException();
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
}
