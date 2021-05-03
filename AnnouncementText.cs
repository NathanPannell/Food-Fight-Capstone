using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnnouncementText : MonoBehaviour
{
    public TextMeshProUGUI tmp;

    public float defaultDisplayTime;
    public float additionalTimeModifier;
    public string text;
    string oldText;

    private void Start()
    {
        tmp.enabled = false;
    }

    private void Update()
    {
        tmp.text = text;
        if (oldText != text)
        {
            oldText = text;
            StartCoroutine(DisplayText());
        }
    }

    IEnumerator DisplayText()
    {
        tmp.enabled = true;
        Debug.Log("Waiting for: " + (defaultDisplayTime + text.Length) * additionalTimeModifier + " seconds.");
        yield return new WaitForSeconds(defaultDisplayTime + text.Length * additionalTimeModifier);
        tmp.enabled = false;
        text = "";
        oldText = "";
    }
}
