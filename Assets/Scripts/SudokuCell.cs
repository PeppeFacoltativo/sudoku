using System;
using Newtonsoft.Json;
public class SudokuCell
{

    [JsonProperty("value")]
    public int Value = 0;
    [JsonProperty("inner_box")]
    public int InnerBox = -1;
    public SudokuCell()
    {
    }
}
