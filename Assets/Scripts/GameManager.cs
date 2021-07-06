using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    SudokuBoardGameLogic m_GameLogic;
    void Start()
    {
        m_GameLogic = new SudokuBoardGameLogic();
        m_GameLogic.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
