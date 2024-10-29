using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath; // ������ ���� ���

    void Start()
    {
        // ���� ���� ���
        saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
    }

    // ���� ����
    public void SaveGame(int playerLives, Vector3 playerPosition /*, List<string> items, List<bool> traps */)
    {
        GameData gameData = new GameData
        {
            playerHp = playerLives,
            playerPosition = playerPosition,
            // items = items, // ������ ��� (���� ����)
            // traps = traps // ���� ���� (���� ����)
        };

        // JSON���� ��ȯ�Ͽ� ���Ͽ� ����
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("���� �Ϸ�"); // ���� �Ϸ� �α�
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