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

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}