using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerManager : UdonSharpBehaviour
{
    [Header("プレイヤー管理")]
    public string[] playerNames;
    public int[] playerIDs;
    public VRCPlayerApi[] registeredPlayers;  // 追加：プレイヤー参照を保存
    public int playerCount = 0;

    [Header("ゲーム状態")]
    public bool[] playerAliveStatus;  // 追加：各プレイヤーの生存状態
    public bool gameStarted = false;  // 追加：ゲーム開始フラグ

    [Header("当たり判定管理")]
    public ConnectKnifeHitbox[] knifeHitbox;  // すべての当たり判定オブジェクト

    [Header("デバッグ")]
    public String debugText;

    public void PushStartButton()
    {
        Debug.Log("スタートボタンが押されました");

        // 全プレイヤーの情報を取得
        VRCPlayerApi[] allPlayers = VRCPlayerApi.GetPlayers(new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()]);

        // 配列のサイズを調整
        playerNames = new string[allPlayers.Length];
        playerIDs = new int[allPlayers.Length];
        registeredPlayers = new VRCPlayerApi[allPlayers.Length];
        playerAliveStatus = new bool[allPlayers.Length];
        playerCount = allPlayers.Length;

        // プレイヤー情報を配列に格納
        for (int i = 0; i < allPlayers.Length; i++)
        {
            if (allPlayers[i] != null && allPlayers[i].IsValid())
            {
                playerNames[i] = allPlayers[i].displayName;
                playerIDs[i] = allPlayers[i].playerId;
                registeredPlayers[i] = allPlayers[i];
                playerAliveStatus[i] = true;  // 全員生存状態で開始
            }
        }

        // ゲーム開始
        gameStarted = true;

        // すべての当たり判定を有効化
        EnableAllHitboxes();

        UpdateDebugDisplay();
        Debug.Log($"ゲーム開始！プレイヤー数: {playerCount}");
    }

    // すべての当たり判定オブジェクトを有効化
    void EnableAllHitboxes()
    {
        foreach (ConnectKnifeHitbox hitbox in knifeHitbox)
        {
            if (hitbox != null)
            {
                hitbox.SetPlayerManager(this);
                hitbox.gameObject.SetActive(true);
            }
        }
    }

    // プレイヤーが死亡した時の処理
    public void PlayerDied(VRCPlayerApi player)
    {
        if (!gameStarted) return;

        for (int i = 0; i < playerCount; i++)
        {
            if (registeredPlayers[i] == player)
            {
                playerAliveStatus[i] = false;
                Debug.Log($"プレイヤー {playerNames[i]} が死亡しました");
                UpdateDebugDisplay();
                CheckGameEnd();
                return;
            }
        }
    }

    // プレイヤーが生存しているかチェック
    public bool IsPlayerAlive(VRCPlayerApi player)
    {
        if (!gameStarted) return false;

        for (int i = 0; i < playerCount; i++)
        {
            if (registeredPlayers[i] == player)
            {
                return playerAliveStatus[i];
            }
        }
        return false;
    }

    // ゲーム終了チェック
    void CheckGameEnd()
    {
        int aliveCount = 0;
        for (int i = 0; i < playerCount; i++)
        {
            if (playerAliveStatus[i]) aliveCount++;
        }

        if (aliveCount <= 1)
        {
            Debug.Log("ゲーム終了！");
            gameStarted = false;
            // ゲーム終了処理をここに追加
        }
    }

    void UpdateDebugDisplay()
    {
        if (debugText != null)
        {
            string debugInfo = $"ゲーム状態: {(gameStarted ? "開始中" : "停止中")}\n";
            debugInfo += $"登録プレイヤー数: {playerCount}\n";
            for (int i = 0; i < playerCount; i++)
            {
                string status = playerAliveStatus[i] ? "生存" : "死亡";
                debugInfo += $"{i + 1}. {playerNames[i]} ({status})\n";
            }
            debugText = debugInfo;
        }
    }

    public bool IsPlayerInArray(VRCPlayerApi player)
    {
        if (player == null || !player.IsValid()) return false;

        for (int i = 0; i < playerCount; i++)
        {
            if (registeredPlayers[i] == player)
            {
                return true;
            }
        }
        return false;
    }
}