using System;
using System.Linq;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using IConnectComponent;

public class GameSystem : UdonSharpBehaviour
{
    public bool isReady = false;
    public bool isAutoStart = true;   
    
    // 外部スクリプトを読み込み
    private KillerSelect _killerSelect;
    
    private void Start()
    {   
        _killerSelect = GetComponentInChildren<KillerSelect>();
    }
    
    /// <summary>
    /// ゲーム準備
    /// </summary>
    public void ReadyGame()
    {
        // init
        _killerSelect.SelectKillers();
        isReady = true;
        if (isAutoStart) StartGame(); 
        else Debug.Log("Waiting for game to start");
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    public void StartGame()
    {
        // 
    }
}
