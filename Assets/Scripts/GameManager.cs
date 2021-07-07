using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    SudokuBoardGameLogic m_GameLogic;
    [SerializeField]
    ViewManager view;

    void Start()
    {
        m_GameLogic = new SudokuBoardGameLogic();
        m_GameLogic.StartGame();
        view.setUpBoard(m_GameLogic.GetBoard());
    }


    public void updateCell(int row, int column, int tileValue)
    {
        m_GameLogic.GetBoard().SetSudokuValue(row, column, tileValue);
    }

    public bool validateCell(int row, int column, int tileValue)
    {
        return m_GameLogic.GetBoard().TestSudokuValueValidity(row, column, tileValue);
    }
}
