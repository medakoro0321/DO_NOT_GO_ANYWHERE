
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerManager : UdonSharpBehaviour
{
    [Header("プレイヤー管理")]
    public string[] playerNames;      // プレイヤー名を保存
    public int[] playerIDs;          // プレイヤーIDを保存
    public int playerCount = 0;      // 現在のプレイヤー数

    [Header("Debug")] public String DebugInformation;

    /// <summary>
    /// スタートボタンが押されたとき
    /// </summary>
    public void PushStartButton()
    {
        // 全プレイヤーの情報を取得
        VRCPlayerApi[] allPlayers = VRCPlayerApi.GetPlayers(new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()]);

        // 配列のサイズを調整
        playerNames = new string[allPlayers.Length];
        playerIDs = new int[allPlayers.Length];
        playerCount = allPlayers.Length;

        // プレイヤー情報を配列に格納
        for (int i = 0; i < allPlayers.Length; i++)
        {
            if (allPlayers[i] != null && allPlayers[i].IsValid())
            {
                playerNames[i] = allPlayers[i].displayName;
                playerIDs[i] = allPlayers[i].playerId;
            }
        }

        // デバッグ表示
        UpdateDebugDisplay();

        Debug.Log($"プレイヤー情報を取得しました。総数: {playerCount}");
    }

    /// <summary>
    ///  2番目のプレイヤーが配列に含まれているかチェック
    /// </summary>
    /// <returns>2番目のプレイヤーが配列に含まれている</returns>
    public bool CheckSecondPlayerExists()
    {
        if (playerCount >= 2)
        {
            VRCPlayerApi secondPlayer = VRCPlayerApi.GetPlayers(new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()])[1];

            if (secondPlayer != null && secondPlayer.IsValid())
            {
                return IsPlayerInArray(secondPlayer.displayName, secondPlayer.playerId);
            }
        }
        return false;
    }

    /// <summary>
    /// プレイヤーが配列に含まれているかチェック（名前とIDで確認）
    /// </summary>
    /// <param name="playerName">String;プレイヤーの名前</param>
    /// <param name="playerID">int;プレイヤーID</param>
    /// <returns>分からん</returns>
    public bool IsPlayerInArray(string playerName, int playerID)
    {
        for (int i = 0; i < playerCount; i++)
        {
            if (playerNames[i] == playerName && playerIDs[i] == playerID)
            {
                Debug.Log($"プレイヤー {playerName} (ID: {playerID}) が配列に見つかりました");
                return true;
            }
        }
        Debug.Log($"プレイヤー {playerName} (ID: {playerID}) は配列に見つかりませんでした");
        return false;
    }

    // 特定のプレイヤーが配列に含まれているかチェック
    public bool IsPlayerInArray(VRCPlayerApi player)
    {
        if (player == null || !player.IsValid()) return false;

        return IsPlayerInArray(player.displayName, player.playerId);
    }

    // デバッグ表示更新
    void UpdateDebugDisplay()
    {
        string debugInfo = $"登録プレイヤー数: {playerCount}\n";
        for (int i = 0; i < playerCount; i++)
        {
            debugInfo += $"{i + 1}. {playerNames[i]} (ID: {playerIDs[i]})\n";
        }
        DebugInformation = debugInfo;
    }

    // 新しいプレイヤーが入室した際の検知例
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        Debug.Log($"新しいプレイヤーが入室: {player.displayName}");

        // 既に配列に登録済みかチェック
        if (IsPlayerInArray(player))
        {
            Debug.Log("このプレイヤーは既に登録されています");
        }
        else
        {
            Debug.Log("新規プレイヤーです");
        }
    }
}