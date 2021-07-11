using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class CellController : MonoBehaviour
{
    //There are 4 cell types: WHITE (normal, editable), GREY (locked, written in the json), BLUE (Locked, Hint), RED (invalid, its content is deleted automatically as soon as the user clicks confirm)

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

    /// <summary>
    /// Reset a cell value to 0 and makes it a basic white cell
    /// </summary>
    public void resetCell()
    {
        value = 0;
        lockedValue = false;
        inputField.text = "";
        inputField.GetComponent<Image>().color = Color.white;
        inputField.readOnly = false;
    }

    /// <summary>
    /// If the cell is editable it checks if the selected number is valid: if it is not it 
    /// </summary>
    public void updateCell()
    {
        if (lockedValue)
            return;

        valid = true;
        string newVal = inputField.text;
        if (string.IsNullOrEmpty(newVal))
        {
            resetCell();
            confirmUpdate();
        }
        else if (value != int.Parse(newVal))
        {
            value = int.Parse(inputField.text);

            //The view updates the model only if the cell is valid
            if (!LocalUtilities.getGameManager().validateCell(row, column, value))
                setInvalidCell();
        }
    }

    /// <summary>
    /// If the value inserted by the user is valid the board in the board in the Model is updated, otherwise the cell is cleared
    /// </summary>
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
            LocalUtilities.getGameManager().checkGameOver();
            valid = true;
        }
    }

    /// <summary>
    /// The value of the cell is not editable Anymore
    /// </summary>
    /// <param name="tileValue">The locked Value</param>
    private void lockValue(int tileValue)
    {
        value = tileValue;
        lockedValue = true;
        inputField.text = tileValue.ToString();
        setReadOnly(true);
    }

    /// <summary>
    /// Sets the cell to GREY and locks it
    /// </summary>
    public void setFixedValue(int tileValue)
    {
        lockValue(tileValue);
        inputField.GetComponent<Image>().color = new Color(220f / 255f, 220f / 255f, 220f / 255f); //PASTEL GREY
    }

    /// <summary>
    /// Sets the cell to BLUE and locks it
    /// </summary>
    public void setHintedCell(int tileValue)
    {
        lockValue(tileValue);
        inputField.GetComponent<Image>().color = new Color(174f / 255f, 198f / 255f, 207f / 255f); //PASTEL BLUE
        LocalUtilities.getGameManager().checkGameOver();
    }

    /// <summary>
    /// Sets the cell to RED
    /// </summary>
    private void setInvalidCell()
    {
        inputField.GetComponent<Image>().color = inputField.GetComponent<Image>().color = new Color(253f / 255f, 170f / 255f, 170f / 255f); //PASTEL RED
        valid = false;
    }

    public void setReadOnly(bool readOnly)
    {
        inputField.readOnly = readOnly;
    }
}
