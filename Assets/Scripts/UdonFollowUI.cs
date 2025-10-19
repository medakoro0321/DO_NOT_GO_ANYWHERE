using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class UdonFollowUI : UdonSharpBehaviour {
    [SerializeField] private float followMoveSpeed = 0.1f;
    [SerializeField] private float followRotateSpeed = 0.02f;
    [SerializeField] private bool lockHorizon;
    
    private Quaternion rot;
    private Quaternion rotDiff;
    private Vector3 playerPosition;
    private Quaternion playerRotation;

    void Start() {
        //初期位置の設定
        var player = Networking.LocalPlayer;
        if (player == null) return;
        var headData = player.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
        transform.position = headData.position;
        transform.rotation = headData.rotation;
    }

    void Update() {
        //プレイヤーの位置・回転をリアルタイムに取得
        var player = Networking.LocalPlayer;
        if (player == null) return;
        var headData = player.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
        playerPosition = headData.position;
        playerRotation = headData.rotation;
    }

    private void LateUpdate() {
        //UI位置とプレイヤー位置の差分をLerp関数で滑らか補完
        transform.position = Vector3.Lerp(transform.position, playerPosition, followMoveSpeed);

        //上と同じく回転をLerp関数で滑らかに補完
        //lockHorizonがtrueの場合はxzの角度を0にすることで水平に固定
        rot = playerRotation;
        if(lockHorizon) {
            rot.x = 0;
            rot.z = 0;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, followRotateSpeed);
    }
}