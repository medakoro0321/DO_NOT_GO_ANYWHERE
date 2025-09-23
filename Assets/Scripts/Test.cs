
using Mono.Cecil.Cil;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Test : UdonSharpBehaviour
{
    private bool isHeld = false;

    ///<summary>
    ///持った時呼ばれる
    /// <summary>
    public override void OnPickup()
    {
        isHeld = true;
        Debug.Log("持ったよ");
    }

    /// <summary>
    /// 落とした時
    /// <summary>
    public override void OnDrop()
    {
        isHeld = false;
        Debug.Log("落としたよ");
    }

}