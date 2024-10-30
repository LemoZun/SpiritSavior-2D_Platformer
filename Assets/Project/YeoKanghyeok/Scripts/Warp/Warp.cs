using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Queue<ButtonController> buttons = new Queue<ButtonController>();
    public bool uiActive;
    public void OnUI()
    {
        ButtonController[] arrButtons = new ButtonController[buttons.Count]; // Queue�� �ִ� ��ư ������Ʈ ������

        for (int i = 0; i < buttons.Count; i++)
        {
            if (arrButtons[i].ActiveButton == true) // Ȱ��ȭ�� ��ư ã��
            {
                arrButtons[i].OnActive(); // ������Ʈ Ȱ��ȭ
            }
        }
    }
    public void OffUI()
    {
        ButtonController[] arrButtons = new ButtonController[buttons.Count];

        for(int i = 0;i < buttons.Count; i++)
        {
            arrButtons[i].OffActive();
        }
    }
}
