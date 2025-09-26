using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GameController : UdonSharpBehaviour
{
    [Header("参照")]
    public PlayerManager playerManager;
    public PlayerTrackingHitbox[] trackingHitboxes;

    [Header("ゲーム設定")]
    public float gameStartDelay = 3f;  // ゲーム開始までの遅延

    void Start()
    {
        if (playerManager == null)
            Debug.LogError("PlayerManagerがアタッチされてません!");
    }

    // ボタンから呼び出される
    public void StartGame()
    {
        Debug.Log("ゲームを開始します...");

        // プレイヤー情報を登録
        playerManager.PushStartButton();

        // 遅延後に当たり判定の追従を開始
        SendCustomEventDelayedSeconds(nameof(StartTracking), gameStartDelay);
    }

    public void StartTracking()
    {
        if (playerManager.registeredPlayers == null || playerManager.registeredPlayers.Length == 0)
        {
            Debug.LogWarning("登録されたプレイヤーがいません");
            return;
        }

        // 各当たり判定に異なるプレイヤーを割り当て
        for (int i = 0; i < trackingHitboxes.Length && i < playerManager.registeredPlayers.Length; i++)
        {
            if (trackingHitboxes[i] != null && playerManager.registeredPlayers[i] != null)
            {
                trackingHitboxes[i].StartTracking(playerManager.registeredPlayers[i]);
            }
        }

        Debug.Log("全ての当たり判定の追従を開始しました");
    }

    // ゲーム終了時の処理
    public void EndGame()
    {
        // 全ての追従を停止
        foreach (PlayerTrackingHitbox hitbox in trackingHitboxes)
        {
            if (hitbox != null)
            {
                hitbox.StopTracking();
            }
        }

        Debug.Log("ゲームを終了しました");
    }
}