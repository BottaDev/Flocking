using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // If is Boid...
        if (other.gameObject.layer == 8)
            Destroy(this.gameObject);
    }
}
