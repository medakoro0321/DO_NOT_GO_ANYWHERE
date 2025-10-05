using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TagZone : UdonSharpBehaviour
{
    // GameSystemをアタッチ
    public GameSystem gameSystem;

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        // プレイヤーが既に配列に含まれていないか確認
        if (!IsPlayerInArray(player))
        {
            //debug
            player.SetVoiceLowpass(true);
            
            AddPlayerToArray(player);
            Debug.Log($"プレイヤー {player.displayName} がトリガーに入りました。現在の人数: {gameSystem.PlayersInTrigger.Length}");
        }
    }

    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        // StayでもEnterで追加漏れがあった場合の保険として追加
        if (!IsPlayerInArray(player))
        {
            AddPlayerToArray(player);
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        
        //debug
        player.SetVoiceLowpass(false);
        
        // プレイヤーを配列から削除
        RemovePlayerFromArray(player);
        Debug.Log($"プレイヤー {player.displayName} がトリガーから出ました。現在の人数: {gameSystem.PlayersInTrigger.Length}");
    }

    // プレイヤーが配列内に存在するかチェック
    private bool IsPlayerInArray(VRCPlayerApi player)
    {
        if (player == null) return false;

        foreach (var t in gameSystem.PlayersInTrigger)
        {
            if (t != null && t.playerId == player.playerId)
            {
                return true;
            }
        }
        return false;
    }

    // プレイヤーを配列に追加
    private void AddPlayerToArray(VRCPlayerApi player)
    {
        if (player == null) return;

        // 新しい配列を作成（サイズを1増やす）
        VRCPlayerApi[] newArray = new VRCPlayerApi[gameSystem.PlayersInTrigger.Length + 1];

        // 既存のプレイヤーをコピー
        for (int i = 0; i < gameSystem.PlayersInTrigger.Length; i++)
        {
            newArray[i] = gameSystem.PlayersInTrigger[i];
        }

        // 新しいプレイヤーを追加
        newArray[gameSystem.PlayersInTrigger.Length] = player;
        gameSystem.PlayersInTrigger = newArray;
    }

    // プレイヤーを配列から削除
    private void RemovePlayerFromArray(VRCPlayerApi player)
    {
        if (player == null) return;

        int indexToRemove = -1;

        // 削除するプレイヤーのインデックスを探す
        for (int i = 0; i < gameSystem.PlayersInTrigger.Length; i++)
        {
            if (gameSystem.PlayersInTrigger[i] != null && gameSystem.PlayersInTrigger[i].playerId == player.playerId)
            {
                indexToRemove = i;
                break;
            }
        }

        // 見つからなければ何もしない
        if (indexToRemove == -1) return;

        // 新しい配列を作成（サイズを1減らす）
        VRCPlayerApi[] newArray = new VRCPlayerApi[gameSystem.PlayersInTrigger.Length - 1];

        // 削除するプレイヤー以外をコピー
        int newIndex = 0;
        for (int i = 0; i < gameSystem.PlayersInTrigger.Length; i++)
        {
            if (i != indexToRemove)
            {
                newArray[newIndex] = gameSystem.PlayersInTrigger[i];
                newIndex++;
            }
        }

        gameSystem.PlayersInTrigger = newArray;
    }
}