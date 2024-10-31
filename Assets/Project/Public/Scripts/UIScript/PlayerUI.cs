using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : BaseUI
{

    List<GameObject> _lifesUI = new List<GameObject>();


    [SerializeField] bool _canTag;
    public bool CanTag { get { return _canTag; } set { _canTag = value; OnChangeCanTag.Invoke(); } }
    public event Action OnChangeCanTag;
    [SerializeField] bool _canWallJump;
    public bool CanWallJump { get { return _canWallJump; } set { _canWallJump = value; OnChangeCanWallJump.Invoke(); } }
    public event Action OnChangeCanWallJump;
    [SerializeField] bool _canDoubleJump;
    public bool CanDoubleJump { get { return _canDoubleJump; } set { _canDoubleJump = value; OnChangeCanDoubleJump.Invoke(); } }
    public event Action OnChangeCanDoubleJump;
    [SerializeField] bool _canDash;
    public bool CanDash { get { return _canDash; } set { _canDash = value; OnChangeCanDash.Invoke(); } }
    public event Action OnChangeCanDash;

    protected override void Awake()
    {
        base.Awake();
        InitLifesUI();
    }

    protected void Start()
    {
        SubsCribeEvents();
        InitHpBar();
    }

    #region ĳ���� �ʻ�ȭ UI
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
    #endregion
    public void SetHp()
    {
        int hp = Manager.Game.Player.playerModel.hp;

        for (int i = 0; i < _lifesUI.Count; i++)
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
    }

    #region �±� �ɷ�
    /// <summary>
    /// �±� �ɷ� Ȱ��ȭ UI
    /// </summary>
    public void ShowTagSkill()
    {
        GetUI("TagOff").gameObject.SetActive(false);
        GetUI("TagOn").gameObject.SetActive(true);
    }
    /// <summary>
    /// �±� �ɷ� ��Ȱ��ȭ UI
    /// </summary>
    public void HideTagSkill()
    {
        GetUI("TagOff").gameObject.SetActive(true);
        GetUI("TagOn").gameObject.SetActive(false);
    }
    #endregion
    #region �� ���� �ɷ�
    /// <summary>
    /// �� ���� �ɷ� Ȱ��ȭ UI
    /// </summary>
    public void ShowWallJumpSkill()
    {
        GetUI("WallJumpOff").gameObject.SetActive(false);
        GetUI("WallJumpOn").gameObject.SetActive(true);
    }
    /// <summary>
    /// �� ���� �ɷ� ��Ȱ��ȭ UI
    /// </summary>
    public void HideWallJumpSkill()
    {
        GetUI("WallJumpOff").gameObject.SetActive(true);
        GetUI("WallJumpOn").gameObject.SetActive(false);
    }
    #endregion
    #region ���� ���� �ɷ�
    /// <summary>
    /// ���� ���� �ɷ� Ȱ��ȭ UI
    /// </summary>
    public void ShowDoubleJumpSkill()
    {
        GetUI("DoubleJumpOff").gameObject.SetActive(false);
        GetUI("DoubleJumpOn").gameObject.SetActive(true);
    }
    /// <summary>
    /// ���� ���� �ɷ� ��Ȱ��ȭ UI
    /// </summary>
    public void HideDoubleJumpSkill()
    {
        GetUI("DoubleJumpOff").gameObject.SetActive(true);
        GetUI("DoubleJumpOn").gameObject.SetActive(false);
    }
    #endregion
    #region �뽬 �ɷ�
    /// <summary>
    /// �뽬 �ɷ� Ȱ��ȭ UI
    /// </summary>
    public void ShowDashSkill()
    {
        GetUI("DashOff").gameObject.SetActive(false);
        GetUI("DashOn").gameObject.SetActive(true);
    }
    /// <summary>
    /// �뽬 �ɷ� ��Ȱ��ȭ UI
    /// </summary>
    public void HideDashSkill()
    {
        GetUI("DashOff").gameObject.SetActive(true);
        GetUI("DashOn").gameObject.SetActive(false);
    }
    #endregion


    void InitLifesUI()
    {
        _lifesUI.Add(GetUI("Life1"));
        _lifesUI.Add(GetUI("Life2"));
        _lifesUI.Add(GetUI("Life3"));
    }
    void InitHpBar()
    {
        SetHp();
    }
    
    void SubsCribeEvents()
    {
        Manager.Game.Player.playerModel.OnPlayerDamageTaken += SetHp;
        Manager.Game.Player.playerModel.OnPlayerHealth += SetHp;
        Manager.Game.Player.playerModel.OnPlayerSpawn += SetHp;
    }
}
