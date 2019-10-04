using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCloudsController : MonoBehaviour
{
    private ParticleSystem turnClouds;
    
    void Start()
    {
        turnClouds = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if(turnClouds)
        {
            if(!turnClouds.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
