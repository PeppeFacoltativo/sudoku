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

    private InputField inputField;

    private void Awake() 
    {
        //We could set up row and col specifying them in the inspector, rather than with the gameobject name
        row = int.Parse(name.Substring(0, 1)); //The second letter in the name of the gameobjects indicates the row
        column = int.Parse(name.Substring(1, 1)); //The second letter in the name of the gameobjects indicates the column
        value = 0;

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
        if (string.IsNullOrEmpty(inputField.text))
            value = 0;
        else
            value = int.Parse(inputField.text);
        if (LocalUtilities.getGameManager().validateCell(row, column, value))
        {
            LocalUtilities.getGameManager().updateCell(row, column, value);
            inputField.GetComponent<Image>().color = Color.white;
        }
        else
        {
            inputField.GetComponent<Image>().color = Color.red;
            //Notify User
        }
    }

    public void setFixedValue(int tileValue)
    {
        value = tileValue;
        inputField.text = tileValue.ToString();
        inputField.readOnly = true;
        inputField.GetComponent<Image>().color = new Color(220f / 255f, 220f / 255f, 220f / 255f);
    }
}
