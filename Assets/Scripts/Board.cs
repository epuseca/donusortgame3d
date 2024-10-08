using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private int id;
    private int id_donut_in_board;
    private bool hasDonut = false;
    private Color color;

    public void SetColor(int id_color)
    {
        switch (id_color)
        {
            case 0:
                color = Color.cyan;
                break;
            case 1:
                color = Color.yellow;
                break;
            case 2:
                color = Color.red;
                break;
            case 3:
                color = Color.gray;
                break;
            case 4:
                color = Color.green;
                break;
            case 5:
                color = Color.blue;
                break;
            case 6:
                color = Color.black;
                break;
            case 7:
                color = Color.white;
                break;
            case 8:
                color = Color.magenta;
                break;
            default:
                color = Color.clear;
                break;
        }
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    public Color GetColor()
    {
        return color;
    }

    public bool HasDonut()
    {
        return hasDonut;
    }

    public void SetHasDonut(bool value)
    {
        hasDonut = value;
    }

    public int GetIdBoard()
    {
        return id;
    }

    public void SetIdBoard(int boardId)
    {
        id = boardId;
    }
    public int GetIdDonutInBoard()
    {
        return id_donut_in_board;
    }
    public void SetIdDonutInBoard(int id_donut)
    {
        id_donut_in_board = id_donut;
    }
}
