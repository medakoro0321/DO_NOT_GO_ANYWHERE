
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ConnectKnifeHitbox : UdonSharpBehaviour
{
    [Header("設定")]
    public int hitboxLayer = 22;
    public Vector3 offsetPosition = Vector3.zero;
    private VRCPlayerApi attachedPlayer;
    private bool isAttached = false;

    [Header("当たり判定")] public GameObject KnifeHitboxCollider;

    void Start()
    {
        gameObject.layer = hitboxLayer;
        attachedPlayer = Networking.LocalPlayer;
        if (attachedPlayer != null)
        {
            isAttached = true;
            gameObject.SetActive(true);
        }
    }

    void Update()
    {
        // 開始しているか
        // debug
        bool isStarted = true;
        int JoinPlayerCount;
        

        if (isStarted)
        {
            PlayerConnectHitbox();
        }
    }

    void PlayerConnectHitbox()
    {
        if (isAttached && attachedPlayer != null && attachedPlayer.IsValid())
        {
            // プレイヤーの位置に追従
            Vector3 playerPos = attachedPlayer.GetPosition();
            Quaternion playerRot = attachedPlayer.GetRotation();

            transform.position = playerPos + (playerRot * offsetPosition);
            transform.rotation = playerRot;
        }
    }
}
