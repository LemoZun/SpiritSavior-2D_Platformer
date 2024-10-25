using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    //��������
    private bool _isDead;
    // �׾����� Sprite (������ ��)
    [SerializeField] GameObject deadFace;

    // �����Ӽ�
    private bool _isBlue;
    // �Ķ���
    [SerializeField] GameObject BlueFace;

    // �ҼӼ�
    private bool _isRed;
    // ������
    [SerializeField] GameObject RedFace;



    /* ���߿� �÷��̾�� ���� ?
     * �÷��̾�� �ƹ����� hp?�����ҰŰ����ϱ� / ��������? 
    [SerializeField] private Slider HPBar;
    */
    [Header ("HP Bar")]
    [SerializeField] float curHp;
    [SerializeField] float maxHp;
    [SerializeField] private Slider _hpBar;


    [Header("Live Count")]
    [SerializeField] GameObject[] lives;
    [SerializeField] public int life;
    [SerializeField] public int maxLife;

    /// <summary>
    /// Ÿ�Ժ��� (�� <--> ����)
    /// 'Tab'Ű�� ����
    /// </summary>
    public void ChangeType()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_isBlue == false)
            {
                _isBlue = true;
                _isRed = false;
                Debug.Log($"{_isBlue},    {_isRed}");
            }
            else if(_isBlue == true)
            {
                _isBlue = false;
                _isRed = true;
                Debug.Log($"{_isBlue},    {_isRed}");
            }
        }
    }

    /// <summary>
    /// ���������̽�����
    /// </summary>
    public void FaceToggleSwtich()
    {
        if (_isBlue)
        {
            BlueFace.SetActive(true);
            RedFace.SetActive(false);
        }
        else if (!_isBlue)
        {
            BlueFace.SetActive(false);
            RedFace.SetActive(true);
        }
    }


    /// <summary>
    /// ������ ��UI Ȱ��ȭ
    /// </summary>
    public void DeadFaceOn()
    {
        if (curHp <= 0)
        {
            _isDead = true;
            deadFace.SetActive(true);
        }
        // ���� �ؿ� active�� ���ų� ��ó�� �׳� �پ��ų�
        // deadFace.SetActive(true);
    }

    /// <summary>
    /// ������ �޴��Լ�
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if (life >= 1)
        {
            life -= damage;

            //Destroy(lives[life].gameObject);
            lives[life].SetActive(false);
            if (life < 1)
            {
                _isDead = true;
                // ��Ʈ �� ������ ���� �� Ȱ��ȭ
                deadFace.SetActive(true); 
            }
        }
    }

    /// <summary>
    /// Life �߰� (������ �����ִ� �������� �Դ´ٴ���
    /// </summary>
    public void GiveLife()
    {
        // HP BAR
        // �̰Ŵ� �ǻ��� ������ �� �ؿ� Update���� �÷��̾� ���Ḹ �ϸ��
        #region �׽�Ʈ��HP����
        if (curHp >= 100)
        {
            curHp = 100;
        }
        else
        {
            curHp += 33;

        }
        #endregion

        // LIFE 
        if (life >= maxLife)
        {
            // �ִ�ü�� ���Ѿ��.
            life = maxLife;
            lives[life].SetActive(true);
        }
        else
        {

            lives[life].SetActive(true);
            life += 1;
        }
    }



    void Start()
    {
        maxLife = lives.Length;
        life = lives.Length;
        curHp = maxHp;
    }

    void Update()
    {
        _hpBar.value = curHp / maxHp;
        ChangeType();
        FaceToggleSwtich();

    }





    // Ȯ���ϰ� ����
    #region �׽�Ʈ�� �ʿ��������
   


    /// <summary>
    /// HP���� �Լ�
    /// �ε� ���߿� ���������ſ��� ������ �����ϸ� �ű� �ݸ������� �������޾ƿðŰ��Ƽ� �M�� �ʿ��������
    /// ���� �׳� �׽�Ʈ��
    /// </summary>
    public void GiveDamage()
    {
        curHp -= 33;
        // ü�� �� ������ ������ Ȱ��ȭ
        DeadFaceOn();
    }
    #endregion


}



/* ���� ��� UI��
public void LifeCount()
{
    // ������ �޴°Ÿ��� �׳� ��Ʈ���� �ϸ�
    if (life < 1)
    {
        Destroy(lives[0].gameObject);
    }
    else if (life < 2)
    {
        Destroy(lives[1].gameObject);
    }
    else if (life < 3)
    {
        Destroy(lives[2].gameObject);
    } 

}
*/