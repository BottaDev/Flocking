using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private readonly Dictionary<string, IState> _stateDictionary = new Dictionary<string, IState>();

    private IState _currentState = new EmptyState();

    public void OnUpdate()
    {
        _currentState.OnUpdate();
    }
    
    public void ChangeState(string id)
    {
        _currentState = _stateDictionary[id];
    }

    public void AddState(string id, IState state)
    {
        _stateDictionary.Add(id, state);
    }
}

public class EmptyState : IState
{
    public void OnUpdate() { }
    public void Move() { }
    public void Rest() { }
    public void CheckEnergy() { }
}
