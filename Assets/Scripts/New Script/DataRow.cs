using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataRow
{
    public string color;
    public List<int> row1 = new List<int>();
    public DataRow(List<int> row1, string color)
    {

        this.row1 = row1;
        this.color = color;
    }
    public DataRow()
    {
      
    }
}
