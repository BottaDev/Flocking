using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    public float maxSpeed;
    [Range(0.01f, 1f)]
    public float maxForce;
    public Transform[] wayPoints;
    public float stoppingDistance;
    
    private Vector3 _velocity;
    private int _currentWayPoint = 0;
    
    private void Update()
    {
        CheckBounds();
        Move();
    }
    
    public Vector3 GetVelocity()
    {
        return _velocity;
    }
    
    private void Move()
    {
        Vector3 pointDistance = wayPoints[_currentWayPoint].transform.position - transform.position;
        
        if (pointDistance.magnitude < stoppingDistance)
        {
            _currentWayPoint++;
            if (_currentWayPoint > wayPoints.Length - 1)
                _currentWayPoint = 0;
        }
        
        Seek();
        
        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity.normalized;
    }
    
    private void Seek()
    {
        Vector3 desired;
        desired = wayPoints[_currentWayPoint].transform.position - transform.position;
        desired.Normalize();
        desired *= maxSpeed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        _velocity = Vector3.ClampMagnitude(_velocity + steering, maxSpeed);
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
