using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO; 
using TMPro;
using System.Text;
public class GameManager : Singleton<GameManager>
{
    public static GameManager Instance;
    //
    public DonutSnapHandle donutPrefab;
    private DonutSnapHandle donut;
    private AllDataLevel currentData;

    public TMP_Text stepCountText;
    public Text LevelText;
    public Board boardPrefab;
    bool isGameStarted;
    bool isGameFinished;
    public int max_row;
    private int count_col = 0;
    private Board board;
    private Board boardOut;
    private List<Vector3> boardPositions = new List<Vector3>();
    private List<Board> boards = new List<Board>();
    private List<DonutSnapHandle> donuts = new List<DonutSnapHandle>();
    private int[] count_row_array;
    public GameObject hitVFX;
    private List<GameObject> hitVFXList = new List<GameObject>();
    int[] rowCount;
    private int coins_bonus = 6;
    private int curStep = 8;
    private int stepCount;
    int[] data_array;
    private int[][] board_id_donut;
    private int[][] board_id_donut_setted;
    private int shortestStep;
    private int zones;
    private int levelBtnGm;
    private int levelAfterLoad;
    private int stepAfterLoad;
    private int coinsAfterLoad;
    public void SetLevelBtnGm(int levelBtn1)
    {
        levelBtnGm = levelBtn1;
    }
    //
    RaycastHit2D hit;
    public GameObject currentHingeJoinCheck;
    public override void Awake()
    {
        Instance = this;
        MakeSingleton(false);
        isGameStarted = false;
        isGameFinished = false;
    }
    

    private void StartRowCount(int rowx)
    {
        max_row = rowx;
        rowCount = new int[max_row];
        for (int i = 0; i < max_row; i++)
            rowCount[i] = Random.Range(max_row, max_row+1);
        count_row_array = new int[max_row]; // Initialize the count_row array
        isGameStarted = true;
        count_col = 0;
        stepCount = 0;
    }

