using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustEffect : MonoBehaviour
{
    // �ν����Ϳ��� �߰��� �ּ���.
    public ParticleSystem dust;
    /// <summary>
    /// ��������Ʈ 
    /// ���߿� �÷��̾ ���ؼ�
    /// Flip�̳� ���� �׷��� �۵��ǰ� ���� �Լ��� �߰�
    /// ��ǿ� �� ������ ���� ����.
    /// </summary>
    public void CreateDust()
    {
        dust.Play();
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
