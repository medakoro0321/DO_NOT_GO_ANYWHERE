using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class SimpleUdonTrigger : UdonSharpBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[{gameObject.name}] 🔥 OnTriggerEnter発火！");
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log($"[{gameObject.name}] 🚪 OnTriggerExit発火！");
    }
}