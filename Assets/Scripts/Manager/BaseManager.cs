using UnityEngine;

/// <summary>
/// This is a base class of singleton Manager
/// </summary>
public abstract class BaseManager<T> : MonoSingleton<T> where T : BaseManager<T>
{
    protected virtual void OnReset() {}
}
