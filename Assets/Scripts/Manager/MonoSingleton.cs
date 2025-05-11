using UnityEngine;

/// <summary>
/// A MonoSingleton class
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool Global => true;

    private static T instance;
    public static T Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null) 
                {
                    GameObject new_instance = new GameObject(typeof(T).Name);
                    instance = new_instance.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake() 
    {
        InitialSingleton();
    }

    protected virtual void OnStart() {}

    protected virtual void OnDestroy() {}

    protected virtual void Init() {}

    private void InitialSingleton() 
    {
        if (Global) 
        {
            if (instance != null && instance != instance.gameObject.GetComponent<T>()) 
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            instance = gameObject.GetComponent<T>();
        }
        Init();
    }
}
