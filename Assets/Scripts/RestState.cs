using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestState : IState
{
    private StateMachine _sm;
    private readonly Hunter _hunter;
    private float _timer = 0;
    
    public RestState(Hunter hunter, StateMachine sm)
    {
        _sm = sm;
        _hunter = hunter;
    }

    public void OnUpdate()
    {
        Move();

        _timer += Time.deltaTime;
        
        if (_timer >= _hunter.restingTime && _hunter.isResting)
        {
            _hunter.isResting = false;
            _timer = 0;
            _sm.ChangeState("PatrolState");
        }
    }

    public void Move()
    {
        if (_hunter.GetVelocity() != new Vector3(0,0,0))
            _hunter.SetVelocity(new Vector3(0,0,0));    // Hunter stays idle...
    }

    public void CheckEnergy()
    {
    }
    
    public void Rest()
    {
    }
}
