
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Knife : UdonSharpBehaviour
{
    private Transform _transform;

    private void Update()
    {
        if (!_transform)
        {
            _transform = transform;
        }

        if (_transform.position.y < 0)
        {
            Debug.LogAssertion("Knife is fell down into void.");
            //_transform.position = new Vector3(0, 1, 0);
        }
    }
}
