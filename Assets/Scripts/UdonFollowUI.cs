using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class UdonFollowUI : UdonSharpBehaviour {
    [SerializeField] private float followMoveSpeed = 0.1f;
    [SerializeField] private float followRotateSpeed = 0.02f;
    [SerializeField] private bool lockHorizon;
    
    private Quaternion _rot;
    private Quaternion _rotDiff;
    private Vector3 _playerPosition;
    private Quaternion _playerRotation;

    private void Start() {
        //初期位置の設定
        var player = Networking.LocalPlayer;
        if (player == null) return;
        var headData = player.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
        transform.position = headData.position;
        transform.rotation = headData.rotation;
    }

    private void Update() {
        //プレイヤーの位置・回転をリアルタイムに取得
        var player = Networking.LocalPlayer;
        if (player == null) return;
        var headData = player.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
        _playerPosition = headData.position;
        _playerRotation = headData.rotation;
    }

    private void LateUpdate() {
        //UI位置とプレイヤー位置の差分をLerp関数で滑らか補完
        transform.position = Vector3.Lerp(transform.position, _playerPosition, followMoveSpeed);

        //上と同じく回転をLerp関数で滑らかに補完
        //lockHorizonがtrueの場合はxzの角度を0にすることで水平に固定
        _rot = _playerRotation;
        if(lockHorizon) {
            _rot.x = 0;
            _rot.z = 0;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, followRotateSpeed);
    }
}