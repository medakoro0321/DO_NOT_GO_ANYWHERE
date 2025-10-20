using System;
using System.Linq;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using IConnectComponent;
using CustomDisplayText;

public class GameSystem : UdonSharpBehaviour
{
    public bool isReady = false;
    public bool isAutoStart = true;
    private TextMeshProUGUI _textTyperGUI;
    
    // 外部スクリプトを読み込み
    private KillerSelect _killerSelect;
    
    private void Start()
    {   
        _killerSelect = GetComponentInChildren<KillerSelect>();
        _textTyperGUI = Components.GetComponent<TextMeshProUGUI>("TextTyperGUI");
        CustomDisplayText.Text text = new Text();
    }
    
    /// <summary>
    /// ゲーム準備
    /// </summary>
    public void ReadyGame()
    {
        // TODO : ここ実装
        //debug
        Text.TextTyper(_textTyperGUI,"YOU RUN AWAY NOW!!!!!");
        
        _killerSelect.SelectKillers();
        isReady = true;
        // ゲーム自動開始
        if (isAutoStart) StartGame(); 
        else Debug.Log("Waiting for game to start");
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    public void StartGame()
    {
        Text.TextTyper(_textTyperGUI,"YOU RUN AWAY NOW!!!!!");
    }
}