    public override void Start()
    {
        //ImportDataLoaded();
        if (!isGameStarted)
        {
            GameGUIManager.Ins.GameState("GameHome");
            AudioController.Ins.PlayBackgroundMusic();
        }
        if (!PlayerPrefs.HasKey(Const.COIN_KEY))
            Prefs.Coins = 100;
        if (!PlayerPrefs.HasKey(Const.LEVEL_ZONE1))
            Prefs.LevelDone1 = 1;
        if (!PlayerPrefs.HasKey(Const.LEVEL_ZONE2))
            Prefs.LevelDone2 = 1;
        if (!PlayerPrefs.HasKey(Const.LEVEL_ZONE3))
            Prefs.LevelDone3 = 1;
        Debug.Log("Level Zone 1: " + Prefs.LevelDone1.ToString());
        Debug.Log("Level Zone 2: " + Prefs.LevelDone2.ToString());
        Debug.Log("Level Zone 3: " + Prefs.LevelDone3.ToString());

    }
    void Update()
    {
        if (IsGameStarted && !isGameFinished)
        {
            Debug.Log("Level: " + levelBtnGm);
            Debug.Log("Current Step:" + curStep);
            Debug.Log("Coins bonus:" + coins_bonus);
            CheckRowDone1();
            //CheckRowDone();
            CountStep();
            AdjustCameraPosition(max_row);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = GetCamZ(); // Distance from the camera to the object
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            if (currentHingeJoinCheck != null)
            {
                currentHingeJoinCheck.transform.position = new Vector3(transform.position.x, transform.position.y, currentHingeJoinCheck.transform.position.z);
            }
            Debug.Log("mousePosition" + mousePosition);
            Debug.Log("transform.position" + transform.position);

        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentHingeJoinCheck != null)
            {
                currentHingeJoinCheck.GetComponent<DonutSnapHandle>().ResetDonut();
                currentHingeJoinCheck = null;
            }
        }
        GameGUIManager.Ins.UpdateCoins();

    }
    public void ImportDataLoaded()
    {
        // Use Application.dataPath to access the "Assets" folder
        string filePath = Application.dataPath + "/Data/ImportData/FileDataConfig.json";

        currentData = DataHandle.ReadFile(filePath); // Use filePath instead of the old location
        if (currentData != null)
        {
            Debug.Log("Data is loaded");

            foreach (var dataLevel in currentData.dataLevels)
            {
                if (dataLevel.zone == zones && dataLevel.level == levelBtnGm)
                {
                    Debug.Log($"Level: {dataLevel.level}, Zone: {dataLevel.zone}, Coins: {dataLevel.coins}, Steps: {dataLevel.step}");
                    curStep = dataLevel.step;
                    coins_bonus = dataLevel.coins;
                    max_row = dataLevel.max_row;

                    board_id_donut_setted = new int[max_row][];
                    for (int i = 0; i < dataLevel.rows.Count; i++)
                    {
                        board_id_donut_setted[i] = dataLevel.rows[i].row1.ToArray();  // Assign row1 from JSON to the array
                        Debug.Log($"Color: {dataLevel.rows[i].color}, Row {i}: {string.Join(",", board_id_donut_setted[i])}");
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Cannot load data from file at " + filePath);
        }
    }

    public void CheckPlayGame()
    {
        ImportDataLoaded();
        count_row_array = new int[max_row]; // Initialize the count_row array
        isGameStarted = true;
        count_col = 0;
        stepCount = 0;
        InitializeGame1();


        //LevelText.text = "L:" + levelBtnGm.ToString() + " S:" + curStep + " C:" + coins_bonus;
        LevelText.text = "LEVEL " + levelBtnGm.ToString();
        GameGUIManager.Ins.GameState("GameGui");
        DestroyHitVFX();

    }
    void InitializeGame1()
    {
        int board_max = max_row;
        float scaleFactor = 1f;
        // Check if the zone is 1 and set the scale factor to 0.8
        if (zones == 1)
        {
            scaleFactor = 0.8f;
        }
        if (zones == 2)
        {
            scaleFactor = 0.7f;
        }
        if (zones == 3)
        {
            scaleFactor = 0.5f;
        }

        // Create Boards
        for (int i = 0; i < board_max; i++)
        {
            for (int j = 0; j < board_id_donut_setted[i].Length; j++)
            {
                // Create board with specified position and rotation
                board = Instantiate(boardPrefab, new Vector3(j * 1.5f  + 0.75f * (board_max - board_id_donut_setted[i].Length), i * 1.5f, 0) * scaleFactor, Quaternion.Euler(-90, 0, 0));
                board.transform.localScale *= scaleFactor; // Apply scaling to the board
                board.SetIdBoard(i);
                board.SetColor(i);

                boards.Add(board);
                boardPositions.Add(board.transform.position); // Add board to list
                Debug.Log($"Board {i},{j} v? trí: {board.transform.position}");
            }

            if (i == board_max - 1)
            {
                board = Instantiate(boardPrefab, new Vector3(board_max * 1.5f, board_max * 1.5f, 3f), Quaternion.Euler(-90, 0, 0)); // Another board
                board.transform.localScale *= scaleFactor; // Apply scaling to the additional board
                boardOut = board;
            }
        }

        int zz = 0;

        // Create donuts based on board_id_donut_setted
        for (int i = 0; i < board_max; i++)
        {
            for (int j = 0; j < board_id_donut_setted[i].Length; j++)
            {
                int donutId = board_id_donut_setted[i][j];

                // Only create donut if donutId is not -1
                if (donutId != -1)
                {
                    Vector3 boardPosition = boards[zz].transform.position;

                    // Create donut
                    donut = Instantiate(donutPrefab, boardPosition, Quaternion.identity);
                    donut.transform.localScale *= scaleFactor; // Apply scaling to the donut
                    donut.SetColor(donutId); // Set color based on donutId
                    donut.SetIdDonut(donutId); // Set ID for donut

                    donuts.Add(donut);

                    // Assign the donut to the corresponding board
                    Board board = boards[zz];
                    board.SetHasDonut(true);
                    board.SetIdDonutInBoard(donutId); // Set ID donut for board
                }
                zz++;
            }
        }

        // Assign default ID -1 to empty boards (where there's no donut)
        foreach (Board board in boards)
        {
            if (!board.HasDonut())
            {
                board.SetIdDonutInBoard(-1); // When board has no donut, set ID = -1
            }
        }

        board_id_donut = new int[board_max][];
        int length_data_array = 0;
        for (int i = 0; i < board_max; i++)
        {
            board_id_donut[i] = new int[board_id_donut_setted[i].Length];
            int j = 0;
            foreach (Board board in boards)
            {
                if (board.GetIdBoard() == i)
                {
                    board_id_donut[i][j] = board.GetIdDonutInBoard();
                    j++;
                    length_data_array++;
                }
            }
        }

        // Print data to LOG
        for (int i = 0; i < board_max; i++)
        {
            string rowData = "Row " + i + ": ";
            for (int j = 0; j < board_id_donut[i].Length; j++)
            {
                rowData += board_id_donut[i][j] + (j < board_id_donut[i].Length - 1 ? ", " : "");
            }
            Debug.Log(rowData);
        }
        data_array = new int[length_data_array];
    }

    void CheckRowDone1()
    {
        // Reset count_row_array
        for (int i = 0; i < count_row_array.Length; i++)
        {
            count_row_array[i] = 0;
        }
        // Check for matching IDs between boards and donuts
        foreach (Board board in boards)
        {
            if (board.HasDonut())
            {
                foreach (DonutSnapHandle donut in donuts)
                {
                    if (Vector3.Distance(board.transform.position, donut.transform.position) < 0.1f) // Check if the donut is on the board
                    {
                        if (board.GetIdBoard() == donut.GetIdDonut())
                        {
                            count_row_array[board.GetIdBoard()]++;
                        }
                    }
                }
            }
        }

        // Debugging: Print the count_row array
        for (int i = 0; i < count_row_array.Length; i++)
        {
            //Debug.Log("Count for ID " + i + ": " + count_row_array[i]);

        }

        // Check if any row is complete
        for (int i = 0; i < count_row_array.Length; i++)
        {
            if (!isDragging && count_row_array[i] == board_id_donut[i].Length)
            {
                Debug.Log("Row with ID " + i + " is complete! Row done.");
                AudioController.Ins.PlaySound(AudioController.Ins.getScore);

                //Destroy board and donut in row
                for (int j = boards.Count - 1; j >= 0; j--)
                {
                    Board board = boards[j];
                    if (board.GetIdBoard() == i)
                    {
                        if (hitVFX)
                        {
                            GameObject vfxInstance = Instantiate(hitVFX, boards[j].transform.position, Quaternion.identity);
                            hitVFXList.Add(vfxInstance);
                        }

                        Destroy(board.gameObject);
                        boards.RemoveAt(j);
                    }
                }

                for (int j = donuts.Count - 1; j >= 0; j--)
                {
                    DonutSnapHandle donut = donuts[j];
                    if (donut.GetIdDonut() == i)
                    {
                        Destroy(donut.gameObject);
                        donuts.RemoveAt(j);
                    }
                }
                count_col++;
                Debug.Log("count_col: " + count_col);
            }
            if (count_col == max_row - 1)
            {
                Debug.Log("Game Done!!!");
                ShowIsGameFinished();
                if (zones == 1)
                    Prefs.LevelDone1 += 1;
                if (zones == 2)
                    Prefs.LevelDone2++;
                if (zones == 3)
                    Prefs.LevelDone3++;
                Debug.Log("Level Zone 1 after win: " + Prefs.LevelDone1.ToString());
                Debug.Log("Level Zone 2 after win: " + Prefs.LevelDone2.ToString());
                Debug.Log("Level Zone 3 after win: " + Prefs.LevelDone3.ToString());

                AudioController.Ins.PlaySound(AudioController.Ins.wingame);
                break;
            }
        }

        if (isGameStarted == true && isGameFinished == false && (curStep - stepCount) == 0)
        {

            ShowLoseLevel();
            GameGUIManager.Ins.GameState("GameGui");
        }
    }
    public void PlayGame()
    {
        StartRowCount(4);
        InitializeGame();
        GameGUIManager.Ins.GameState("GameGui");
        DestroyHitVFX();
    }
    public void PlayLevel1()
    {
        StartRowCount(3);
        InitializeGame();
        DestroyHitVFX();
        zones = 1;
        LevelText.text = "LEVEL 1";
        coins_bonus = 5;
    }
    public void PlayLevel2()
    {
        StartRowCount(4);
        InitializeGame();
        DestroyHitVFX();
        zones = 2;
        LevelText.text = "LEVEL 2";
        coins_bonus = 10;
    }
    public void PlayLevel3()
    {
        StartRowCount(6);
        InitializeGame();
        DestroyHitVFX();
        zones = 3;
        LevelText.text = "LEVEL 3";
        coins_bonus = 15;
    }
    public void OpenSetting()
    {
        GameGUIManager.Ins.GameDialog("GameSetting", true);
        HideAllObjects();
    }
    public void CloseSetting()
    {
        GameGUIManager.Ins.GameDialog("GameSetting", false);
        ShowAllObjects();
    }
    public void BackHome()
    {
        GameGUIManager.Ins.GameState("GameHome");
        DestroyAllBoards();
        DestroyAllDonut();
        isGameStarted = false;
        count_row_array = null; 
        CloseSetting();
    }
    public void ShowWhiteLevel()
    {
        zones = 1;
        GameGUIManager.Ins.GameDialog("GameLevelTable", true);
    }
    public void ShowAsianLevel()
    {
        zones = 2;
        GameGUIManager.Ins.GameDialog("GameLevelTable", true);
    }
    public void ShowBlackLevel()
    {
        zones = 3;
        GameGUIManager.Ins.GameDialog("GameLevelTable", true);
    }
    public void CloseLevel()
    {
        GameGUIManager.Ins.GameDialog("GameLevelTable", false);
    }
    
    public void NextLevel()
    {
        GameGUIManager.Ins.CoinsBonus(coins_bonus);
        GameGUIManager.Ins.GameState("GameLevel");
        GameGUIManager.Ins.GameDialog("GameWin", false);
        GameGUIManager.Ins.GameDialog("GameLose", false);
        isGameStarted = false;
        isGameFinished = false;
        count_row_array = null;
    }
    public void ReplayLevel()
    {
        GameGUIManager.Ins.GameState("GameLevel");
        GameGUIManager.Ins.GameDialog("GameWin", false);
        GameGUIManager.Ins.GameDialog("GameLose", false);
        DestroyAllBoards();
        DestroyAllDonut();
        isGameStarted = false;
        isGameFinished = false;
        count_row_array = null;
    }
    public void BackHomeInLose()
    {
        GameGUIManager.Ins.GameState("GameHome");
        GameGUIManager.Ins.GameDialog("GameWin", false);
        GameGUIManager.Ins.GameDialog("GameLose", false);
        DestroyAllBoards();
        DestroyAllDonut();
        isGameStarted = false;
        isGameFinished = false;
        count_row_array = null;
    }
    public void GameLevelShow()
    {
        GameGUIManager.Ins.GameState("GameLevel");
    }
    public void RetryLevel()
    {
        DestroyAllBoards();
        DestroyAllDonut();
        count_row_array = null;
        if (zones == 1)
            PlayLevel1();
        if (zones == 2)
            PlayLevel2();
        if (zones == 3)
            PlayLevel3();
    }
    public void RetryLevel1()
    {
        DestroyAllBoards();
        DestroyAllDonut();

        CheckPlayGame();
    }
    public void UsedCoinsToBuyStep(int coins)
    {
        int step = 0;
        if (coins == 5) 
            step = 2;
        if(coins == 7)
            step = 3;
        if (coins == 10)
            step = 5;
        if (coins == 15)
            step = 10;
        if (Prefs.Coins - coins >= 0)
        {
            GameGUIManager.Ins.CoinsUsed(coins);
            AudioController.Ins.PlaySound(AudioController.Ins.wingame);
            curStep += step;

            ShowAllObjects();
            isGameStarted = true;
            isGameFinished = false;
            GameGUIManager.Ins.GameDialog("GameLose", false);
        }
        else if (Prefs.Coins - coins < 0)
        {
            ReplayLevel();
        }
    }

    

    public void SetCurrentDonut(GameObject obj)
    {
        currentHingeJoinCheck = obj;
    }

    private void CountStep()
    {
        if(stepCountText)
            //stepCountText
            stepCountText.text = (curStep - stepCount).ToString();

    }

    void InitializeGame()
    {
        // Create Boards
        for (int i = 0; i < max_row; i++)
        {
            for (int j = 0; j < rowCount[i]; j++)
            {
                // Create board with specified position and rotation
                board = Instantiate(boardPrefab, new Vector3(j * 1.5f + 0.75f * (max_row - rowCount[i]), i * 1.5f, 0), Quaternion.Euler(-90, 0, 0));
                board.SetIdBoard(i);
                board.SetColor(i);

                boards.Add(board);
                boardPositions.Add(board.transform.position); // Add board to list
            }

            if (i == max_row - 1)
            {
                board = Instantiate(boardPrefab, new Vector3(max_row * 1.5f, max_row * 1.5f, 3f), Quaternion.Euler(-90, 0, 0)); // Another board
                boardOut = board;
            }
        }

        // Create donuts randomly and assign them to boards
        List<Vector3> usedPositions = new List<Vector3>();
        for (int i = 0; i < max_row; i++)
        {
            for (int j = 0; j < rowCount[i]; j++)
            {
                if (i == 0 && j == 0)
                {
                    donut = Instantiate(donutPrefab, boardOut.transform.position, Quaternion.identity);
                    donut.SetColor(i);
                    donut.SetIdDonut(i);

                    donuts.Add(donut);
                    continue;
                }

                if (boardPositions.Count > 0)
                {
                    int randomIndex = Random.Range(0, boardPositions.Count);
                    Vector3 randomPosition = boardPositions[randomIndex];
                    usedPositions.Add(randomPosition);
                    boardPositions.RemoveAt(randomIndex); // Remove used position

                    donut = Instantiate(donutPrefab, randomPosition, Quaternion.identity);
                    donut.SetColor(i);
                    donut.SetIdDonut(i);

                    donuts.Add(donut);

                    // Assign the donut to the corresponding board
                    foreach (Board board in boards)
                    {
                        if (board.transform.position == randomPosition)
                        {
                            board.SetHasDonut(true);
                            board.SetIdDonutInBoard(donut.GetIdDonut()); // set id donut for board
                            break;
                        }
                    }
                }
            }
        }
        foreach (Board board in boards)
        {
            if (!board.HasDonut())
            {
                board.SetIdDonutInBoard(-1); // When board has not donut set id = -1
            }
        }
        board_id_donut = new int[max_row][];
        int length_data_array = 0;
        for (int i = 0; i < max_row; i++)
        {
            board_id_donut[i] = new int[rowCount[i]];
            int j = 0;
            foreach (Board board in boards)
            {
                if(board.GetIdBoard() == i)
                {
                    board_id_donut[i][j] = board.GetIdDonutInBoard();
                    j++;
                    length_data_array++;
                }
            }
        }
        // print data to LOG
        //Debug.Log("length_data_array: " + length_data_array);
        for (int i = 0; i < max_row; i++)
        {
            
            string rowData = "Row " + i + ": ";
            for (int j = 0; j < board_id_donut[i].Length; j++)
            {
                rowData += board_id_donut[i][j] + (j < board_id_donut[i].Length - 1 ? ", " : "");
            }
            Debug.Log(rowData);
        }
        data_array = new int[length_data_array];
        string filePath = @"E:\Game\My project\Donut_Sort_3D\Assets\Data\ExportData\board_id_donut_info.txt";

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            int index = 0;
            int buy_step = 0;
            int coins = 7;
            int level = 1;
            int zone = zones;
            int max_row_1 = max_row; 
            string color = "";
            string allData = "{\"buy_step\"" + ":" + buy_step + "," + "\"coins\"" + ":" + coins + "," + "\"level\"" + ":" + level + "," + "\"zone\"" + ":" + zone + "," + "\"max_row\"" + ":" + max_row_1 + "," + "\"rows\":[";

            for (int i = 0; i < max_row; i++)
            {
                if (i == 0) color = "cyan";
                if (i == 1) color = "yellow";
                if (i == 2) color = "red";
                if (i == 3) color = "gray";
                if (i == 4) color = "green";
                if (i == 5) color = "blue";

                //string rowData = "{\"color\":\"\", \"row1\":[";
                string rowData = "{\"color\":\"" + color + "\", \"row1\":[";
                for (int j = 0; j < board_id_donut[i].Length; j++)
                {
                    rowData += board_id_donut[i][j] + (j < board_id_donut[i].Length - 1 ? "," : "]");
                    data_array[index] = board_id_donut[i][j];
                    index++;
                }

                allData += rowData + (i < max_row - 1 ? "}," : "}");
            }
            allData += "]";

            Debug.Log("Board_id_donut information saved to: " + filePath);
            shortestStep = FindShortestSteps.CountMinStep(data_array);
            curStep = shortestStep;
            Debug.Log("Min step: " + shortestStep);

            allData += ", \"step\": \"" + shortestStep + "\"}";
            writer.WriteLine(allData);
        }
    }
    void Print(int[] numbers)
    {
        foreach (int number in numbers)
        {
            Debug.Log(number);
        }
    }

    void AdjustCameraPosition(int maxRow)
    {
        float cameraX = Camera.main.transform.position.x; 
        float cameraY = Camera.main.transform.position.y; 
        float cameraZ = Camera.main.transform.position.z;
        
        //float RotCamY = 20f;
        
        switch (maxRow)
        {
            case 3:
                cameraX = 1.25f;
                cameraY = 1.25f;
                cameraZ = -10f;
                break;
            case 4:
                cameraX = 1.5f;
                cameraY = 1.5f;
                cameraZ = -10f;
                break;
            case 5:
                cameraX = 3f;
                cameraY = 3f;
                cameraZ = -12f;
                break;
            case 6:
                cameraX = 1.85f;
                cameraY = 1.9f;
                cameraZ = -15f;
                break;
            case 7:
                cameraX = 4.5f;
                cameraY = 4.5f;
                cameraZ = -18f;
                break;
            case 8:
                cameraX = 5.25f;
                cameraY = 5.25f;
                cameraZ = -20f;
                break;
            case 9:
                cameraX = 6f;
                cameraY = 6f;
                cameraZ = -21f;
                break;
        }
        SetCamZ(-cameraZ);
        Camera.main.transform.position = new Vector3(cameraX, cameraY, cameraZ);

        //Camera.main.transform.position = new Vector3(-2f, cameraY, cameraZ);
        //Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, RotCamY, Camera.main.transform.rotation.eulerAngles.z);

    }
    void CheckRowDone()
    {
        // Reset count_row_array
        for (int i = 0; i < count_row_array.Length; i++)
        {
            count_row_array[i] = 0;
        }
        // Check for matching IDs between boards and donuts
        foreach (Board board in boards)
        {
            if (board.HasDonut())
            {
                foreach (DonutSnapHandle donut in donuts)
                {
                    if (Vector3.Distance(board.transform.position, donut.transform.position) < 0.1f) // Check if the donut is on the board
                    {
                        if (board.GetIdBoard() == donut.GetIdDonut())
                        {
                            count_row_array[board.GetIdBoard()]++;
                        }
                    }
                }
            }
        }

        // Debugging: Print the count_row array
        for (int i = 0; i < count_row_array.Length; i++)
        {
            //Debug.Log("Count for ID " + i + ": " + count_row_array[i]);

        }

        // Check if any row is complete
        for (int i = 0; i < count_row_array.Length; i++)
        {
            if (!isDragging && count_row_array[i] == rowCount[i])
            {
                Debug.Log("Row with ID " + i + " is complete! Row done.");
                AudioController.Ins.PlaySound(AudioController.Ins.getScore);

                //Destroy board and donut in row
                for (int j = boards.Count - 1; j >= 0; j--)
                {
                    Board board = boards[j];
                    if (board.GetIdBoard() == i)
                    {
                        if (hitVFX)
                        {
                            GameObject vfxInstance = Instantiate(hitVFX, boards[j].transform.position, Quaternion.identity);
                            hitVFXList.Add(vfxInstance);
                        }

                        Destroy(board.gameObject);
                        boards.RemoveAt(j);
                    }
                }

                for (int j = donuts.Count - 1; j >= 0; j--)
                {
                    DonutSnapHandle donut = donuts[j];
                    if (donut.GetIdDonut() == i)
                    {
                        Destroy(donut.gameObject);
                        donuts.RemoveAt(j);
                    }
                }
                count_col++;
                Debug.Log("count_col: " + count_col);
            }
            if(count_col == max_row - 1)
            {
                Debug.Log("Game Done!!!");
                ShowIsGameFinished();
                if(zones == 1)
                    Prefs.LevelDone1 += 1;
                if (zones == 2)
                    Prefs.LevelDone2++;
                if (zones == 3)
                    Prefs.LevelDone3++;
                Debug.Log("Level Zone 1 after win: " + Prefs.LevelDone1.ToString());
                Debug.Log("Level Zone 2 after win: " + Prefs.LevelDone2.ToString());
                Debug.Log("Level Zone 3 after win: " + Prefs.LevelDone3.ToString());

                AudioController.Ins.PlaySound(AudioController.Ins.wingame);
                break;
            }
        }
        
        if (isGameStarted == true && isGameFinished == false && (curStep - stepCount) == 0)
        {
            
            ShowLoseLevel();
            GameGUIManager.Ins.GameState("GameGui");
        }
    }
    void ShowIsGameFinished()
    {
        isGameFinished = true;
        GameGUIManager.Ins.GameDialog("GameWin", true);
        GameGUIManager.Ins.HideGameGui();
        GameGUIManager.Ins.SetTextCoinsBonus(coins_bonus);
        DestroyAllBoards();
        DestroyAllDonut();
    }
    void ShowLoseLevel()
    {
        AudioController.Ins.PlaySound(AudioController.Ins.gameover);
        isGameStarted = false;
        isGameFinished = true;
        GameGUIManager.Ins.GameDialog("GameLose", true);
        GameGUIManager.Ins.CoinsRemainLose();
        HideAllObjects();
    }
    void ContinueGameAfterBuyCoins()
    {
        ShowAllObjects();
        isGameStarted = true;
        isGameFinished = false;
        GameGUIManager.Ins.GameDialog("GameLose", false);

    }
    void ShowIsGameover()
    {
        AudioController.Ins.PlaySound(AudioController.Ins.gameover);
        isGameFinished = true;
        GameGUIManager.Ins.GameDialog("GameLose", true);
        GameGUIManager.Ins.HideGameGui();
        DestroyAllBoards();
        DestroyAllDonut();
    }
    void DestroyAllBoards()
    {
        foreach (Board board in boards)
        {
            if (board)
                Destroy(board.gameObject);
        }
        if(boardOut)
            Destroy(boardOut.gameObject);
        boards = new List<Board>(); // Reset the list of boards to null
        boardPositions = new List<Vector3>();
    }
    void DestroyAllDonut()
    {
        foreach (DonutSnapHandle donut in donuts)
        {
            if (donut)
                Destroy(donut.gameObject);
        }
        donuts = new List<DonutSnapHandle>(); // Reset the list of donuts to null
    }
    void DestroyHitVFX()
    {
        foreach (GameObject vfx in hitVFXList)
        {
            if (vfx)
                Destroy(vfx);
        }
        hitVFXList.Clear(); // Clear the list after destroying all VFX instances
    }

    void HideAllObjects()
    {
        foreach (Board board in boards)
        {
            if (board)
                board.gameObject.SetActive(false);
        }
        if (boardOut)
            boardOut.gameObject.SetActive(false);

        foreach (DonutSnapHandle donut in donuts)
        {
            if (donut)
                donut.gameObject.SetActive(false);
        }
    }

    void ShowAllObjects()
    {
        foreach (Board board in boards)
        {
            if (board)
                board.gameObject.SetActive(true);
        }
        if (boardOut)
            boardOut.gameObject.SetActive(true);

        foreach (DonutSnapHandle donut in donuts)
        {
            if (donut)
                donut.gameObject.SetActive(true);
        }
    }
    public void ShowTutorial()
    {
        GameGUIManager.Ins.GameDialog("GameSetting", false);
        GameGUIManager.Ins.GameDialog("Tutorial",true);
    }
    public void CloseTutorial()
    {
        GameGUIManager.Ins.GameDialog("Tutorial", false);
        if (isGameStarted)
            ShowAllObjects();
    }
    public int GetZones()
    {
        return zones;

    }
    public int GetStepCount()
    {
        return stepCount;

    }
    public int IncrementStepCount()
    {
        return stepCount++;
    }

    public void SetStepCount(int step)
    {
        stepCount = step;
    }
    private bool isDragging = false;
    public void SetStateIsDragging(bool state)
    {
        isDragging = state;
    }

    float camZ;
    public float GetCamZ()
    {
        return camZ;
    }
    public void SetCamZ(float camZZ)
    {
        camZ = camZZ;
    }
    public bool IsGameStarted { get => isGameStarted; }
}
