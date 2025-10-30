using UdonSharp;
using UnityEngine;
using TMPro;

namespace DoNotGoAnyWhere
{
    public class TextTyper : UdonSharpBehaviour
    {
        public TextMeshProUGUI textUI;
        public float charDelay = 0.1f;
        
        private string _displayText = "";
        private int _currentIndex = 0;
        private float _timer = 0f;
        private bool _isPlaying = false;
        
        // 表示完了後の待機時間
        private float _displayDuration = 0f;
        private float _displayTimer = 0f;
        private bool _isWaitingToHide = false;
        
        private void Update()
        {
            // テキスト表示中
            if (_isPlaying)
            {
                _timer += Time.deltaTime;
                
                if (_timer >= charDelay)
                {
                    _timer = 0f;
                    
                    if (_currentIndex >= _displayText.Length)
                    {
                        _isPlaying = false;
                        
                        // 表示完了後、待機時間があれば待機モードへ
                        if (_displayDuration > 0f)
                        {
                            _isWaitingToHide = true;
                            _displayTimer = 0f;
                        }
                        return;
                    }
                    
                    _currentIndex++;
                    textUI.text = _displayText.Substring(0, _currentIndex);
                }
            }
            
            // 表示完了後の待機中
            if (_isWaitingToHide)
            {
                _displayTimer += Time.deltaTime;
                
                if (_displayTimer >= _displayDuration)
                {
                    textUI.text = "";
                    _isWaitingToHide = false;
                }
            }
        }
        
        /// <summary>
        /// テキストを一文字ずつ表示
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="hideAfterSeconds">全文表示後、何秒後に消すか</param>
        public void Play(string text, float hideAfterSeconds = 0f)
        {
            _displayText = text;
            _displayDuration = hideAfterSeconds;
            _currentIndex = 0;
            _timer = 0f;
            _displayTimer = 0f;
            _isPlaying = true;
            _isWaitingToHide = false;
            textUI.text = "";
        }
        
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _isPlaying = false;
        }
    }
}