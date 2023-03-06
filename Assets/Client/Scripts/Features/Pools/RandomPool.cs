using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRandomPool", menuName = "Pools/NewRandomPool")]
[System.Serializable]
public class RandomPool : ScriptableObject
{
    public int TotalObjectsCount { get; private set; }
    public int CurrentObjectsCount { get; private set; }
    public string ParentName { get; private set; }
    public GameObject[] Prefabs;
    public Queue<GameObject> ObjectPool { get; private set; }
    public Vector3 CreationPoint { get; set; }

    public GameObject Parent { get; private set; }

    public RandomPool(GameObject[] prefabs, Vector3 creationPoint, int objectsCount, string parentName = null)
    {
        if (objectsCount < 1)
        {
            Debug.LogError("ObjectsCount is less than zero!");
            return;
        }

        if (prefabs == null)
        {
            Debug.LogError("The pool did not have a prefab!");
            return;
        }

        Prefabs = prefabs;
        ObjectPool = new Queue<GameObject>();
        CreationPoint = creationPoint;
        ParentName = parentName ?? null;

        if (ParentName != null)
        {
            Parent = new GameObject();
            Parent.name = ParentName;
        }

        CreateObjects(objectsCount);
    }

    public void CreateObjects(int objectsCount)
    {
        for (int i = 0; i < objectsCount; i++)
        {
            int prefabIndex = Random.Range(0, Prefabs.Length);

            var poolObject = GameObject.Instantiate(Prefabs[prefabIndex], CreationPoint, Quaternion.identity);
            poolObject.SetActive(false);
            ObjectPool.Enqueue(poolObject);

            if (Parent != null)
            {
                poolObject.transform.SetParent(Parent.transform);
            }

            TotalObjectsCount += objectsCount;
            CurrentObjectsCount += objectsCount;
        }
    }

    public GameObject GetFromPool()
    {
        GameObject poolObject;

        if (ObjectPool.TryDequeue(out poolObject))
        {
            poolObject.SetActive(true);
        }
        else
        {
            CreateObjects(1);
            poolObject = ObjectPool.Dequeue();
            poolObject.SetActive(true);
        }

        CurrentObjectsCount--;

        return poolObject;
    }

    public void ReturnToPool(GameObject gameObject)
    {
        ObjectPool.Enqueue(gameObject);

        CurrentObjectsCount++;
    }
}
