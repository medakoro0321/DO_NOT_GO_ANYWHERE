using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerTrackingHitbox : UdonSharpBehaviour
{
    [Header("追従設定")]
    public string targetPlayerName = "";  // 追従するプレイヤー名（空なら最初のプレイヤー）
    public Vector3 offset = Vector3.zero;  // プレイヤーからのオフセット
    public bool followRotation = false;    // 回転も追従するか

    [Header("当たり判定設定")]
    public PlayerManager playerManager;

    [Header("エフェクト")]
    public GameObject hitEffect;
    public AudioSource hitSound;

    [Header("デバッグ")]
    public bool debugMode = true;

    private VRCPlayerApi targetPlayer;
    private bool isTracking = false;

    void Start()
    {
        if (playerManager == null)
        {
            Debug.LogError("PlayerManagerがアタッチされてません!");
        }
    }

    void Update()
    {
        if (isTracking && targetPlayer != null && targetPlayer.IsValid())
        {
            // プレイヤーの位置に追従
            Vector3 targetPosition = targetPlayer.GetPosition() + offset;
            transform.position = targetPosition;

            // 回転も追従する場合
            if (followRotation)
            {
                transform.rotation = targetPlayer.GetRotation();
            }
        }
    }

    // 特定のプレイヤーを追従開始
    public void StartTracking(VRCPlayerApi player)
    {
        if (player != null && player.IsValid())
        {
            targetPlayer = player;
            isTracking = true;
            if (debugMode)
                Debug.Log($"プレイヤー {player.displayName} の追従を開始しました");
        }
    }

    // 追従停止
    public void StopTracking()
    {
        isTracking = false;
        targetPlayer = null;
        if (debugMode)
            Debug.Log("追従を停止しました");
    }

    // プレイヤーマネージャーから呼び出される
    public void SetPlayerManager(PlayerManager pm)
    {
        playerManager = pm;
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (debugMode)
            Debug.Log($"プレイヤー {player.displayName} が当たり判定に入りました");

        if (playerManager == null || !playerManager.gameStarted)
        {
            if (debugMode)
                Debug.Log("ゲームが開始されていないため無視");
            return;
        }

        if (!playerManager.IsPlayerInArray(player))
        {
            if (debugMode)
                Debug.Log($"プレイヤー {player.displayName} は登録されていません");
            return;
        }

        if (!playerManager.IsPlayerAlive(player))
        {
            if (debugMode)
                Debug.Log($"プレイヤー {player.displayName} は既に死亡しています");
            return;
        }

        // 当たり判定処理
        ProcessHit(player);
    }

    void ProcessHit(VRCPlayerApi player)
    {
        Debug.Log($"プレイヤー {player.displayName} がヒット！");

        if (hitEffect != null)
        {
            hitEffect.SetActive(false);
            hitEffect.SetActive(true);
        }

        if (hitSound != null)
        {
            hitSound.Play();
        }

        playerManager.PlayerDied(player);

        // 追従停止（必要に応じて）
        // StopTracking();
    }
}