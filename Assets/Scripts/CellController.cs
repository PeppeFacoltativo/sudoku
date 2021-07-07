using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class CellController : MonoBehaviour
{
    private int row;
    private int column;
    private int value;

    private bool lockedValue;

    private InputField inputField;

    private void Awake() 
    {
        //We could set up row and col specifying them in the inspector, rather than with the gameobject name
        row = int.Parse(name.Substring(0, 1)); //The second letter in the name of the gameobjects indicates the row
        column = int.Parse(name.Substring(1, 1)); //The second letter in the name of the gameobjects indicates the column
        value = 0;

        lockedValue = false;
        inputField = GetComponent<InputField>();
    }

    public int getRow()
    {
        return row;
    }

    public int getColumn()
    {
        return column;
    }

    public void updateCell()
    {
        if (lockedValue)
            return;

        if (string.IsNullOrEmpty(inputField.text))
        {
            value = 0;
            inputField.GetComponent<Image>().color = Color.white;
        }
        else
        {
            value = int.Parse(inputField.text);

            //The view updates the model only if the cell is valid
            if (LocalUtilities.getGameManager().validateCell(row, column, value))
            {
                LocalUtilities.getGameManager().updateCell(row, column, value);
                inputField.GetComponent<Image>().color = Color.white;

                checkGameOver();
            }
            else
            {
                inputField.GetComponent<Image>().color = inputField.GetComponent<Image>().color = new Color(253f / 255f, 170f / 255f, 170f / 255f);
                //Notify User
            }
        }
    }

    private void checkGameOver()
    {
        if (LocalUtilities.getGameManager().isGameOver())
            Debug.Log("Game Over!"); // win notification
    }

    private void lockValue(int tileValue)
    {
        value = tileValue;
        inputField.text = tileValue.ToString();
        inputField.readOnly = true;
        lockedValue = true;
    }

    public void setFixedValue(int tileValue)
    {
        lockValue(tileValue);
        inputField.GetComponent<Image>().color = new Color(220f / 255f, 220f / 255f, 220f / 255f);
    }

    public void setHintedCell(int tileValue)
    {
        lockValue(tileValue);
        inputField.GetComponent<Image>().color = new Color(174f / 255f, 198f / 255f, 207f / 255f);
        checkGameOver();
    }
}
