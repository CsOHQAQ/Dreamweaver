using UnityEngine;

public class ControllableManager : BaseManager<ControllableManager>
{
    [SerializeField] 
    private PlayerController player;                // Always stores the player
    [SerializeField]    
    private BaseControllable currentControllable;   // Keep track of current one
    [SerializeField]
    private BaseControllable previousControllable;  // Keep track of previous one

    void Start()
    {
        OnStart();
    }

    protected override void OnStart()
    {
        base.OnStart();
        Init();
    }

    protected override void OnReset()
    {
        base.OnReset();
        Init();
    }

    private void Init() 
    {
        player = FindObjectOfType<PlayerController>();
        currentControllable = player;   // The player should be always be the first controllable.
        previousControllable = null;
    }

    public void ChangeControllable(BaseControllable newControllable) 
    {
        if (newControllable == null || currentControllable == newControllable) return;

        if (currentControllable != null) 
        {
            currentControllable.SetControl(false);
            currentControllable.DisableControl();
        }
        previousControllable = currentControllable;
        // TODO: Update camera logic

        currentControllable = newControllable;
        // TODO: Update camera logic
        currentControllable.SetControl(true);
        currentControllable.EnableControl();
    }

}
