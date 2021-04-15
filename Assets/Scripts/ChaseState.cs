using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private StateMachine _sm;
    private readonly Hunter _hunter;
    
    public ChaseState(Hunter hunter, StateMachine sm)
    {
        _sm = sm;
        _hunter = hunter;
    }

    public void OnUpdate()
    {
        Move();
        CheckEnergy();
    }

    public void Move()
    {
        
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
}
