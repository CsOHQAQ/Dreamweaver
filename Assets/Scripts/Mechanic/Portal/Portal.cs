using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Portal : MonoBehaviour
{
    public GameObject runeOnPortal1;
    public GameObject runeOnPortal2;

    public int portalRequiredRuneNumber1;
    public int portalRequiredRuneNumber2;

    void Start()
    {
        portalRequiredRuneNumber1 = 1;
        portalRequiredRuneNumber2 = 2;

        // Initialize the portal with the required rune numbers
        if (runeOnPortal1 != null)
        {
            runeOnPortal1.GetComponentInChildren<TextMeshProUGUI>().text = portalRequiredRuneNumber1.ToString();
        }

        if (runeOnPortal2 != null)
        {
            runeOnPortal2.GetComponentInChildren<TextMeshProUGUI>().text = portalRequiredRuneNumber2.ToString();
        }
    }

    void Update()
    {
        //let the runes rotate around the portal
        if (runeOnPortal1 != null)
        {
            runeOnPortal1.transform.RotateAround(transform.position, Vector3.up, 45 * Time.deltaTime);
        }

        if (runeOnPortal2 != null)
        {
            runeOnPortal2.transform.RotateAround(transform.position, Vector3.up, 45 * Time.deltaTime);
        }
    }

    public bool ClosePortal(int runeNumber1, int runeNumber2)
    {
        if ((runeNumber1 == portalRequiredRuneNumber1 && runeNumber2 == portalRequiredRuneNumber2) ||
            (runeNumber1 == portalRequiredRuneNumber2 && runeNumber2 == portalRequiredRuneNumber1))
        {
            Debug.Log("Portal closed with runes: " + runeNumber1 + " and " + runeNumber2);
            gameObject.SetActive(false); // Close the portal
            return true; // Indicate that the portal was successfully closed
        }

        return false; // Indicate that the portal was not closed
    }
}
