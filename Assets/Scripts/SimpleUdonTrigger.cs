using UnityEngine;
using VRC.SDKBase;
using UdonSharp;

/// <summary>
/// VRChat公式スタイルに合わせたTrigger検知システム
/// 公式サンプルの書き方を参考にした確実に動作するバージョン
/// </summary>
[AddComponentMenu("Udon Sharp/Utilities/VRChat Trigger Detector")]
[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
public class SimpleUdonTrigger : UdonSharpBehaviour
{
    [Header("設定")]
    [Tooltip("デバッグログを表示するか")]
    public bool enableDebugLog = true;

    [Tooltip("プレイヤーレイヤー番号")]
    public int playerLayer = 10;

    // プライベート変数
    private VRCPlayerApi localPlayer;
    private bool isInEditor;

    private void Start()
    {
        // 公式サンプルと同じ初期化方法
        localPlayer = Networking.LocalPlayer;
        isInEditor = localPlayer == null; // エディターではnull

        if (enableDebugLog)
        {
            if (isInEditor)
            {
                Debug.Log($"[{gameObject.name}] エディターモードで実行中");
            }
            else
            {
                Debug.Log($"[{gameObject.name}] VRChatで実行中 - プレイヤー: {localPlayer.displayName}");
            }
        }

        // Collider確認
        CheckColliderSetup();
    }

    /// <summary>
    /// Colliderの設定を確認
    /// </summary>
    private void CheckColliderSetup()
    {
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError($"[{gameObject.name}] Colliderが見つかりません！");
            return;
        }

        if (!col.isTrigger)
        {
            Debug.LogWarning($"[{gameObject.name}] ColliderのIs Triggerがオフです！");
        }

        if (enableDebugLog)
        {
            Debug.Log($"[{gameObject.name}] Collider設定完了:");
            Debug.Log($"  - タイプ: {col.GetType().Name}");
            Debug.Log($"  - Is Trigger: {col.isTrigger}");
            Debug.Log($"  - Enabled: {col.enabled}");
        }
    }

    /// <summary>
    /// Trigger領域に入った時
    /// </summary>
    public void OnTriggerEnter(Collider other)
    {
        if (enableDebugLog)
        {
            Debug.Log($"[{gameObject.name}] OnTriggerEnter - {other.name} (Layer: {other.gameObject.layer})");
        }

        // エディターモードでは簡単なログのみ
        if (isInEditor)
        {
            Debug.Log($"[{gameObject.name}] 🔥 エディター: {other.name} が入りました");
            return;
        }

        // VRChatでの処理
        HandleTriggerEnter(other);
    }

    /// <summary>
    /// Trigger領域から出た時
    /// </summary>
    public void OnTriggerExit(Collider other)
    {
        if (enableDebugLog)
        {
            Debug.Log($"[{gameObject.name}] OnTriggerExit - {other.name} (Layer: {other.gameObject.layer})");
        }

        // エディターモードでは簡単なログのみ
        if (isInEditor)
        {
            Debug.Log($"[{gameObject.name}] 🚪 エディター: {other.name} が出ました");
            return;
        }

        // VRChatでの処理
        HandleTriggerExit(other);
    }

    /// <summary>
    /// VRChatでのTrigger Enter処理
    /// </summary>
    private void HandleTriggerEnter(Collider other)
    {
        // プレイヤーレイヤーチェック
        if (other.gameObject.layer == playerLayer)
        {
            Debug.Log($"🎯 [{gameObject.name}] プレイヤー検知！");

            // ローカルプレイヤー情報
            if (localPlayer != null && localPlayer.IsValid())
            {
                Debug.Log($"👤 プレイヤー {localPlayer.displayName} がTriggerに入りました！");

                // ここに具体的な処理を追加
                OnPlayerEnterTrigger();
            }
        }
        else
        {
            Debug.Log($"[{gameObject.name}] 非プレイヤーオブジェクト: {other.name} (Layer: {other.gameObject.layer})");
        }
    }

    /// <summary>
    /// VRChatでのTrigger Exit処理
    /// </summary>
    private void HandleTriggerExit(Collider other)
    {
        // プレイヤーレイヤーチェック
        if (other.gameObject.layer == playerLayer)
        {
            Debug.Log($"🚪 [{gameObject.name}] プレイヤー退出");

            // ローカルプレイヤー情報
            if (localPlayer != null && localPlayer.IsValid())
            {
                Debug.Log($"👋 プレイヤー {localPlayer.displayName} がTriggerから出ました");

                // ここに具体的な処理を追加
                OnPlayerExitTrigger();
            }
        }
    }

    /// <summary>
    /// プレイヤーがTriggerに入った時の具体的な処理
    /// ここをカスタマイズして使用
    /// </summary>
    protected virtual void OnPlayerEnterTrigger()
    {
        Debug.Log($"★★★ プレイヤーがTriggerに入りました！ ★★★");

        // 例: エフェクト再生、色変更、サウンド再生など
        // ChangeObjectColor(Color.red);
        // PlayEffect();
    }

    /// <summary>
    /// プレイヤーがTriggerから出た時の具体的な処理
    /// ここをカスタマイズして使用
    /// </summary>
    protected virtual void OnPlayerExitTrigger()
    {
        Debug.Log($"☆☆☆ プレイヤーがTriggerから出ました ☆☆☆");

        // 例: 元の色に戻す、エフェクト停止など
        // ChangeObjectColor(Color.white);
        // StopEffect();
    }

    /// <summary>
    /// オブジェクトの色を変更（使用例）
    /// </summary>
    private void ChangeObjectColor(Color color)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }
}