using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllDataLevel 
{
    public List<DataLevel> dataLevels = new List<DataLevel>();

    public AllDataLevel()
    {
        // Thêm DataLevel tr?c ti?p v?i các DataRow
        dataLevels.Add(new DataLevel(1, 1, 10, 3, 100, new List<DataRow>
        {
            new DataRow(new List<int> { 1, 2, 3 }, "blue"),
            new DataRow(new List<int> { 4, 5, 6 }, "yellow")
        }));

        dataLevels.Add(new DataLevel(1, 2, 20, 4, 200, new List<DataRow>
        {
            new DataRow(new List<int> { 7, 8, 9 }, "blue"),
            new DataRow(new List<int> { 10, 11, 12 }, "yellow"),
            new DataRow(new List<int> { 10, 11, 12 }, "red")
        }));
    }
}
