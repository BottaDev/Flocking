using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boid : MonoBehaviour
{
    public float maxSpeed;
    [Range(0.01f, 1f)]
    public float maxForce;
    public float viewDistance;
    public float cohesionWeight;
    public float alignWeight;
    public float separationWeight;
    
    private Hunter _hunter;
    private Vector3 _velocity;

    private void Start()
    {
        GameManager.instance.allBoids.Add(this);

        _hunter = GameObject.FindObjectOfType<Hunter>();

        float randomX = Random.Range(-10, 10);
        float randomZ = Random.Range(-10, 10);

        Vector3 desired = new Vector3(randomX, 0, randomZ);
        desired.Normalize();
        desired *= maxSpeed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        ApplyForce(steering);
    }
    
    private void Update()
    {
        UpdateValues();
        CheckBounds();
        Move();
    }
    
    private void UpdateValues()
    {
        viewDistance = GameManager.instance.globalViewDistance;
        cohesionWeight = GameManager.instance.globalCohesionWeight;
        alignWeight = GameManager.instance.globalAlignWeight;
        separationWeight = GameManager.instance.globalSeparationWeight;
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
    
    private void Move()
    {
        Vector3 hunterDistance = _hunter.transform.position - transform.position;
        if (hunterDistance.magnitude <= viewDistance)
        {
            Evade();   
        }
        else
        {
            Collider[] colls = Physics.OverlapSphere(transform.position, viewDistance, 1<<9);

            if (colls.Length > 0)
                Arrive(colls[0].transform);
            else
                ApplyForce(CalculateSteering(SteeringType.Cohesion) * cohesionWeight +
                           CalculateSteering(SteeringType.Align) * alignWeight +
                           CalculateSteering(SteeringType.Separation) * separationWeight);
        }

        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity.normalized;
    }
    
    private Vector3 CalculateSteering(SteeringType type)
    {
        Vector3 desired = new Vector3();
        int visibleBoids = 0;

        foreach (var boid in GameManager.instance.allBoids)
        {
            if (boid != null && boid != this)
            {
                Vector3 dist = boid.transform.position - transform.position;
                if (dist.magnitude < viewDistance)
                {
                    if (type == SteeringType.Align)
                    {
                        desired.x += boid.GetVelocity().x;
                        desired.z += boid.GetVelocity().z;
                    }
                    else if (type == SteeringType.Cohesion)
                    {
                        desired.x += boid.transform.position.x;
                        desired.z += boid.transform.position.z;   
                    } 
                    else if (type == SteeringType.Separation)
                    {
                        desired.x += dist.x;
                        desired.z += dist.z;
                    }
                    
                    visibleBoids++;
                }
            }
        }
        
        if (visibleBoids == 0) 
            return desired;

        desired /= visibleBoids;

        desired -= transform.position;
        desired.Normalize();
        desired *= maxSpeed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(desired, maxForce);

        return steering;
    }
    
    private void Evade()
    {
        Vector3 objective = _hunter.transform.position + _hunter.GetVelocity();
    
        if (_hunter != null)
            _hunter.transform.position = objective;

        Vector3 desired = objective - transform.position;
        desired.Normalize();
        desired *= maxSpeed;
        desired *= -1;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);
    
        _velocity = Vector3.ClampMagnitude(_velocity + steering, maxSpeed);
    }
    
    private void Arrive(Transform target)
    {
        Vector3 desired;
        desired = target.transform.position - transform.position;

        float dist = (target.transform.position - transform.position).magnitude;
        
        float speed = Map(dist, 0, viewDistance, 0, maxSpeed);
            
        desired.Normalize();
        desired *= speed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        _velocity = Vector3.ClampMagnitude(_velocity + steering, maxSpeed);
    }
    
    private void ApplyForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, maxSpeed);
    }
    
    private Vector3 GetVelocity()
    {
        return _velocity;
    }

    float Map(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (from - toMin) / (fromMax - fromMin) * (toMax - toMin) + fromMin;
    }
    
    private enum SteeringType
    {
        Separation,
        Align,
        Cohesion
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}