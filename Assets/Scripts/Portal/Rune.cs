using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rune : MonoBehaviour
{
    public GameObject runePrefab;
    public int runeNumber;
    public GameObject UICanvas;
    public TextMeshProUGUI text;
    public bool isGenerated = false;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - UICanvas.transform.position;
        text.text = runeNumber.ToString();
        //runeNumber = int.Parse(text.text);

        if (isGenerated)
        {
            StartCoroutine(CanFusion());
        }
        else
        {
            // Ensure the UICanvas is positioned correctly at the start
            UICanvas.transform.position = transform.position - offset;
        }
    }

    void Update()
    {
        UICanvas.transform.position = transform.position - offset;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (this.GetInstanceID() > collision.gameObject.GetInstanceID() || !collision.gameObject.CompareTag("Rune") || isGenerated)
            return;

        Debug.Log("Collision detected with: " + collision.gameObject.name);

        ContactPoint contact = collision.contacts[0];
        Vector3 spawnPosition = contact.point;
        Quaternion spawnRotation = Quaternion.identity;

        // Check if the spawn position is nearby a portal
        Collider[] colliders = Physics.OverlapSphere(spawnPosition, 5f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Portal"))
            {
                //Todo: close portal
                // collider.gameObject.SetActive(false);
                // collision.gameObject.transform.parent.gameObject.SetActive(false);
                // transform.parent.gameObject.SetActive(false);

                if(collider.gameObject.GetComponent<Portal>().ClosePortal(runeNumber, collision.gameObject.GetComponent<Rune>().runeNumber))
                {
                    // Successfully closed the portal
                    collision.gameObject.transform.parent.gameObject.SetActive(false);
                    transform.parent.gameObject.SetActive(false);
                    return;
                }
            }
        }

        GameObject newRune = Instantiate(runePrefab, spawnPosition, spawnRotation);
        int collisionNumber = collision.gameObject.GetComponent<Rune>().runeNumber;
        newRune.GetComponentInChildren<Rune>().text.text = runeNumber + collisionNumber + "";
        newRune.GetComponentInChildren<Rune>().runeNumber = runeNumber + collisionNumber;
        isGenerated = true;

        //todo: destory runes but currently just disable them
        collision.gameObject.transform.parent.gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

    // Coroutine to reset isGenerated after a delay
    IEnumerator CanFusion()
    {
        yield return new WaitForSeconds(1f);
        isGenerated = false;
    }
}
