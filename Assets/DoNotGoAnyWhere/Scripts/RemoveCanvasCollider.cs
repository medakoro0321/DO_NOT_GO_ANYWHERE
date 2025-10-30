using UdonSharp;
using UnityEngine;

namespace DoNotGoAnyWhere
{
    public class RemoveCanvasCollider : UdonSharpBehaviour
    {
        private bool _colliderRemoved = false;

        private void Start()
        {
            RemoveCollider();
        }

        private void Update()
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
            if (!collider)
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
}