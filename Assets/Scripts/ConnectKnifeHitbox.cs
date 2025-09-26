using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ConnectKnifeHitbox : UdonSharpBehaviour
{
    [Header("当たり判定設定")]
    public string targetPlayerName = "";  // 空なら全プレイヤーが対象

    [Header("参照")]
    public PlayerManager playerManager;

    [Header("エフェクト")]
    public GameObject hitEffect;  // ヒット時のエフェクト
    public AudioSource hitSound;  // ヒット時の音

    [Header("デバッグ")]
    public bool debugMode = true;

    void Start()
    {
        // PlayerManagerを自動で見つける
        if (playerManager == null)
        {
            Debug.LogError("playerManagerがアタッチされていません!");
        }
    }

    public void SetPlayerManager(PlayerManager pm)
    {
        playerManager = pm;
    }

    // プレイヤーが当たり判定に入った時
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (debugMode)
            Debug.Log($"プレイヤー {player.displayName} が当たり判定に入りました");

        // ゲームが開始されていない場合は無視
        if (playerManager == null || !playerManager.gameStarted)
        {
            if (debugMode)
                Debug.Log("ゲームが開始されていないため、当たり判定を無視します");
            return;
        }

        // 登録されたプレイヤーでない場合は無視
        if (!playerManager.IsPlayerInArray(player))
        {
            if (debugMode)
                Debug.Log($"プレイヤー {player.displayName} は登録されていません");
            return;
        }

        // 既に死亡している場合は無視
        if (!playerManager.IsPlayerAlive(player))
        {
            if (debugMode)
                Debug.Log($"プレイヤー {player.displayName} は既に死亡しています");
            return;
        }

        // 特定のプレイヤーのみ対象の場合
        if (!string.IsNullOrEmpty(targetPlayerName) && player.displayName != targetPlayerName)
        {
            if (debugMode)
                Debug.Log($"対象プレイヤーではありません: {player.displayName} != {targetPlayerName}");
            return;
        }

        // 当たり判定処理
        ProcessHit(player);
    }

    void ProcessHit(VRCPlayerApi player)
    {
        Debug.Log($"プレイヤー {player.displayName} がヒット！");
        /*
         *Todo
        // エフェクト再生
        if (hitEffect != null)
        {
            hitEffect.SetActive(false);
            hitEffect.SetActive(true);
        }

        // サウンド再生
        if (hitSound != null)
        {
            hitSound.Play();
        }
        */
        // プレイヤーマネージャーに死亡を通知
        playerManager.PlayerDied(player);

        // この当たり判定を無効化（一度だけヒットさせる場合）
        // gameObject.SetActive(false);
    }
}