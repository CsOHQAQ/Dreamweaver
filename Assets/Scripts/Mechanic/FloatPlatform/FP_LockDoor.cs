using UnityEngine;

public class FP_LockDoor : FloatPlatform
{
    protected override void StepForward()
    {
        if(RouteTransforms.Count==0)
            return;

        if(currentStation == -1)
            currentStation = 0;

        if (currentStation >= RouteTransforms.Count-1)
        {
            currentStation = RouteTransforms.Count-1;
            Platform.position = RouteTransforms[currentStation].position;
            enabled = false;
            return;
        }
        else
        {
            if (Vector3.Distance(Platform.position, RouteTransforms[currentStation + 1].position) < 0.05f)
            {
                currentStation++;
                Platform.position= RouteTransforms[currentStation].position;
            }
            else
            {
                Platform.position = Vector3.MoveTowards(Platform.position, RouteTransforms[currentStation + 1].position, MoveSpeed * Time.deltaTime);
            }
        }
    }
}
