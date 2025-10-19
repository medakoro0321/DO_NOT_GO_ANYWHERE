using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using VRC.SDKBase;
using Random = UnityEngine.Random;

public class KillerSelect : UdonSharpBehaviour
{
    [Header("設定")]
    [Tooltip("殺人鬼の人数")]
    public int killerCount = 2;
    [Header("デバッグ")]
    public bool showDebugLogs = true;
    public TextMeshProUGUI whoKiller;
    
    // トリガー内のプレイヤーリスト（配列で管理）
    private VRCPlayerApi[] _playersInTrigger = new VRCPlayerApi[0];
    // 選ばれた殺人鬼のリスト
    private VRCPlayerApi[] _selectedKillers;
    // 殺人鬼プレイヤーテレポート場所
    public Transform killerTpTransform;
    
    
    void Start()
    {
        // 選択された殺人鬼を保存する配列を初期化
        _selectedKillers = new VRCPlayerApi[0];
    }
    
    /// <summary>
    /// 指定トリガーに入った時
    /// </summary>
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player == null) return;
        
        // リストに追加（重複チェック）
        if (ContainsPlayer(player)) return;
        AddPlayer(player);
            
        if (showDebugLogs)
        {
            Debug.Log($"[KillerSelector] プレイヤーが入場: {player.displayName} (合計: {_playersInTrigger.Length}人)");
        }
    }
    
    /// <summary>
    /// トリガーから出た時
    /// </summary>
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player == null) return;
        
        // リストから削除
        if (!ContainsPlayer(player)) return;
        RemovePlayer(player);
            
        if (showDebugLogs)
        {
            Debug.Log($"[KillerSelector] プレイヤーが退出: {player.displayName} (残り: {_playersInTrigger.Length}人)");
        }
    }

    /// <summary>
    /// プレイヤーを配列に追加
    /// </summary>
    private void AddPlayer(VRCPlayerApi player)
    {
        VRCPlayerApi[] newArray = new VRCPlayerApi[_playersInTrigger.Length + 1];
        for (int i = 0; i < _playersInTrigger.Length; i++)
        {
            newArray[i] = _playersInTrigger[i];
        }
        newArray[_playersInTrigger.Length] = player;
        _playersInTrigger = newArray;
    }
    
    /// <summary>
    /// プレイヤーを配列から削除
    /// </summary>
    private void RemovePlayer(VRCPlayerApi player)
    {
        int index = -1;
        for (int i = 0; i < _playersInTrigger.Length; i++)
        {
            if (_playersInTrigger[i] != null && _playersInTrigger[i].playerId == player.playerId)
            {
                index = i;
                break;
            }
        }
        
        if (index == -1) return;
        
        VRCPlayerApi[] newArray = new VRCPlayerApi[_playersInTrigger.Length - 1];
        int newIndex = 0;
        for (int i = 0; i < _playersInTrigger.Length; i++)
        {
            if (i != index)
            {
                newArray[newIndex] = _playersInTrigger[i];
                newIndex++;
            }
        }
        _playersInTrigger = newArray;
    }

    /// <summary>
    /// 重複チェック
    /// </summary>
    /// <returns>重複した場合、TRUE</returns>
    private bool ContainsPlayer(VRCPlayerApi player)
    {
        foreach (var t in _playersInTrigger)
        {
            if (t != null && t.playerId == player.playerId)
            {
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    /// 殺人鬼選択
    /// </summary>
    public void SelectKillers()
    {
        CleanupPlayerList();
        
        var playerCount = _playersInTrigger.Length;
        
        if (playerCount == 0) // トリガー内にプレイヤーが居ない場合
        {
            Debug.LogWarning("[KillerSelector] トリガー内にプレイヤーがいません");
            return;
        }
        
        // 殺人鬼の人数を調整 (killerCount もしくは playerCount どちらか少ない方に合わせる)
        var actualKillerCount = Mathf.Min(killerCount, playerCount);
        
        if (showDebugLogs)
        {
            Debug.Log($"[KillerSelector] {playerCount}人から{actualKillerCount}人の殺人鬼を選択します");
        }
        
        // ランダムに殺人鬼を選択
        _selectedKillers = SelectRandomPlayers(actualKillerCount);
        
        // 結果を表示
        if (showDebugLogs)
        {
            Debug.Log("[KillerSelector] === 殺人鬼選択結果 ===");
            for (int i = 0; i < _selectedKillers.Length; i++)
            {
                if (_selectedKillers[i] != null)
                {
                    Debug.Log($"[KillerSelector] 殺人鬼 {i + 1}: {_selectedKillers[i].displayName}");
                }
            }
        }
        NotifyPlayers();
    }
    
    /// <summary>
    /// ランダムにプレイヤーを選択
    /// </summary>
    /// <param name="count">actualKillerCount(殺人鬼Max人数)</param>
    /// <returns>殺人鬼に選択された人の配列 [Array]</returns>
    private VRCPlayerApi[] SelectRandomPlayers(int count)
    {
        var result = new VRCPlayerApi[count];
        var tempArray = new VRCPlayerApi[_playersInTrigger.Length];
        
        // 配列をコピー
        for (int i = 0; i < _playersInTrigger.Length; i++)
        {
            tempArray[i] = _playersInTrigger[i];
        }
        
        int remainingCount = tempArray.Length;
        
        for (var i = 0; i < count; i++)
        {
            var randomIndex = Random.Range(0, remainingCount);
            result[i] = tempArray[randomIndex];
            PlayerTeleport(result[i]);
            
            // 選ばれたプレイヤーを配列の最後と入れ替えて、有効な範囲を減らす
            tempArray[randomIndex] = tempArray[remainingCount - 1];
            remainingCount--;
        }
        return result;
    }
    
    /// <summary>
    /// 無効なプレイヤーをリストから削除
    /// </summary>
    private void CleanupPlayerList()
    {
        int validCount = 0;
        
        // 有効なプレイヤーをカウント
        foreach (var t in _playersInTrigger)
        {
            // 退出済みもしくはプレイヤーオブジェクトが存在しない場合はスキップ
            if (t != null && t.IsValid()) 
            {
                validCount++;
            }
        }
        
        // 有効なプレイヤーだけを新しい配列に詰める
        VRCPlayerApi[] newArray = new VRCPlayerApi[validCount];
        int newIndex = 0;
        
        foreach (var t in _playersInTrigger)
        {
            if (t != null && t.IsValid())
            {
                newArray[newIndex] = t;
                newIndex++;
            }
        }
        
        _playersInTrigger = newArray;
    }
    
    /// <summary>
    /// 殺人鬼プレイヤーを別室へテレポート
    /// </summary>
    private void PlayerTeleport(VRCPlayerApi player)
    {
        player.TeleportTo(
            killerTpTransform.position,
            player.GetRotation()
        );
    }
    
    /// <summary>
    /// プレイヤーに通知
    /// </summary>
    private void NotifyPlayers()
    {
        var localPlayer = Networking.LocalPlayer;
        // TODO: 通知を送信
        whoKiller.text = IsKiller(localPlayer).ToString();
    }
    
    /// <summary>
    ///  選ばれた殺人鬼かどうかをチェック
    /// </summary>
    /// <returns>殺人鬼の場合、TRUE</returns>
    public bool IsKiller(VRCPlayerApi player)
    {
        if (player == null) return false;
        
        foreach (var t in _selectedKillers)
        {
            if (t != null && t.playerId == player.playerId)
            {
                return true;
            }
        }
        return false;
    }
}