using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustEffect : MonoBehaviour
{
    // �ν����Ϳ��� �߰��� �ּ���.
    public ParticleSystem jumpDust;
    public ParticleSystem dJumpDust;
    public ParticleSystem movingDust;
    /// <summary>
    /// ��������Ʈ 
    /// ���߿� �÷��̾ ���ؼ�
    /// Flip�̳� ���� �׷��� �۵��ǰ� ���� �Լ��� �߰�
    /// ��ǿ� �� ������ ���� ����.
    /// </summary>
    public void JumpDust()
    {
        jumpDust.Play();
    }
    public void DJumpDust()
    {
        dJumpDust.Play();
    }
    public void MovingDust()
    {
        movingDust.Play();
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
