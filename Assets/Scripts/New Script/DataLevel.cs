using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataLevel 
{
    public int level;
    public int step;
    public int buy_step;
    public int coins;
    public int max_row;
    public int zone;

    public List<DataRow> rows = new List<DataRow>();

    public DataLevel(int zone, int level, int step, int max_row, int coins, List<DataRow> datarows)
    {
        this.zone = zone;
        this.level = level;
        this.step = step;
        this.max_row = max_row;
        this.coins = coins;
        rows = datarows;
    }

    public DataLevel(int level, List<DataRow> datarows)
    {
        this.level = level;
        rows = datarows;
    }
    public DataLevel()
    {

    }
}
