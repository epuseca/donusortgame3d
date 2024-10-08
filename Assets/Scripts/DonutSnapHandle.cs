using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutSnapHandle : MonoBehaviour
{
    GameManager g_m;
    private int id;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 originalPosition;
    public float snapDistance = 0.2f; // Distance within which the donut snaps to the board
    Color color;
    private Board currentBoard;
    //private float zDistance = 15.0f;

    private void Awake()
    {
        
    }

    void Start()
    {
        g_m = GameManager.Ins;
        UpdateBoardTransforms();
    }
    
    void Update()
    {/*
        if (isDragging)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = g_m.GetCamZ(); // Distance from the camera to the object
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition) + offset;

            transform.localPosition = new Vector3(transform.position.x, transform.position.y, -0.3f);
            //Debug.Log("Dang giu chuot");
        }

        // Check if the mouse button is released
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            g_m.SetStateIsDragging(false);
            //Debug.Log("Khi tha chuot");
            SnapToClosestBoard();
        }*/
    }

    public void ResetDonut()
    {
        isDragging = false;
        g_m.SetStateIsDragging(false);
        //Debug.Log("Khi tha chuot");
        SnapToClosestBoard();
    }

    void OnMouseDown()
    {
        if (!isDragging)
        {
            //
            GameManager.Instance.SetCurrentDonut(gameObject);
            Debug.Log("Bat dau nhan");
            if (!g_m)
                return;
            // Start dragging
            isDragging = true;
            g_m.SetStateIsDragging(true);
            AudioController.Ins.PlaySound(AudioController.Ins.click);

            originalPosition = transform.position; // Store the original position
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = g_m.GetCamZ(); // Distance from the camera to the object
             
            offset = transform.position - Camera.main.ScreenToWorldPoint(mousePosition);

            // Find the current board when first dragg
            Board[] boards = FindObjectsOfType<Board>();
            foreach (Board board in boards)
            {
                if (Vector3.Distance(transform.position, board.transform.position) < snapDistance)
                {
                    currentBoard = board;
                    currentBoard.SetHasDonut(false); // Set false khi ma keo donut di cho khac, bang hien tai bao k co donut
                    break;
                }
            }
        }
    }

    void SnapToClosestBoard() // chon bang gan nhat
    {
        Transform closestBoard = null;
        float closestDistance = Mathf.Infinity;
        Board[] boards = FindObjectsOfType<Board>();
        Board newBoard = null;

        foreach (Board board in boards)
        {
            float distance = Vector3.Distance(transform.position, board.transform.position);
            if (distance < closestDistance && !board.HasDonut())
            {
                closestDistance = distance;
                closestBoard = board.transform;
                newBoard = board;
            }
        }

        if (closestBoard && closestDistance < snapDistance)
        {
            transform.position = closestBoard.position; // Snap to the closest board
            if (newBoard != currentBoard) // kiem tra xem no co cho vao vi tri cu khong
            {
                g_m.IncrementStepCount();
                AudioController.Ins.PlaySound(AudioController.Ins.click);
                //Debug.Log("Step: " + g_m.GetStepCount());
            }
            newBoard.SetHasDonut(true);
            currentBoard = newBoard;
        }
        else
        {
            // Return to the original position if not snapping
            transform.position = originalPosition;
            currentBoard.SetHasDonut(true); // khi ma donut k mark voi bat cu board nao va quay ve vi tri cu thi vi tri board cu se hien thi hasDonut
        }
    }
    public Color GetColor()
    {
        return color;
    }
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

    public void UpdateBoardTransforms()
    {
        
    }
    public int GetIdDonut()
    {
        return id;
    }
    public void SetIdDonut(int id_board)
    {
        id = id_board;
    }
    
}
