using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    public BaseControllable CurrentControllable;
    public BaseControllable PrevControllable;
    private void Awake() {
        if (Instance == null) 
        {
            Instance = this;
        } 
        else 
        {
            Destroy(this);
        }
    }

    private void OnEnable() 
    {
        EventManager.OnSwitchControl += SetCurrControllable;
    }

    private void OnDisable() 
    {
        EventManager.OnSwitchControl -= SetCurrControllable;
    }

    public void SetCurrControllable(BaseControllable newTarget) {
        if (newTarget == CurrentControllable) return;
        if (CurrentControllable != null) 
        {
            CurrentControllable.SetControl(false);
            CurrentControllable.DisableControl();
            PrevControllable = CurrentControllable;
        }

        PrevControllable = CurrentControllable;
        if (PrevControllable != null) {
            PrevControllable.GetComponentInChildren<CinemachineFreeLook>().Priority = 10;
        }
        CurrentControllable = newTarget;
        CurrentControllable.GetComponentInChildren<CinemachineFreeLook>().Priority = 11;
        CurrentControllable.SetControl(true);
        CurrentControllable.EnableControl();
        Debug.Log($"{PrevControllable.gameObject.name} Switched control to: {CurrentControllable.gameObject.name}");
    }
}
