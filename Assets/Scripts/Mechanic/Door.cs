using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpening;
    public float OpenSpeed;
    Vector3 originalPos;
    
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpening)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos + Vector3.down * 10,OpenSpeed*Time.deltaTime);
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, originalPos, OpenSpeed * Time.deltaTime);

        isOpening = false;
    }

    public void Open()
    {
        isOpening = true;
    }
}
