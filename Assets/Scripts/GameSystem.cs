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
    // 外部スクリプトを読み込み
    private KillerSelect _killerSelect;

    private void Start()
    {
        _killerSelect = Components.GetComponent<KillerSelect>(this.gameObject);
    }
    
    /// <summary>
    /// ゲームが開始した時
    /// </summary>
    public void GameStart()
    {
        _killerSelect.SelectKillers();
    }
}
