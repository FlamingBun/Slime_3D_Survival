using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;
    public static PoolManager Instance { get { return instance; } }

    private Transform poolTransform;
    private Dictionary<int, Queue<Component>> poolDictionary = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            poolTransform = this.transform;
        }
        else
        {
            Destroy(this);
        }
    }

    public void CreatePool<T>(GameObject prefab, int poolSize) where T : Component
    {
        int poolKey = prefab.GetInstanceID();
        string prefabName = prefab.name;

        GameObject parentGameObject = new GameObject(prefabName + "Pool");
        parentGameObject.transform.SetParent(poolTransform);

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<Component>());

            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab, parentGameObject.transform);
                newObject.SetActive(false);

                T component = newObject.GetComponent<T>();
                
                poolDictionary[poolKey].Enqueue(component);
            }
        }
    }

    public T ReuseComponent<T>(GameObject prefab, Vector3 position, Quaternion rotation) where T : Component
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            T componentToReuse = GetComponentFromPool<T>(poolKey);
            ResetObject(position, rotation, componentToReuse, prefab);
            return componentToReuse;
        }
        else
        {
            Debug.LogWarning($"[PoolManager] 풀에 {prefab.name} 이(가) 없습니다.");
            return null;
        }
    }

    private T GetComponentFromPool<T>(int poolKey) where T : Component
    {
        Component component = poolDictionary[poolKey].Dequeue();
        poolDictionary[poolKey].Enqueue(component);

        if (component.gameObject.activeSelf)
        {
            component.gameObject.SetActive(false);
        }

        return component as T;
    }

    private void ResetObject(Vector3 position, Quaternion rotation, Component componentToReuse, GameObject prefab)
    {
        Transform t = componentToReuse.transform;
        t.position = position;
        t.rotation = rotation;
        // t.localScale = prefab.transform.localScale;
    }
}
