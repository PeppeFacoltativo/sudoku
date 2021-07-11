using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class SudokuBoard
{
    [JsonProperty("board")]
    private SudokuCell[,] m_Board;
    public SudokuBoard()
    {
        m_Board = new SudokuCell[9, 9];
    }

    //Added constructor to generate a cloned table
    public SudokuBoard(SudokuBoard board)
    {
        m_Board = new SudokuCell[9, 9];
        for (int i = 0; i < m_Board.GetLength(0); i++)
        {
            for (int j = 0; j < m_Board.GetLength(1); j++)
            {
                SetSudokuValue(i, j, board.GetSudokuTileValue(i,j));
            }
        }
    }

    /// <summary>
    /// Set Value for a sudoku tile
    /// </summary>
    /// <param name="Row">Row location of the tile, value can be from 0-8</param>
    /// <param name="Coloumn">Coloumn location of the tile, value can be from 0-8</param>
    /// <param name="TileValue">Value of the tile, the values can range from 1-9 </param>
    public void SetSudokuValue(int Row, int Coloumn, int TileValue)
    {
        if (Row < 0 || Row > 8 ||
            Coloumn < 0 || Coloumn > 8 ||
            TileValue < 0 || TileValue > 9)
        {
            throw new System.ArgumentOutOfRangeException("Invalid Sudoku Values");
        }
       

        m_Board[Row, Coloumn] = new SudokuCell
        {
            Value = TileValue,
            InnerBox = GetInnerBoxLocation(Row, Coloumn)
    };


    }

    public void ResetSudokuValue(int Row, int Coloumn)
    {
        if (Row < 0 || Row > 8 ||
            Coloumn < 0 || Coloumn > 8 )
        {
            throw new System.ArgumentOutOfRangeException("Invalid Sudoku Values");
        }


        m_Board[Row, Coloumn] = new SudokuCell
        {
            Value = 0,
            InnerBox = -1
        };


    }

    private int GetInnerBoxLocation(int Row, int Coloumn)
    { 
        int majorRow = Row / 3;  
        int majorCol = Coloumn / 3; 
        return majorCol + majorRow * 3 ;
  
    }
    public bool TestSudokuValueValidity(int row, int Coloumn,int value)
    {

        SudokuCell validatedCell = new SudokuCell { Value=value,
        InnerBox= GetInnerBoxLocation(row,Coloumn)};
        for (int i = 0; i < m_Board.GetLength(0); i++)
        {
            for (int j = 0; j < m_Board.GetLength(1); j++)
            {
                SudokuCell currentCell = m_Board[i, j];
                if (validatedCell.Value == currentCell.Value)
                {
                    if (validatedCell.InnerBox == currentCell.InnerBox)
                    {
                        return false;
                    }
                    if (row == i)
                    {
                        return false;
                    }
                    if (Coloumn == j)
                    {
                        return false;
                    }
                }

            }
        }
        return true;
    }

    public bool IsBoardFull()
    {
        for (int i = 0; i < m_Board.GetLength(0); i++)
        {
            for (int j = 0; j < m_Board.GetLength(1); j++)
            {
                if (m_Board[i,j].Value==0)
                {
                    return false;
                }
            }
        }
        return true;
    }
    /// <summary>
    /// Get Value for a sudoku tile, value of 0 means the tile is empty
    /// </summary>
    /// <param name="Row">Row location of the tile, value can be from 0-8</param>
    /// <param name="Coloumn">Coloumn location of the tile, value can be from 0-8</param>
    ///<returns> Return the value of the tile, 0 </returns>
    public int GetSudokuTileValue(int Row, int Coloumn)
    {
        if (Row < 0 || Row > 8 ||
            Coloumn < 0 || Coloumn > 8)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        return m_Board[Row, Coloumn].Value;
    }

    /// <summary>
    /// Get Value for a sudoku Inner Box, value of -1 means the tile is empty
    /// </summary>
    /// <param name="Row">Row location of the tile, value can be from 0-8</param>
    /// <param name="Coloumn">Coloumn location of the tile, value can be from 0-8</param>
    ///<returns> Return the value of the inner box the tile resides in, 0 </returns>
    public int GetSudokuInnerBoxValue(int Row, int Coloumn)
    {
        if (Row < 0 || Row > 8 ||
            Coloumn < 0 || Coloumn > 8)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        return m_Board[Row, Coloumn].InnerBox;
    }

    public void PrintBoard()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < m_Board.GetLength(0); i++)
        {
            for (int j = 0; j < m_Board.GetLength(1); j++)
            {
                if (j%3==0)
                {
                    sb.Append("|");
                }
                if (m_Board[i,j].Value==0)
                {
                    sb.Append("X");
                }
                else
                {
                    sb.Append(m_Board[i, j].Value);
                }
                
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }


    /// <summary>
    /// Calculates the coordinates of the empty cells in the board
    /// </summary>
    ///<returns>Returns a list of int[2] containing the coordinates (Row, column) of the empty cells</returns>
    public List<int[]> getEmptyCells()
    {
        List<int[]> emptyCellsCoordinates = new List<int[]>();

        for (int i = 0; i < m_Board.GetLength(0); i++)
        {
            for (int j = 0; j < m_Board.GetLength(1); j++)
            {
                if (m_Board[i, j].Value == 0)
                {
                    int[] coords = new int[2];
                    coords[0] = i; //Row
                    coords[1] = j; //Column
                    emptyCellsCoordinates.Add(coords);
                }
            }
        }

        return emptyCellsCoordinates;
    }


    public int getLength()
    {
        return m_Board.GetLength(0); //It is 9, but it's better to make a function for it in case we wanted to make a variant of the normal sudoku
    }
}
