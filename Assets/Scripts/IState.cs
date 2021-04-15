using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnUpdate();
    void Move();
    void Rest();
    void CheckEnergy();
}
