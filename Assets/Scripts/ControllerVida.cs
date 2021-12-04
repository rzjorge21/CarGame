using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerVida : MonoBehaviour
{
    public Slider slider;

    public void setMaxVida(int vida)
    {
        slider.maxValue = vida;
        slider.value = 0;
    }

    public void SetVida(int vida)
    {
        slider.value = vida;
    }
}
