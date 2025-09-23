
using Mono.Cecil.Cil;
using TMPro;
using UdonSharp;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VRC.SDKBase;
using VRC.Udon;

public class Test : UdonSharpBehaviour
{
    private bool isHeld = false;
    private VRC_Pickup pickup;

    void Start()
    {
        pickup = (VRC_Pickup)GetComponent(typeof(VRC_Pickup));
    }

    void Update()
    {
        //desktop用
        //左クリック判定
        if (Input.GetMouseButtonDown(0) && isHeld)
        {
            Debug.Log("Clicked!");
        }
    }

    ///<summary>
    ///持った時呼ばれる
    /// <summary>
    public override void OnPickup()
    {
        isHeld = true;
        Debug.Log(pickup.currentPlayer);
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