using UdonSharp;
using UnityEngine;

public class RemoveCanvasCollider : UdonSharpBehaviour
{
    private bool _colliderRemoved = false;

    void Start()
    {
        RemoveCollider();
    }

    void Update()
    {
        // 何度でも削除を試みる
        if (!_colliderRemoved)
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
            _colliderRemoved = true;
        }
    }
}