using System.Collections;
using System.Collections.Generic;
using Project.ParkJunMin.Scripts.States.Switch;
using UnityEngine;

public class ClearSwitchInteractable : SwichInteractable
{
    [SerializeField] GameObject _clearUI;
    [SerializeField] ParticleSystem _particle;

    //BoxCollider2D _boxCollider;

    private void Awake()
    {
        //_boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
       //Manager.Game.OnClear += UpdateClearSwitch;

        _clearUI.SetActive(false);
       // _particle.gameObject.SetActive(false);
       // _boxCollider.enabled = false;

        //UpdateClearSwitch();
    }

    public override void Interact()
    {
        ShowClearUI();

        //if (Manager.Game.IsClearStageDIc.Count >= Manager.Game.MaxStage) 
        //{
        //    if (Manager.Game.IsClear)
        //    {
        //        // Ŭ����
        //        ShowClearUI();
        //    }
        //    else
        //    {
        //        // Ŭ���� ����
        //    }
        //}
    }

    void ShowClearUI()
    {

        Manager.Sound.PlaySFX(Manager.Sound.Data.ClearTreeInteractionSound);

        _clearUI.SetActive(true);
        Time.timeScale = 0;
    }

    void UpdateClearSwitch()
    {
        if (Manager.Game.IsClearStageDIc.Count >= Manager.Game.MaxStage)
        {
           // _particle.gameObject.SetActive(true);
            //_boxCollider.enabled = true;
        }
    }
}
