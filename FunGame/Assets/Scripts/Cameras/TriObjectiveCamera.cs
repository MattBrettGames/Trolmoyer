﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriObjectiveCamera : DualObjectiveCamera
{

    [SerializeField]
    public List<Transform> targets
    {
        get { return targets; }
        set
        {
            targets = value;
            RetryTargets();
        }
    }
    private Bounds boundBox;
    public float dampTime;
    GameObject blank;
    Vector3 velocity;

    public override void Start() { RetryTargets(); blank = new GameObject("BlankCameraTarget"); }

    public override void LateUpdate()
    {

        transform.position = boundBox.center + new Vector3(offset.x, Mathf.Max(boundBox.size.x, boundBox.size.z), offset.z);

        Vector3.SmoothDamp(transform.forward, blank.transform.forward, ref velocity, dampTime);

        blank.transform.LookAt(boundBox.center);

    }

    public void RemoveTarget(int targetNum)
    {
        targets.RemoveAt(targetNum);
        RetryTargets();
    }

    void RetryTargets()
    {
        boundBox = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            boundBox.Encapsulate(targets[i].position);
        }

        if (targets.Count <= 2)
        {
            GetComponent<DualObjectiveCamera>().enabled = true;
            this.enabled = false;
        }

    }

}