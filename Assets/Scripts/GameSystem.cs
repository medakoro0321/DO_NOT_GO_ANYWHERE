
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GameSystem : UdonSharpBehaviour
{
    //Synced Variables
    [UdonSynced] public String[] OniPlayerNames = new String[1];
    [UdonSynced] public bool isStaerted = false;
    void Start()
    {

    }
    void StartGame()
    {

    }
}
