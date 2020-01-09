﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingTrap : MonoBehaviour
{
    bool travelling;
    Vector3 dir;
    [SerializeField] Rigidbody rb2d;
    [SerializeField] float speed;
    [SerializeField] float maxDistance;
    float curDistance;
    Carmen carTrue;

    public void OnThrow(Vector3 dirNew, Carmen car, int layer)
    {
        travelling = true;
        dir = dirNew;
        carTrue = car;
        gameObject.layer = layer;
        Physics.IgnoreLayerCollision(layer, layer);
        transform.forward = dir;
        curDistance = 0;
        gameObject.transform.SetParent(null);

    }

    void Update()
    {
        if (travelling)
        {
            rb2d.velocity = dir * speed;
        }

        curDistance += Time.deltaTime;
        if (curDistance >= maxDistance)
        {
            End();
        }

    }

    void OnCollisionEnter(Collision other)
    {
        gameObject.transform.SetParent(other.transform);
        travelling = false;
        carTrue.GetLocation(gameObject.transform.position);
        rb2d.velocity = Vector3.zero;
    }

    public void End()
    {
        gameObject.SetActive(false);
        transform.position = new Vector3(0, -100, 0);
    }

}
