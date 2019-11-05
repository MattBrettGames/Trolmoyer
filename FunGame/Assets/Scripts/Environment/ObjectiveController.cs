﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveController : MonoBehaviour
{

    public int healthPool;



    void OnCollisionEnter(Collision other)
    { 

    }

    public void TakeDamage(int damage)
    {
        healthPool -= damage;
        if (healthPool <= 0)
        {
            GameObject.FindGameObjectWithTag("Boss").GetComponent<BossBase>().ObjectiveDestroyed();
        }
    }


}
