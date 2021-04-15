using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Hunter : MonoBehaviour
{
    [Header("Properties")]
    public float maxSpeed;
    [Range(0.01f, 1f)]
    public float maxForce;
    public float energy = 20f;
    public float restingTime = 5f;
    
    [Header("Movement")]
    public Transform[] wayPoints;
    public float stoppingDistance;
    [HideInInspector]
    public int currentWayPoint = 0;
    [HideInInspector]
    public bool isResting = false;

    private Vector3 _velocity;
    [SerializeField]
    private float _currentEnergy;
    private StateMachine _sm;

    private void Awake()
    {
        _sm = GetComponent<StateMachine>();

        _sm.AddState("PatrolState", new PatrolState(this, _sm));
        _sm.AddState("RestState", new RestState(this, _sm));
        _sm.AddState("ChaseState", new ChaseState(this, _sm));
        
        _sm.ChangeState("PatrolState");

        _currentEnergy = energy;
    }
    
    private void Update()
    {
        CheckBounds();
        _sm.OnUpdate();
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        _velocity = newVelocity;
    }
    
    public Vector3 GetVelocity()
    {
        return _velocity;
    }

    public float GetCurrentEnergy()
    {
        return _currentEnergy;
    }

    public void SetCurrentEnergy(float e)
    {
        _currentEnergy = e;
    }
    
    private void CheckBounds()
    {
        if (transform.position.z > GameManager.instance.globalZLimit) 
            transform.position = new Vector3(transform.position.x, transform.position.y, -GameManager.instance.globalZLimit);
        
        if (transform.position.z < -GameManager.instance.globalZLimit) 
            transform.position = new Vector3(transform.position.x, transform.position.y, GameManager.instance.globalZLimit);
        
        if (transform.position.x > GameManager.instance.globalXLimit) 
            transform.position = new Vector3(-GameManager.instance.globalXLimit, transform.position.y, transform.position.z);
        
        if (transform.position.x < -GameManager.instance.globalXLimit) 
            transform.position = new Vector3(GameManager.instance.globalXLimit, transform.position.y, transform.position.z);
    }
}
