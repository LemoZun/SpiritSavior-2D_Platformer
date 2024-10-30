using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath; // ������ ���� ���
    private Coroutine autoSave;  // �ڵ� ����
    public PlayerController playerController;
    void Start()
    {
        // ���� ���� ���
        saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        // �ڵ� ���� ����
        autoSave = StartCoroutine(AutoSaveTime());
    }

    private IEnumerator AutoSaveTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(180f);

            SaveGame(playerController);
        }
    }
    // ���� ����
    public void SaveGame(PlayerController playerController)
    {
        if (playerController != null)
        {
            GameData gameData = new GameData
            {
                playerHp = playerController.hp,                       // ���� hp���� (�÷��̾� ��Ʈ�ѷ����� ���� ���� �߰����� �ʿ�� �˷������)
                playerPosition = playerController.transform.position, // ���� �÷��̾��� ��ġ�� ����
                // items = items, // ������ ��� (���� ����)
                // traps = traps // ���� ���� (���� ����)
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