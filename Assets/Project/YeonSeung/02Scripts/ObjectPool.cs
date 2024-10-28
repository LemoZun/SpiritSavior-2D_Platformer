using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static List<PooledObject> ObjectPools = new List<PooledObject>();

    private GameObject _objectPoolEmptyHolder;

    private static GameObject _particleSystemsEmpty;
    private static GameObject _gameObjectsEmpty;


    // Hierarchy ����
    public enum PoolType
    {
        ParticleSystem, GameObject, None
    }

    public static PoolType PoolingType;

    private void Awake()
    {
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects");

        _particleSystemsEmpty = new GameObject("Particle Effects");
        _particleSystemsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _gameObjectsEmpty = new GameObject("GameObjects");
        _gameObjectsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
    }




    public static GameObject SpawnObject(GameObject objToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        PooledObject pool = ObjectPools.Find(p => p.checkString == objToSpawn.name);

        // �������� �ʴ� pool�̸� �����
        if (pool == null)
        {
            pool = new PooledObject() { checkString = objToSpawn.name };
            ObjectPools.Add(pool);
        }

        // pool �� ��Ȱ�� object �ֳ� Ȯ��
        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();
        
        
        // ������, �����
        if (spawnableObj == null)
        {
            // EmptyObject �θ� ã��
            GameObject parentObject = SetParentObject(poolType);

            // ��Ȱ��ȭ�� object ������ �����
            spawnableObj = Instantiate(objToSpawn, spawnPosition, spawnRotation);

            // �θ�object������ SetParent
            if (parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
            }
        }

        // ������, Ȱ��ȭ
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }

    public static void ReturnObjectPool(GameObject obj)
    {
        string objName = obj.name.Substring(0, obj.name.Length - 7);
        // ���Ҷ� (clone) <-- �̰� 7���� ������ ���ؼ� �����
        // instantiate���� �Ǵٺ��� Ŭ���� �ȵǰ� ���ͼ� �װŶ� ���ϱ�����

        PooledObject pool = ObjectPools.Find(p => p.checkString == objName);
        if (pool == null)
        {
            // Debug.LogWarning($"pool �ȵ� object, {obj.name}");
            }
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
            // transform ������ �ʿ��ϸ� �ű⼭ �������ְų� �ؾߵ�
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.ParticleSystem:
                return _particleSystemsEmpty;
            case PoolType.GameObject:
                return _gameObjectsEmpty;
            case PoolType.None:
                return null;

            default:
                return null; 
        }
    }


    public class PooledObject
    {
        public string checkString;
        public List<GameObject> InactiveObjects = new List<GameObject>();
    }

}
