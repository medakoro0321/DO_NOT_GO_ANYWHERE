
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class isClickedTest : UdonSharpBehaviour
{
    public GameObject myself;

    public override void Interact()
    {
        myself.GetComponent<GameController>().StartGame();
    }
}
