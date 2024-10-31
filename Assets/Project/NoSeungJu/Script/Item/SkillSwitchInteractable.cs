using UnityEngine;
using UnityEngine.UI;

public class SkillSwitchInteractable : SwichInteractable
{
    public enum Skill { Tag, WallJump, DoubleJump, Dash }

    [SerializeField] SkillTooltipUI _tooltipUI;
    [SerializeField] Skill _skill;

    public override void Interact()
    {
        ShowTooltipUi();
    }

    void ShowTooltipUi()
    {
        transform.SetParent(null);

        SkillTooltipUI tooltipUI = Instantiate(_tooltipUI);
        tooltipUI.GetUI<Button>("CancelButton").onClick.AddListener(ActivePlayerSkill);
    }

    void ActivePlayerSkill()
    {
        switch (_skill)
        {
            case Skill.Tag:
                // Tag Ȱ��ȭ
                break;
            case Skill.WallJump:
                // WallJump Ȱ��ȭ
                break;
            case Skill.DoubleJump:
                // DoubleJump Ȱ��ȭ
                break;
            case Skill.Dash:
                // Dash Ȱ��ȭ
                break;
        }

        Destroy(gameObject);
    }
}
