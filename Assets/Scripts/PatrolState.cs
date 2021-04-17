using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PatrolState :  IState
{
    private StateMachine _sm;
    private readonly Hunter _hunter;
    
    public PatrolState(Hunter hunter, StateMachine sm)
    {
        _sm = sm;
        _hunter = hunter;
    }

    public void OnUpdate()
    {
        Move();
        CheckEnergy();
    }

    public void Rest()
    {
        _hunter.isResting = true;
        _hunter.SetCurrentEnergy(_hunter.energy);
        _sm.ChangeState("RestState");
    }

    public void CheckEnergy()
    {
        _hunter.SetCurrentEnergy(_hunter.GetCurrentEnergy() - Time.deltaTime);
        
        if (_hunter.GetCurrentEnergy() <= 0 && !_hunter.isResting)
            Rest();
    }

    public void Move()
    {
        Collider[] colls = Physics.OverlapSphere(_hunter.transform.position, _hunter.viewDistance, 1<<8);

        if (colls.Length > 0)
        {
            _hunter.target = colls[0].transform.parent.root.gameObject.GetComponent<Boid>();
            _sm.ChangeState("ChaseState");
        }
        else
        {
            Vector3 pointDistance = _hunter.wayPoints[_hunter.currentWayPoint].transform.position - _hunter.transform.position;
        
            if (pointDistance.magnitude < _hunter.stoppingDistance)
            {
                _hunter.currentWayPoint++;
                if (_hunter.currentWayPoint > _hunter.wayPoints.Length - 1)
                    _hunter.currentWayPoint = 0;
            }
        
            Seek();   
        }

        _hunter.transform.position += _hunter.GetVelocity() * Time.deltaTime;
        _hunter.transform.forward = _hunter.GetVelocity();
    }
    
    private void Seek()
    {
        Vector3 desired;
        desired = _hunter.wayPoints[_hunter.currentWayPoint].transform.position - _hunter.transform.position;
        desired.Normalize();
        desired *= _hunter.maxSpeed;

        Vector3 steering = desired - _hunter.GetVelocity();
        steering = Vector3.ClampMagnitude(steering, _hunter.maxForce);

        _hunter.SetVelocity(Vector3.ClampMagnitude(_hunter.GetVelocity() + steering, _hunter.maxSpeed));
    }
}
