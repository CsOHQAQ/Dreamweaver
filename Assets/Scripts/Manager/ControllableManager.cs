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

}
