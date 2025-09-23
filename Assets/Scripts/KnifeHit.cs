using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class KnifeHit : UdonSharpBehaviour
{
    [UdonSynced] public int killerPlayerID;
    [UdonSynced] public int killedPlayerID;

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーかどうかを判定
        VRCPlayerApi hitPlayer = VRCPlayerApi.GetPlayerByGameObject(other.gameObject);

        if (hitPlayer != null && hitPlayer != Networking.LocalPlayer)
        {
            // プレイヤーに当たった場合の処理
            killerPlayerID = Networking.LocalPlayer.playerId;
            killedPlayerID = hitPlayer.playerId;

            // ワールドのキルマネージャーに通知
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OnPlayerKilled");
        }
    }
}