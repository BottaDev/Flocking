using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    [Header("Boid Settings")]
    public List<Boid> allBoids = new List<Boid>();

    public float globalViewDistance = 5;
    public float globalCohesionWeight = 0.8f;
    public float globalAlignWeight = 0.5f;
    public float globalSeparationWeight = 0.3f;

    [Header("Food Settings")]
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