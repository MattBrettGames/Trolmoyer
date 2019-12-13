﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : BlankMono
{
    private int damageFull;
    private int knockFull;
    private Vector3 knockDir;
    private Collider hitBox;
    private TrailRenderer trails;
    private bool pvpTrue;
    private float stunDurTrue;

    public void GainInfo(int damage, int knockback, Vector3 forward, bool pvp, float stunDur)
    {
        damageFull = damage;
        knockFull = knockback;
        knockDir = forward;
        pvpTrue = pvp;
        stunDurTrue = stunDur;
    }
    private void Start()
    {
        hitBox = gameObject.GetComponent<Collider>();
        hitBox.enabled = false;
        trails = gameObject.GetComponentInChildren<TrailRenderer>();
        trails.enabled = false;
    }

    public void StartAttack()
    {
        hitBox.enabled = true;
        trails.enabled = true;
    }

    public void EndAttack()
    {
        hitBox.enabled = false;
        trails.enabled = false;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (pvpTrue)
        {
            PlayerBase player = other.gameObject.GetComponent<PlayerBase>();
            print(player.name + " has been hit");
            if (player != null)
            {
                player.TakeDamage(damageFull);
                player.Knockback(knockFull, knockDir);
            }
        }
        else
        {
            if (other.transform.tag == "Enemy")
            {
                EnemyBase target = other.transform.GetComponent<EnemyBase>();
                target.TakeDamage(damageFull, tag);
            }
            else if (other.transform.tag == "Objective")
            {
                ObjectiveController target = other.GetComponent<ObjectiveController>();
                target.TakeDamage(damageFull);
            }
        }
    }


}
