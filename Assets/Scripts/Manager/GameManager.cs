using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    [SerializeField]
    private BaseControllable currentControllable;
    [SerializeField]
    private BaseControllable prevControllable;

    [Tooltip("Testing Purpose")]
    public BaseControllable player;
    public BaseControllable dreamBody;
    public BaseControllable curr;

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
        if (currentControllable != null) 
        {
            currentControllable.SetControl(false);
            prevControllable = currentControllable;
        }

        prevControllable = currentControllable;
        currentControllable = newTarget;
        currentControllable.SetControl(true);

        Debug.Log($"{prevControllable.gameObject.name} Switched control to: {currentControllable.gameObject.name}");
    }

    // Anson: Testing purpose, Update should be change to event (e.g. line touch something and trigger)
    void Update() {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            curr = curr == null ? dreamBody : curr;
            EventManager.TriggerSwitchControl(curr);
            curr = prevControllable;
        }
    }
  
}
