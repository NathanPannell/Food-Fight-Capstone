using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    public Toggle toggle;
    public Slider slider;
    Info info;

    [Header("Relevant Value")]
    public bool autoAim;
    public bool playerSpeed;

    private void Start()
    {
        info = GameObject.FindGameObjectWithTag("Highlander").GetComponent<Info>();

        if (autoAim)
        {
            toggle.isOn = info.isAutoAim;
        }

        if (playerSpeed)
        {
            slider.value = info.playerSpeed;
        }
    }

    private void Update()
    {
        if (autoAim)
        {
            info.isAutoAim = toggle.isOn;
        }

        if (playerSpeed)
        {
            info.playerSpeed = slider.value;
        }
    }
}
