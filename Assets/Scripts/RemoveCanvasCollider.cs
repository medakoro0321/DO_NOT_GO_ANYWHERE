using UdonSharp;
using UnityEngine;

public class RemoveCanvasCollider : UdonSharpBehaviour
{
    private bool colliderRemoved = false;

    void Start()
    {
        RemoveCollider();
    }

    void Update()
    {
        // 何度でも削除を試みる
        if (!colliderRemoved)
        {
            RemoveCollider();
        }
    }

    private void RemoveCollider()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider != null)
        {
            Destroy(collider);
            Debug.Log("[RemoveCanvasCollider] Box Colliderを削除しました");
        }
        else
        {
            colliderRemoved = true;
        }
    }
}