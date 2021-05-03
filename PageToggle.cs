using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageToggle : MonoBehaviour
{
    public Toggle toggle;
    public GameObject level1;

    private void Update()
    {
        if (toggle.isOn)
        {
            level1.SetActive(false);
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            level1.SetActive(true);
            transform.rotation = new Quaternion(0, 0, 180, 0);
        }
    }
}
