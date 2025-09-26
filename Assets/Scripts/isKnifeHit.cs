
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class isKnifeHit : UdonSharpBehaviour
{
    public Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        // 追従ヒットボックスを検知
        if (other.gameObject.layer == 22) // Game Participant Layer
        {
            Debug.LogWarning("Player hit by knife!");

            VRCPlayerApi localPlayer = Networking.LocalPlayer;
            if (localPlayer != null)
            {
                localPlayer.TeleportTo(respawnPoint.position, respawnPoint.rotation);
            }
        }
    }
}
