
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class StartButton : UdonSharpBehaviour
{
    private GameSystem _gameSystem;

    private void Start()
    {
        _gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
    }

    public override void Interact()
    {
        Debug.Log("<color=red>Interacted</color>");
        _gameSystem.ReadyGame();
    }
}
