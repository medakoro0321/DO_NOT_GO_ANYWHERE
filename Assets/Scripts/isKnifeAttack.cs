using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;
using UnityEngine.UI;
using TMPro;

public class isKnifeAttack : UdonSharpBehaviour
{
    [Header("GUI")]
    public TextMeshProUGUI DeadTextGUI;
    [Header("DebugTextGUI")] public TextMeshProUGUI text;
    [Header("RespawnPointTransform")] public Transform RespawnPoint;
    private int DetectedPlayerCollider = 0;
    public VRC_Pickup pickup;

    void Start()
    {
        CheckAttachments();
        DetectedPlayerCollider = 0;
        VRCPlayerApi PlayerAPI = Networking.LocalPlayer;
        text.text = "Local Player: " + PlayerAPI.displayName;
        DeadTextGUI.gameObject.SetActive(false);

    }

    void UpdateMethod(String Type, VRCPlayerApi player,VRCPlayerApi Owner)
    {
        // Debug用
        text.text = "Player Entered: " + player.displayName + "\nDetected Counter: " + DetectedPlayerCollider
        + "\nKnifeOwner: " + Owner.displayName;
    }
    // Overridesたち
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        DetectedPlayerCollider += 1;
        OnPlayerCollision(player);
    }
    public override void OnPlayerCollisionEnter(VRCPlayerApi player)
    {
        DetectedPlayerCollider += 1;
        OnPlayerCollision(player);
    }
    /// <summary>
    /// プレイヤーが当たったときの処理
    /// </summary>
    /// <param name="player">VRC Player API</param>
    void OnPlayerCollision(VRCPlayerApi player)
    {
        // ナイフのオーナーを取得
        VRCPlayerApi owner = Networking.GetOwner(gameObject);
        //debug
        UpdateMethod("OnPlayerCollision", player, owner);
        // オーナーが存在しない場合は処理を中断
        if (owner == null)
        {
            ErrorConsoleLog("Owner is null.");
            return;
        }
        if (player != owner)
        {
            //プレイヤーのテレポート
            player.TeleportTo(
                RespawnPoint.position,
                player.GetRotation()
            );
            //デッドテキストの表示
            DeadTextGUI.text = "You were killed by a knife!";
            DeadTextGUI.gameObject.SetActive(true);
        }
    }

    void CheckAttachments()
    {
        if (text == null)
        {
            ErrorConsoleLog("TextMeshProUGUI component is not assigned.");
        }
        else if (RespawnPoint == null)
        {
            ErrorConsoleLog("RespawnPoint Transform is not assigned.");
        }
        else if (DeadTextGUI == null)
        {
            ErrorConsoleLog("DeadTextGUI TextMeshProUGUI component is not assigned.");
        }
        else if (pickup == null)
        {
            ErrorConsoleLog("VRC_Pickup component is not found on this GameObject.");
        }
        return;
    }
    void ErrorConsoleLog(string message)
    {
        Debug.LogError("[isKnifeAttack] " + message + " by Method");
    }
}
