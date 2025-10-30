using UdonSharp;
using UnityEngine;

public class GameSystem : UdonSharpBehaviour
{
    public bool isReady = false;
    public bool isAutoStart = true;
    
    // 外部スクリプトを読み込み
    private KillerSelect _killerSelect;
    private TextTyper _textTyper;
    
    private void Start()
    {   
        _killerSelect = GetComponentInChildren<KillerSelect>();
        
        // TextTyperコンポーネントを取得
        _textTyper = GetComponent<TextTyper>();
    }
    
    /// <summary>
    /// ゲーム準備
    /// </summary>
    public void ReadyGame()
    {
        // インスタンスメソッドとして呼び出す
        _textTyper.Play("YOU RUN AWAY NOW!!!!!",2f);
        
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
        _textTyper.Play("YOU RUN AWAY NOW!!!!!",2f);
    }
}