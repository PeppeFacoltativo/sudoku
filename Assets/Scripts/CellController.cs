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
    private bool valid;

    private InputField inputField;

    private void Awake() 
    {
        //We could set up row and col specifying them in the inspector, rather than with the gameobject name
        row = int.Parse(name.Substring(0, 1)); //The second letter in the name of the gameobjects indicates the row
        column = int.Parse(name.Substring(1, 1)); //The second letter in the name of the gameobjects indicates the column
        value = 0;
        valid = true;

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

    private void resetCell()
    {
        value = 0;
        inputField.text = "";
        inputField.GetComponent<Image>().color = Color.white;
    }

    public void updateCell()
    {
        if (lockedValue)
            return;

        if (string.IsNullOrEmpty(inputField.text))
            resetCell();
        else
        {
            value = int.Parse(inputField.text);
            //The view updates the model only if the cell is valid
            if (!LocalUtilities.getGameManager().validateCell(row, column, value))
            {
                inputField.GetComponent<Image>().color = inputField.GetComponent<Image>().color = new Color(253f / 255f, 170f / 255f, 170f / 255f);
                valid = false;
                //Notify User
            }
        }
    }

    public void confirmUpdate()
    {
        if (lockedValue)
            return;

        if (!valid)
            resetCell();
        else
        {
            LocalUtilities.getGameManager().updateCell(row, column, value);
            inputField.GetComponent<Image>().color = Color.white;
            checkGameOver();
            valid = true;
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
