using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : BaseUI
{

    List<GameObject> _lifesUI = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        InitLifesUI();
    }

    protected void Start()
    {
        InitHpBar();
    }

    /// <summary>
    /// ���� ĳ���� UI Ȱ��ȭ
    /// </summary>
    public void ShowCharFace()
    {
        GetUI("CharFace").SetActive(true);
        GetUI("BlueFace").SetActive(false);
        GetUI("DeadFace").SetActive(false);
    }

    /// <summary>
    /// ��� ĳ���� UI Ȱ��ȭ
    /// </summary>
    public void ShowBlueFace()
    {
        GetUI("CharFace").SetActive(false);
        GetUI("BlueFace").SetActive(true);
        GetUI("DeadFace").SetActive(false);
    }

    /// <summary>
    /// ĳ���� ���� �� UI Ȱ��ȭ
    /// </summary>
    public void ShowDeadFace()
    {
        GetUI("CharFace").SetActive(false);
        GetUI("BlueFace").SetActive(false);
        GetUI("DeadFace").SetActive(true);
    }

    public void SetHp(int hp)
    {
        for(int i = 0; i < _lifesUI.Count; i++)
        {
            if (i < hp)
            {
                _lifesUI[i].SetActive(true);
            }
            else
            {
                _lifesUI[i].SetActive(false);
            }
        }
        GetUI<Slider>("HpBar").value = hp;
    }


    void InitLifesUI()
    {
        _lifesUI.Add(GetUI("Life1"));
        _lifesUI.Add(GetUI("Life2"));
        _lifesUI.Add(GetUI("Life3"));
    }
    void InitHpBar()
    {
        GetUI<Slider>("HpBar").maxValue = Manager.Game.Player.playerModel.curMaxHP;
        SetHp(Manager.Game.Player.playerModel.hp);
    }
}
