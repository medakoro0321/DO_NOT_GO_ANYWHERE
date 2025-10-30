using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using DoNotGoAnyWhere.Systems;

namespace DoNotGoAnyWhere.Systems
{
    public class GameSystem : UdonSharpBehaviour
    {
        // 同期変数
        [UdonSynced] public int timeRemaining = 0;
        // 変数
        private const int DefaultLimitSec = 60 * 5; // 5分 : 60(sec) * 5(min)
        public bool isReady = false;
        public bool isAutoStart = true;
        private Transform _startSpawnPoint; // スタート時スポーンする場所

        // 外部スクリプトを読み込み
        private KillerSelect _killerSelect;
        private TextTyper _textTyper;

        private void Start()
        {
            _killerSelect = GetComponentInChildren<KillerSelect>();
            // TextTyperコンポーネントを取得
            _textTyper = GetComponent<TextTyper>();
            // 残り時間を初期時間に設定
            timeRemaining = DefaultLimitSec;
        }

        /// <summary>
        /// ゲーム準備
        /// </summary>
        public void ReadyGame()
        {
            _killerSelect.SelectKillers();
            isReady = true;
            // ゲーム自動開始
            if (isAutoStart) StartGame();
            else Debug.Log("Waiting for game to start");
        }

        /// <summary>
        /// ゲーム開始
        /// </summary>
        private void StartGame()
        {
            var allPlayer = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
            VRCPlayerApi.GetPlayers(allPlayer);
            
            _textTyper.Play("YOU RUN AWAY NOW!!!!!", 2f);
            foreach (var player in allPlayer)
            {
                if (!_killerSelect.IsKiller(player)) return;
                player.TeleportTo(
                    _startSpawnPoint.position,
                    player.GetRotation()
                    );
            }
        }
    }
}