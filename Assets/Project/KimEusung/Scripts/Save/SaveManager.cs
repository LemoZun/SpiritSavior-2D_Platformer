using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath; // ������ ���� ���
    public PlayerController playerController;
    private void Start()
    {
        // ���� ���� ���
        saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        // �ڵ� ���� ����
        InvokeRepeating("SaveGame", 180f, 180f);
    }
    // ���� ����
    public void SaveGame()
    {
        if (playerController != null)
        {
            GameData gameData = new GameData
            {
                playerHp = playerController.hp,                       // ���� hp���� (�÷��̾� ��Ʈ�ѷ����� ���� ���� �߰����� �ʿ�� �˷������)
                playerPosition = playerController.transform.position, // ���� �÷��̾��� ��ġ�� ����
                // items = items, // ������ ��� (���� ����)
                // traps = traps // ���� ���� (���� ����)
                // ���⿡ �����Һκ� �߰����ֽø� �˴ϴ�.
            };

            // JSON���� ��ȯ�Ͽ� ���Ͽ� ����
            string json = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("���� �Ϸ�");
        }
        else
        {
            Debug.Log("�÷��̾� ��Ʈ�ѷ��� �����ϴ�");
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