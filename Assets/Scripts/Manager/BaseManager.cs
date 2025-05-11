using UnityEngine;

/// <summary>
/// This is a base class of singleton Manager
/// </summary>
public abstract class BaseManager<T> : MonoSingleton<T> where T : BaseManager<T>
{
    protected override void Init()
    {
        base.Init();
    }
    
    public virtual void OnGameStart() {}
    public virtual void OnGameReset() {}
}
