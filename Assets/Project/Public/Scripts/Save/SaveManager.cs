using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath; // ������ ���� ���
    private void Start()
    {
        // ���� ���� ���
        saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        // �ڵ� ���� ����
        InvokeRepeating("SaveGame", 180f, 180f);
    }
    // �������� Ȯ��
    private int GetCurrentStage()
    {
        int currentStage = 0;

        for (int stage = 1; stage <= GameManager.Instance.MaxStage; stage++)
        {
            if (GameManager.Instance.GetIsClearStageDIc(stage))
            {
                currentStage = stage;
            }
        }

        return currentStage;
    }
    // �����Ƽ ��Ʈ
    private int GetUnlockedAbilities()
    {
        int unlockedAbilities = 0;

        

        return unlockedAbilities;
    }
    // ���� ����
    public void SaveGame()
    {
        if (Manager.Game.Player != null)
        {
            GameData gameData = new GameData
            {
                playerHp = Manager.Game.Player.playerModel.hp,                       // ���� hp���� (�÷��̾� ��Ʈ�ѷ����� ���� ���� �߰����� �ʿ�� �˷������)
                playerPosition = Manager.Game.Player.transform.position, // ���� �÷��̾��� ��ġ�� ����
                currentStage = GetCurrentStage(),
                /*unlockedAbilities = Manager.Game.Player.playerModel.*/
                // items = items, // ������ ��� (���� ����)
                // traps = traps // ���� ���� (���� ����)
                // ���⿡ �����Һκ� �߰����ֽø� �˴ϴ�.
            };

            // JSON���� ��ȯ�Ͽ� ���Ͽ� ����
            string json = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("���� �Ϸ�");
        }
    }

    // ���� �ҷ�����
    public GameData LoadGame()
    {
        // ���� ���� ���� Ȯ��
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath); // ���� �б�
            GameData gameData = JsonUtility.FromJson<GameData>(json); // JSON�� ��ü�� ��ȯ
            Debug.Log("�ҷ����⸦ �����߽��ϴ�.");
            return gameData; // ������ ��ȯ
        }
        // ������ ������
        else
        {
            Debug.LogWarning("������ ã�� �� �����ϴ�.");
            return null;
        }
    }
}