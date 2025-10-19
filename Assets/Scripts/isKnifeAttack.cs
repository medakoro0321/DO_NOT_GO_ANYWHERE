using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;
using UnityEngine.UI;
using TMPro;
using IConnectComponent;

public class isKnifeAttack : UdonSharpBehaviour
{
    [Header("GUI")]
    // debug
    public TextMeshProUGUI deadTextGUI;
    [Header("DebugTextGUI")] public TextMeshProUGUI text;
    
    [Header("RespawnPointTransform")] public Transform respawnPoint;
    private GameSystem _gameSystem;
    
    private int _detectedPlayerCollider = 0;
    private KillerSelect _killerSelect;
    

    private void Start()
    {
        _detectedPlayerCollider = 0;
        var playerAPI = Networking.LocalPlayer;
        text.text = "Local Player: " + playerAPI.displayName;
        deadTextGUI.gameObject.SetActive(false);
        _killerSelect = Components.GetComponent<KillerSelect>("TagZone");
        _gameSystem = Components.GetComponent<GameSystem>("GameSystem");
        
        if (_killerSelect == null) Debug.LogError("_KillerSelectがアタッチされていないかNULLが返されました!");

    }

    // 当たり判定 x2
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        _detectedPlayerCollider += 1;
        OnPlayerCollision(player);
    }
    public override void OnPlayerCollisionEnter(VRCPlayerApi player)
    {
        _detectedPlayerCollider += 1;
        OnPlayerCollision(player);
    }
    /// <summary>
    /// プレイヤーが当たったときの処理
    /// </summary>
    private void OnPlayerCollision(VRCPlayerApi player)
    {
        // ゲーム開始前の表示を防止
        if (!_gameSystem.isReady) return;
        
        // プレイヤーが存在しないもしくは殺人鬼側だった場合、処理を停止
        if (player == null || _killerSelect.IsKiller(player)) return;
        
        player.TeleportTo(
            respawnPoint.position,
            player.GetRotation()
        );
        //デッドテキストの表示
        deadTextGUI.text = "You were killed by killer! (or yandere...?)";
        deadTextGUI.gameObject.SetActive(true);
    }
}
