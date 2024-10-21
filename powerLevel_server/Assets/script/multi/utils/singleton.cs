using UnityEngine;

namespace PowerLevelNetwork.Core
{
public class singleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log($"Looking for singleton of type (typeof(T))");
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    Debug.Log("No singleton found. Creating...");
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning($"Duplicate singleton of (typeof(T)). Destroying...");
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {instance = null;}
    }
}
}