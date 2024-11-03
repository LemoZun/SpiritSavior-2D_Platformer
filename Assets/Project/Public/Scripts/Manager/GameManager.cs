using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int MaxStage = 4;
    public PlayerController Player;
    public Vector2 RespawnPoint;

    public Dictionary<int, bool> IsClearStageDIc = new Dictionary<int, bool>();
    public event UnityAction<int> OnChangeIsClearStage;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SetPlayer(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>());     
    }

    /// <summary>
    /// 게임매니저에 플레이어 정보 저장
    /// </summary>
    public void SetPlayer(PlayerController player)
    {
        Player = player;
    }

    /// <summary>
    /// 게임매니저에 최근 리스폰 지점 저장
    /// </summary>
    public void SetRespawnPoint(Vector2 respawnPos)
    {
        RespawnPoint = respawnPos;
    }

    
    public bool GetIsClearStageDIc(int key)
    {
        return IsClearStageDIc[key];
    }

    public void SetIsClearStageDIc(int key, bool value)
    {
        if (IsClearStageDIc.ContainsKey(key))
        {
            IsClearStageDIc[key] = value;
            OnChangeIsClearStage(key);
        }
        else
        {
            IsClearStageDIc.Add(key, value);
            OnChangeIsClearStage(key);
        }
    }
}
