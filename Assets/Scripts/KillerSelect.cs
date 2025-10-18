using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class KillerSelect : UdonSharpBehaviour
{
    [Header("設定")]
    [Tooltip("殺人鬼の人数")]
    public int killerCount = 1;
    [Header("デバッグ")]
    public bool showDebugLogs = true;
    
    // トリガー内のプレイヤーリスト
    private readonly List<VRCPlayerApi> _playersInTrigger = new();
    // 選ばれた殺人鬼のリスト
    private VRCPlayerApi[] _selectedKillers;
    // 殺人鬼プレイヤーテレポート場所
    public Transform killerTpTransform;
    
    
    void Start()
    {
        // 選択された殺人鬼を保存する配列を初期化
        _selectedKillers = Array.Empty<VRCPlayerApi>();
        
    }
    
    /// <summary>
    /// 指定トリガーに入った時
    /// </summary>
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player == null) return;
        
        // リストに追加（重複チェック）
        if (_playersInTrigger.Contains(player)) return;
        _playersInTrigger.Add(player);
            
        if (showDebugLogs)
        {
            Debug.Log($"[KillerSelector] プレイヤーが入場: {player.displayName} (合計: {_playersInTrigger.Count}人)");
        }
    }
    
    /// <summary>
    /// トリガーから出た時
    /// </summary>
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player == null) return;
        
        // リストから削除
        if (!_playersInTrigger.Contains(player)) return;
        _playersInTrigger.Remove(player);
            
        if (showDebugLogs)
        {
            Debug.Log($"[KillerSelector] プレイヤーが退出: {player.displayName} (残り: {_playersInTrigger.Count}人)");
        }
    }
    
    /// <summary>
    /// 殺人鬼選択
    /// </summary>
    public void SelectKillers()
    {
        CleanupPlayerList();
        
        var playerCount = _playersInTrigger.Count;
        
        if (playerCount == 0) // トリガー内にプレイヤーが居ない場合
        {
            Debug.LogWarning("[KillerSelector] トリガー内にプレイヤーがいません");
            return;
        }
        
        // 殺人鬼の人数を調整 (killerCount もしくは playerCount どちらか少ない方に合わせる)
        var actualKillerCount = Mathf.Min(killerCount, playerCount);
        
        if (showDebugLogs)
        {
            Debug.Log($"[KillerSelector] {playerCount}人から{actualKillerCount}人の殺人鬼を選択します");
        }
        
        // ランダムに殺人鬼を選択
        _selectedKillers = SelectRandomPlayers(actualKillerCount);
        
        // 結果を表示
        if (showDebugLogs)
        {
            Debug.Log("[KillerSelector] === 殺人鬼選択結果 ===");
            for (int i = 0; i < _selectedKillers.Length; i++)
            {
                if (_selectedKillers[i] != null)
                {
                    Debug.Log($"[KillerSelector] 殺人鬼 {i + 1}: {_selectedKillers[i].displayName}");
                }
            }
        }
        NotifyPlayers();
    }
    
    /// <summary>
    /// ランダムにプレイヤーを選択
    /// </summary>
    /// <param name="count">actualKillerCount(殺人鬼Max人数)</param>
    /// <returns>殺人鬼に選択された人の配列 [Array]</returns>
    private VRCPlayerApi[] SelectRandomPlayers(int count)
    {
        var result = new VRCPlayerApi[count];
        var tempList = new List<VRCPlayerApi>(_playersInTrigger);
        
        for (var i = 0; i < count; i++)
        {
            var randomIndex = Random.Range(0, tempList.Count);
            result[i] = tempList[randomIndex];
            PlayerTeleport(result[i]);
            tempList.RemoveAt(randomIndex);
        }
        return result;
    }
    
    /// <summary>
    /// 無効なプレイヤーをリストから削除
    /// </summary>
    private void CleanupPlayerList()
    {
        for (int i = _playersInTrigger.Count - 1; i >= 0; i--)
        {
            // 退出済みもしくはプレイヤーオブジェクトが存在しない場合リストから削除
            if (_playersInTrigger[i] == null || !_playersInTrigger[i].IsValid()) 
            {
                _playersInTrigger.RemoveAt(i);
            }
        }
    }
    
    /// <summary>
    /// 殺人鬼プレイヤーを別室へテレポート
    /// </summary>
    private void PlayerTeleport(VRCPlayerApi player)
    {
        player.TeleportTo(
            killerTpTransform.position,
            player.GetRotation()
            );
    }
    
    /// <summary>
    /// プレイヤーに通知
    /// </summary>
    private void NotifyPlayers()
    {
        // TODO: 通知を送信
    }
    
    // 選ばれた殺人鬼かどうかをチェック（外部から呼び出し可能）
    public bool IsKiller(VRCPlayerApi player)
    {
        
        foreach (var killer in _selectedKillers)
        {
            if (killer.playerId == player.playerId)
            {
                return true;
            }
        }
        return false;
    }
}