using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEditor.Animations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    public List<Boid> allBoids = new List<Boid>();

    public float globalViewDistance;
    public float globalCohesionWeight;
    public float globalAlignWeight;
    public float globalSeparationWeight;

    public float globalXLimit = 17.5f;
    public float globalZLimit = 9.5f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}