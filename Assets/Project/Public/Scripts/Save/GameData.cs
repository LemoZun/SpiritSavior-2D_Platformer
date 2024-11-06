using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData
{
    public int playerHp; // �÷��̾��� HP
    public Vector3 playerPosition; // �÷��̾��� ��ġ
    public int currentStage; // ���� ��������
    public int unlockedAbilities; // �ɷ� �����Ƽ
    public bool[] trapStates; // ���� ���� �迭
}
    
