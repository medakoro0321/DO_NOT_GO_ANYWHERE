using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;
using UnityEngine.UI;
using TMPro;


public class TrackingPlayerCollider : UdonSharpBehaviour
{
    public TextMeshProUGUI text;
    private int DetectedPlayerCollider = 0;

    void Start()
    {
        DetectedPlayerCollider = 0;
        VRCPlayerApi PlayerAPI = Networking.LocalPlayer;
        text.text = "Local Player: " + PlayerAPI.displayName;

    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        DetectedPlayerCollider += 1;
        text.text = "Player Entered: " + player.displayName + "\nDetected Counter: " + DetectedPlayerCollider;
    }
    public override void OnPlayerCollisionEnter(VRCPlayerApi player)
    {
        DetectedPlayerCollider += 1;
        text.text = "Player Collided: " + player.displayName + "\nDetected Counter: " + DetectedPlayerCollider;
    }
}
