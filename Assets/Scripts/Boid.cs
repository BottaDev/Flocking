using System;
using System.Collections;
using System.Collections.Generic;
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

    private Vector3 _velocity;

    private void Start()
    {
        GameManager.instance.allBoids.Add(this);

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
        if (transform.position.z > 9) 
            transform.position = new Vector3(transform.position.x, transform.position.y, -7.5f);
        
        if (transform.position.z < -7.5f) 
            transform.position = new Vector3(transform.position.x, transform.position.y, 9);
        
        if (transform.position.x > 15) 
            transform.position = new Vector3(-15.5f, transform.position.y, transform.position.z);
        
        if (transform.position.x < -15.5f) 
            transform.position = new Vector3(15, transform.position.y, transform.position.z);
    }
    
    private void Move()
    {
        ApplyForce(CalculateSteering(SteeringType.Cohesion) * cohesionWeight + CalculateSteering(SteeringType.Align) * alignWeight + CalculateSteering(SteeringType.Separation) * separationWeight);

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
    
    private void ApplyForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, maxSpeed);
    }
    
    private Vector3 GetVelocity()
    {
        return _velocity;
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
