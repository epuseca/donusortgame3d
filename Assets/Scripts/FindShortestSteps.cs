using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FindShortestSteps 
{
    static bool Compare(int[] row_count, int[] data, int[] data_id)
    {
        if (data.Length != data_id.Length)
        {
            return false;
        }

        for (int i = row_count[0]; i < data.Length; i++)
        {
            if (data[i] != data_id[i])
            {
                return false; 
            }
        }

        return true; 
    }
    static void Print(int[] numbers)
    {
        foreach (int number in numbers)
        {
            Console.WriteLine(number);
        }
    }

    static int[] AddDataId(int[] data, int[] row_count)
    {
        int[] data_id = new int[data.Length];
        int currentIndex = 0;

        for (int i = 0; i < row_count.Length; i++)
        {
            for (int j = 0; j < row_count[i]; j++)
            {
                if (currentIndex < data.Length)
                {
                    data_id[currentIndex] = i;
                    currentIndex++;
                }
            }
        }

        return data_id;
    }

    static void PrintS(int number)
    {
        Console.WriteLine(number);
    }

    static int FindSpaceDonut(int[] data)
    {
        int id_space = -1;
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == -1)
            {
                id_space = i;
                break;
            }
        }
        return id_space;
    }
    static bool CheckSpaceInData(int[] data, int[] row_count) // true if not has row_count[0]
    { // check if space in row_count[0] and all of bottom is 0 when start game and switch -1 with data[length-1]
        int count = 0;
        for (int i = 0; i < row_count[0]; i++)
        {

            if (data[i] == -1)
            {
                count++;
            }
            if (data[i] == 0)
            {
                count++;
            }
            if (count == row_count[0])
            {
                return false;
            }
        }
        return true;
    }
    static int FindMax(int[] data)
    {
        int max = data[0];
        foreach (int number in data)
        {
            if (number > max)
            {
                max = number;
            }
        }
        return max;
    }

    static int[] CalculateRowCount(int[] data)
    {
        int max = FindMax(data);  
        int[] row_count = new int[max + 1];  

        foreach (int number in data)
        {
            if (number >= 0)  
            {
                row_count[number]++;
            }
        }
        row_count[0]++;
        return row_count;
    }
    public static int CountMinStep(int[] data)
    {
        int[] row_count = CalculateRowCount(data);
        int[] data_id = AddDataId(data, row_count);
        int step = 0;
        int id_space_in_data = FindSpaceDonut(data);
        int id_space_in_data_id = data_id[id_space_in_data];
        
        int find_error = 0;
        while (!Compare(row_count, data, data_id))
        {
            find_error++;
            if (CheckSpaceInData(data, row_count) == false)
            {
                step++;
                for (int i = data.Length - 1; i >= 0; i--)
                {
                    if (data[i] != data_id[i])
                    {
                        int temp = data[i];
                        data[i] = data[id_space_in_data];
                        data[id_space_in_data] = temp;
                        id_space_in_data = i;
                        id_space_in_data_id = data_id[id_space_in_data];
                        break;
                    }
                }

            }
            for (int i = data.Length - 1; i >= 0; i--)
            {
                if (data[i] == id_space_in_data_id && data_id[i] != data[i])
                {
                    step++;
                    int temp = data[i];
                    data[i] = data[id_space_in_data];
                    data[id_space_in_data] = temp;

                    id_space_in_data = i;
                    id_space_in_data_id = data_id[id_space_in_data];

                    break;
                }
            }
            if(find_error == 50)
            {
                Debug.Log("Bugxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                break;
            }
        }
        return step;
    }
}
