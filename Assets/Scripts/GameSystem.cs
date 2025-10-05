
using System;
using System.Linq;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GameSystem : UdonSharpBehaviour
{
    //Synced Variables
    public VRCPlayerApi[] ItPlayers = Array.Empty<VRCPlayerApi>();
    public  VRCPlayerApi[] NormalPlayers = Array.Empty<VRCPlayerApi>();
    public VRCPlayerApi[] PlayersInTrigger = Array.Empty<VRCPlayerApi>();
    [Header("Debug")]
    public TextMeshProUGUI debugtext;
    
    [UdonSynced] public bool isStarted = false;
    [UdonSynced] public String defaultItPlayer = null;
    
    void Update()
    {
        foreach (var a in PlayersInTrigger)
        {
            debugtext.text = a.displayName + "\nLength:" + PlayersInTrigger.Length;
        }
    }

    void StartGame()
    {
        isStarted = true;
        defaultItPlayer = Networking.LocalPlayer.displayName;
    }
}
