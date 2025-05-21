using UnityEngine;
using Cinemachine;
using System.Threading.Tasks;
using System.Collections;


public class CameraSmoothMove : MonoBehaviour
{
    [Tooltip("The Camera Transition speed")]
    [SerializeField][Range(0f, 10f)] private float blendTime = 1.0f;
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private GameObject currLookAt;
    private Coroutine blendingCoroutine = null;

    void Start()
    {
        currLookAt.SetActive(false);
    }

    public async Task SmoothBlendTo(Transform newTarget)
    {
        ActiveDummy();
        if (blendingCoroutine != null)
        {
            StopCoroutine(blendingCoroutine);
            blendingCoroutine = null;
        }
        var tcs = new TaskCompletionSource<bool>();
        blendingCoroutine = StartCoroutine(SmoothBlend(newTarget, tcs));
        await tcs.Task;
        blendingCoroutine = null;
    }

    private IEnumerator SmoothBlend(Transform target, TaskCompletionSource<bool> tcs)
    {
        Vector3 currLookAtPos = currLookAt.transform.position;
        Vector3 targetPos = target.position;
        float elapsed = 0.0f;

        camera.LookAt = currLookAt.transform;
        camera.Follow = currLookAt.transform;

        while (elapsed < blendTime)
        {
            currLookAt.transform.position = Vector3.Lerp(currLookAtPos, targetPos, elapsed / blendTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        currLookAt.transform.position = targetPos;
        tcs.SetResult(true);
    }

    void ActiveDummy()
    {
        currLookAt.SetActive(true);
        currLookAt.transform.position = camera.LookAt.position;
    }

    void DeactiveDummy()
    {
        currLookAt.SetActive(false);
    }
}
