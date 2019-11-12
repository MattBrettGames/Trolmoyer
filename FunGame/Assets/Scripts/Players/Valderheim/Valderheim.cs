﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Valderheim : PlayerBase
{
    [Header("More Componenets")]
    public Weapons hammer;
    public Outline outline;

    [Header("Wide Swing")]
    public int xAttack;
    public int xKnockback;

    [Header("Spin")]
    public int spinDamage;
    public int spinKnockback;

    [Header("Ground Slam")]
    public int slamAttack;
    public int slamKnockback;

    [Header("Kick Up")]
    public int kickAttack;
    public int kickKnockback;

    [Header("Frenzy")]
    public int frenzyDuration;
    public int frenzyBonus;
    private bool frenzy;
    public ParticleSystem frenzyEffects;

    [Header("Passives")]
    public int growingRageDiv;
    private bool comboTime;

    public override void Update()
    {
        dir = new Vector3(player.GetAxis("HoriMove"), 0, player.GetAxis("VertMove")).normalized;
        dodgeTimer -= Time.deltaTime;

        switch (state)
        {
            case State.normal:

                if (player.GetButtonDown("BAttack")) { BAction(); }

                if (!prone && !acting)
                {
                    //Rotating the Character Model
                    aimTarget.position = transform.position + dir * 5;
                    visuals.transform.LookAt(aimTarget);

                    rb2d.velocity = dir * speed;

                    //Standard Inputs
                    if (player.GetButtonDown("AAction")) { AAction(); }
                    if (player.GetButtonDown("XAttack")) { XAction(); }
                    if (player.GetButtonDown("YAttack")) { YAction(); }

                    if (player.GetAxis("HoriMove") != 0 || player.GetAxis("VertMove") != 0) { anim.SetFloat("Movement", 1); }
                    else { anim.SetFloat("Movement", 0); }
                }

                if (acting)
                {
                    dir = Vector3.zero;
                }
                if (poison > 0) { poison -= Time.deltaTime; }
                if (curseTimer <= 0) { LoseCurse(); }
                else { curseTimer -= Time.deltaTime; }
                break;

            case State.dodging:
                if (dodgeTimer <= 0) { DodgeSliding(dir); }
                break;

            case State.knockback:
                KnockbackContinual();
                break;
        }
    }

    public override void XAction()
    {
        if (!comboTime)
        {
            hammer.GainInfo(Mathf.RoundToInt(xAttack * damageMult), Mathf.RoundToInt(xKnockback * damageMult), visuals.transform.forward, pvp);
        }
        else
        {
            Vector3 dir = visuals.transform.forward;
            anim.SetBool("Comboing", true);
            hammer.GainInfo(Mathf.RoundToInt(spinDamage * damageMult), Mathf.RoundToInt(spinKnockback * damageMult), visuals.transform.forward, pvp);
        }
        anim.SetTrigger("XAttack");
    }

    public override void YAction()
    {
        if (!comboTime)
        {
            print("Hammer Slam");
            hammer.GainInfo(Mathf.RoundToInt(slamAttack * damageMult), Mathf.RoundToInt(slamKnockback * damageMult), visuals.transform.forward, pvp);
            anim.SetBool("Comboing", false);
        }
        else
        {
            print("Combo Kick");
            hammer.GainInfo(Mathf.RoundToInt(kickAttack * damageMult), Mathf.RoundToInt(kickKnockback * damageMult), visuals.transform.forward, pvp);
            anim.SetBool("Comboing", true);
        }
        anim.SetTrigger("YAttack");
    }
    public void OpenComboKick() { comboTime = true; outline.OutlineColor = new Color(1, 1, 1); }
    public void CloseComboKick() { comboTime = false; outline.OutlineColor = new Color(0, 0, 0); }

    public override void BAction()
    {
        if (!frenzy)
        {
            Invoke("StopFrenzy", frenzyDuration);
            anim.SetTrigger("BAttack");
            damageMult += frenzyBonus;
            incomingMult += frenzyBonus;
            frenzy = true;
            frenzyEffects.Play();
        }
    }
    private void StopFrenzy()
    {
        damageMult -= frenzyBonus;
        incomingMult -= frenzyBonus;
        frenzy = false;
        frenzyEffects.Clear();
        frenzyEffects.Stop();
    }

    public override void AAction()
    {
        dodgeTimer = dodgeCooldown;
        anim.SetTrigger("AAction");
        state = State.dodging;
        Invoke("EndDodge", dodgeDur);
    }

    public void EndDodge() { state = State.normal; }


    //Passive Effects - Surefooted & Building Rage
    public override void HealthChange(int healthChange) { base.HealthChange(healthChange); damageMult = Mathf.RoundToInt((healthMax - currentHealth) / growingRageDiv) + 1; }

}