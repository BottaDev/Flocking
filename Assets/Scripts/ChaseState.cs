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
        if (_hunter.target == null)
        {
            _sm.ChangeState("PatrolState");
            return;
        }
        
        Move();
        CheckEnergy();
    }

    public void Move()
    {
        Vector3 boidDistance = _hunter.target.transform.position - _hunter.transform.position;
        
        if (boidDistance.magnitude <= _hunter.viewDistance)
            Seek();
        else
            _hunter.target = null;

        _hunter.transform.position += _hunter.GetVelocity() * Time.deltaTime;
        _hunter.transform.forward = _hunter.GetVelocity();
    }

    private void Seek()
    {
        Vector3 desired;
        desired = _hunter.target.transform.position - _hunter.transform.position;
        desired.Normalize();
        desired *= _hunter.maxSpeed;

        Vector3 steering = desired - _hunter.GetVelocity();
        steering = Vector3.ClampMagnitude(steering, _hunter.maxForce);

        _hunter.SetVelocity(Vector3.ClampMagnitude(_hunter.GetVelocity() + steering, _hunter.maxSpeed));
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
