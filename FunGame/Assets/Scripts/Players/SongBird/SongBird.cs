﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongBird : PlayerBase
{

    [SerializeField] string bSoundBonus;

    [Header("More Components")]
    public CorvidDagger weapon;
    private ObjectPooler pooler;
    private GameObject smokeCloud;
    private GameObject smokeCloudCannister;
    private GameObject smokeCloudDodge;
    private GameObject cannister;

    [Header("Dagger Swipe")]
    public int baseXDamage;
    public int baseXKnockback;

    [Header("Cannister Cloud")]
    [SerializeField] int cannisterCloudSize;
    [SerializeField] int cannisterBurstDamage;
    [SerializeField] float cannisterPoisonTime;
    [SerializeField] int cannisterSmokeKnockback;

    [Header("Dodge Cloud")]
    [SerializeField] int dodgeCloudSize;
    [SerializeField] int dodgeBurstDamage;
    [SerializeField] float dodgePoisonTime;
    [SerializeField] int dodgeSmokeKnockback;

    [Header("Thrown Cloud")]
    [SerializeField] int thrownCloudSize;
    [SerializeField] int thrownBurstDamage;
    [SerializeField] float thrownCloudTime;
    [SerializeField] int thrownSmokeKnockback;

    [Header("Death Cloud")]
    [SerializeField] int deathCloudSize;
    [SerializeField] int deathBurstDamage;
    // [SerializeField] float deathCloudTime;
    // [SerializeField] int deathSmokeKnockback;

    [Space]

    private bool hasCannister;



    public override void SetInfo(UniverseController uni, int layerNew)
    {
        base.SetInfo(uni, layerNew);
        pooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        Invoke("GainSmokes", 0.1f);
    }

    void GainSmokes()
    {

        print(playerID);
        print(playerID * 2 + 1);
        print(playerID + 2 + 2);

        smokeCloud = pooler.ReturnSmokeCloud(playerID);
        smokeCloud.tag = tag;
        smokeCloudCannister = pooler.ReturnSmokeCloud(playerID * 2 + 1);//pooler.poisonSmokeList.Count - (playerID + 1));
        smokeCloudCannister.tag = tag;
        smokeCloudDodge = pooler.ReturnSmokeCloud(playerID + 2 + 2);
        smokeCloudDodge.tag = tag;


        cannister = pooler.cannisters[playerID];
        cannister.tag = tag;
        hasCannister = true;
    }

    public override void XAction()
    {
        if (xTimer <= 0)
        {
            anim.SetTrigger("XAttack");
            weapon.GainInfo(baseXDamage, baseXKnockback, visuals.transform.forward, pvp, 0, this);
            xTimer = xCooldown;
            universe.PlaySound(xSound);
        }
    }

    public override void YAction()
    {
        if (yTimer <= 0)
        {
            anim.SetTrigger("YAction");
            yTimer = yCooldown;
            universe.PlaySound(ySound);
        }
    }

    public override void BAction()
    {
        if (hasCannister)
        {
            if (bTimer <= 0)
            {
                cannister.transform.position = transform.position;
                cannister.SetActive(true);
                hasCannister = false;
                anim.SetTrigger("BAction");
                bTimer = bCooldown;
                universe.PlaySound(bSound);
            }
        }
        else
        {
            smokeCloudCannister.transform.localScale = Vector3.one;
            cannister.GetComponent<Cannister>().TriggerBurst(smokeCloudCannister, cannisterBurstDamage, cannisterCloudSize, cannisterSmokeKnockback, lookAtTarget.gameObject, cannisterPoisonTime);
            hasCannister = true;
            universe.PlaySound(bSoundBonus);
        }
    }

    public override void AAction()
    {
        if (aTimer <= 0)
        {
            anim.SetTrigger("AAction");
            state = State.dodging;

            Invoke("EndDodge", dodgeDur);

            smokeCloudDodge.transform.position = transform.position;
            smokeCloudDodge.transform.localScale = Vector3.zero;
            smokeCloudDodge.transform.rotation = new Quaternion(0, 0, 180, 0);
            smokeCloudDodge.SetActive(true);
            smokeCloudDodge.GetComponent<SmokeBase>().Begin(dodgeBurstDamage, dodgeSmokeKnockback, lookAtTarget.gameObject, dodgeCloudSize, dodgePoisonTime);

            for (int i = 0; i < dodgeCloudSize; i++)
            {
                StartCoroutine(smokeGrowth(i * 0.01f, smokeCloudDodge));
            }

            universe.PlaySound(aSound);
        }
    }

    private IEnumerator smokeGrowth(float time, GameObject smokecloud)
    {
        yield return new WaitForSeconds(time);
        smokecloud.transform.localScale += Vector3.one;
    }

    public void ThrowVial()
    {
        smokeCloud.transform.position = transform.position;
        smokeCloud.transform.localScale = Vector3.zero;
        smokeCloud.transform.rotation = new Quaternion(0, 0, 180, 0);
        smokeCloud.SetActive(true);
        smokeCloud.GetComponent<SmokeBase>().Begin(thrownBurstDamage, thrownSmokeKnockback, lookAtTarget.gameObject, thrownCloudSize, thrownCloudTime);

        for (int i = 0; i < thrownCloudSize; i++)
        {
            StartCoroutine(SmokeMove(smokeCloud, dir, i * 0.01f, false));
        }
    }

    public void DeathCloud()
    {
        smokeCloud.transform.position = transform.position;
        smokeCloud.transform.localScale = Vector3.zero;
        smokeCloud.transform.rotation = new Quaternion(0, 0, 180, 0);
        smokeCloud.SetActive(true);

        if (Vector3.Distance(gameObject.transform.position, lookAtTarget.position) <= deathCloudSize)
        {
            lookAtTarget.GetComponentInParent<PlayerBase>().TakeDamage(deathBurstDamage, true);
        }

        for (int i = 0; i < deathCloudSize; i++)
        {
            StartCoroutine(SmokeMove(smokeCloud, Vector3.zero, i * 0.005f, true));
        }
    }

    private IEnumerator SmokeMove(GameObject smokeCloud, Vector3 dir, float time, bool willShrink)
    {
        yield return new WaitForSeconds(time);
        smokeCloud.transform.position += dir * 2;
        smokeCloud.transform.localScale += Vector3.one;
        if (willShrink) StartCoroutine(ShrinkCloud(smokeCloud));
    }

    IEnumerator ShrinkCloud(GameObject smokeCloud)
    {
        yield return new WaitForSeconds(1);
        smokeCloud.transform.localScale -= Vector3.one;
    }

};