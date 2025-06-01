using System.Threading.Tasks;
using Cinemachine;
using Unity.IO.LowLevel.Unsafe;
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
        EventManager.Instance.OnSwitchControl += ChangeControllable;
        EventManager.Instance.OnCameraBlendFinish += ActivateCurrControllable;
    }

    protected override void OnDestroy()
    {
        EventManager.Instance.OnSwitchControl -= ChangeControllable;
        EventManager.Instance.OnCameraBlendFinish -= ActivateCurrControllable;
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

        currentControllable = newControllable;

        CameraManager.Instance.SmoothBlendTo(currentControllable.GetLookAt());
    }

    public BaseControllable GetPlayerControllable()
    {
        return player;
    }

    private void ActivateCurrControllable()
    {
        CameraManager.Instance.SetUpFollowPoint(currentControllable.GetLookAt());
        currentControllable.SetControl(true);
        currentControllable.EnableControl();
        CameraManager.Instance.DeactiveDummy();
    }
}
