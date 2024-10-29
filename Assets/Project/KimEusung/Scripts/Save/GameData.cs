using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData
{
    public int playerHp;             // �÷��̾��� ������
    public Vector3 playerPosition; // �÷��̾��� ��ġ�� ������ (��Ȳ���°� ��� Vector3, Vector2)
    //public Vector2 playerPosition;
    public List<string> items;       // �÷��̾� ������ ��� (���߿� �����ʿ�)
    public List<bool> traps;         // �� �������� ��� ���� (���߿� ���� �ʿ�)

    /*====================================���� �����Ұ��� ������ �߰�===========================================*/
}
    
