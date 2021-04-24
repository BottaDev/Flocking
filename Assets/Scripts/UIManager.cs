using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider globalViewDistance;
    public Slider globalCohesionWeight;
    public Slider globalAlignWeight;
    public Slider globalSeparationWeight;

    private void Start()
    {
        globalViewDistance.value = GameManager.instance.globalViewDistance;
        globalCohesionWeight.value = GameManager.instance.globalCohesionWeight;
        globalAlignWeight.value = GameManager.instance.globalAlignWeight;
        globalSeparationWeight.value = GameManager.instance.globalSeparationWeight;
    }

    public void ChangeViewDistance()
    {
        GameManager.instance.globalViewDistance = globalViewDistance.value;
    }

    public void ChangeCohesion()
    {
        GameManager.instance.globalCohesionWeight = globalCohesionWeight.value;
    }

    public void ChangeAlign()
    {
        GameManager.instance.globalAlignWeight = globalAlignWeight.value;
    }

    public void ChangeSeparation()
    {
        GameManager.instance.globalSeparationWeight = globalSeparationWeight.value;
    }
}
