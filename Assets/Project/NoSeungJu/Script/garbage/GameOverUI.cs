using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : BaseUI
{
    protected void Start()
    {
        GetUI<Button>("RespawnButton").onClick.AddListener(RespawnPlayer); // ������ ��ư�� RespawnPlayer �޼��� ����
        Manager.Game.Player.playerModel.OnPlayerDied += ShowGameOverUI; // �÷��̾ ������ ���ӿ��� UI�� ��Ÿ��

        HideGameOverUI();
    }
    /// <summary>
    /// RespawnPlayer ��ư�� ������ ĳ���Ͱ� ��Ȱ
    /// </summary>
    public void RespawnPlayer()
    {
        Manager.Game.Player.gameObject.SetActive(true);
        HideGameOverUI();
    }

    /// <summary>
    /// ���ӿ��� UI ��Ÿ����
    /// </summary>
    public void ShowGameOverUI()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// ���ӿ��� UI �����
    /// </summary>
    public void HideGameOverUI()
    {
        gameObject.SetActive(false);
    }
}
