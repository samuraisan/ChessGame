using System;
using System.Collections;
using System.Collections.Generic;
using ChineseChess;
using UnityEngine;
using Utility.Event;

public class ChineseChessManager : MonoBehaviour
{
    private void Awake()
    {
        InitSingletons();
    }

    private void InitSingletons()
    {
        ChineseChessBoard.GetInstance();
        EventManager.GetInstance();
        OnRegister();
    }

    private void OnRegister()
    {
        
       
    }
}
