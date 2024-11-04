using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int MaxStage = 4;
    public PlayerController Player;
    public Transform RespawnPoint;
    public Vector2 RespawnPos;


    public bool IsClear;
    public event UnityAction OnClear;
    public Dictionary<int, bool> IsClearStageDIc = new Dictionary<int, bool>();
    public event UnityAction<int> OnChangeIsClearStage;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SetPlayer(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>());
        SetRespawnPoint(GameObject.FindGameObjectWithTag("Respawn").transform);
    }

    /// <summary>
    /// ���ӸŴ����� �÷��̾� ���� ����
    /// </summary>
    public void SetPlayer(PlayerController player)
    {
        Player = player;
    }

    /// <summary>
    /// ���ӸŴ����� �ֱ� ������ ���� ����
    /// </summary>
    public void SetRespawnPoint(Transform respawnPoint)
    {
        RespawnPoint = respawnPoint;  
        RespawnPos = respawnPoint.position;
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

        UpdateIsClear();
    }



    private void UpdateIsClear()
    {
        if(IsClearStageDIc.Count >= MaxStage)
        {
            IsClear = true;
            OnClear?.Invoke();
        }
    }
}
