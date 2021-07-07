using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class LocalUtilities : MonoBehaviour
{
    private static GameManager gameManager;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    public static GameManager getGameManager()
    {
        return gameManager;
    }
}
