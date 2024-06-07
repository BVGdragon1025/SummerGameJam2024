using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay_Menu : MonoBehaviour
{
    public GameObject[] Boards;
    private int Index;

    public GameObject Next_Butt;
    public GameObject Prev_Butt;

    void Awake()
    {
        Index = 0;
        Boards[Index].SetActive(true);
        Prev_Butt.SetActive(false);
    }

    public void NextBoard()
    {
        Boards[Index].SetActive(false);
        Index++;
        Boards[Index].SetActive(true);
        Prev_Butt.SetActive(true);
        if (Index + 1 > Boards.Length - 1)
        {
            Next_Butt.SetActive(false);
        }
    }

    public void PrevBoard()
    {
        Boards[Index].SetActive(false);
        Index--;
        Boards[Index].SetActive(true);
        Next_Butt.SetActive(true);
        if (Index - 1 < 0)
        {
            Prev_Butt.SetActive(false);
        }
    }
}
