﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public abstract class PlayerBase : BlankMono
{
    [Header("GameMode Stuff")]
    public string thisPlayer;
    public int playerID;
    [HideInInspector] public int numOfDeaths = 0;
    public bool pvp;

    [Header("Movement Stats")]
    public float speed;
    public float dodgeSpeed;
    public float dodgeDur;
    private float baseSpeed;
    protected float moving;
    [HideInInspector] public Vector3 dir;

    [Header("Common Stats")]
    public int currentHealth;
    [HideInInspector] public int healthMax;
    public float damageMult = 1;
    public float incomingMult = 1;

    [Header("Status Effects")]
    [SerializeField] private int poisonPerSec;
    [SerializeField] private float secsBetweenTicks;
    [HideInInspector] public bool cursed;
    protected float curseTimer;
    [HideInInspector] public bool prone;
    [HideInInspector] public float poison;
    private bool hyperArmour;
    protected bool iFrames;
    protected bool counterFrames;
    protected bool acting;
    protected Vector3 knockbackForce;
    private float knockBackPower;
    [HideInInspector]
    public State state;
    [HideInInspector]
    public enum State
    {
        normal,
        lockedOn,
        dodging,
        knockback,
        attack,
        unique,
        stun
    }

    [Header("Components")]
    public Transform aimTarget;
    public GameObject visuals;
    protected Animator anim;
    protected Rigidbody rb2d;
    protected PlayerController playerCont;
    protected Player player;
    protected Transform lookAtTarget;
    protected Vector3 lookAtVariant = new Vector3(0, -5, 0);
    protected Transform walkDirection;
    protected UniverseController universe;

    [Header("Cooldowns")]
    public float aCooldown;
    public float bCooldown;
    public float xCooldown;
    public float yCooldown;
    [HideInInspector] public float aTimer;
    [HideInInspector] public float bTimer;
    [HideInInspector] public float xTimer;
    [HideInInspector] public float yTimer;

    [Header("Sound String")]
    [SerializeField] protected string aSound;
    [SerializeField] protected string bSound;
    [SerializeField] protected string xSound;
    [SerializeField] protected string ySound;
    [SerializeField] protected string ouchSound;


    public virtual void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        rb2d = gameObject.GetComponent<Rigidbody>();
        aTimer = aCooldown;
        baseSpeed = speed;

        healthMax = currentHealth;
        InvokeRepeating("PoisonTick", 0, secsBetweenTicks);
        player = ReInput.players.GetPlayer(playerID);
        walkDirection = Instantiate<GameObject>(aimTarget.gameObject, Vector3.zero, Quaternion.identity, gameObject.transform).transform;
    }

    public virtual void SetInfo(UniverseController uni)
    {
        universe = uni;
        if (playerID == 0) { lookAtTarget = GameObject.Find("Player2Base").transform; }
        else { lookAtTarget = GameObject.Find("Player1Base").transform; }
    }

    public virtual void Update()
    {
        if (aTimer > 0) aTimer -= Time.deltaTime;
        if (bTimer > 0) bTimer -= Time.deltaTime;
        if (xTimer > 0) xTimer -= Time.deltaTime;
        if (yTimer > 0) yTimer -= Time.deltaTime;
        print(aTimer);

        dir = new Vector3(player.GetAxis("HoriMove"), 0, player.GetAxis("VertMove")).normalized;

        if (poison > 0) { poison -= Time.deltaTime; }

        aimTarget.position = transform.position + dir * 5;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walking")) acting = false;

        switch (state)
        {
            case State.stun:
                anim.SetBool("Stunned", true);
                break;

            case State.attack:
                break;

            case State.normal:

                anim.SetBool("LockOn", false);
                if (player.GetAxis("LockOn") >= 0.4f) { state = State.lockedOn; }

                if (!prone && !acting)
                {
                    //Rotating the Character Model
                    visuals.transform.LookAt(aimTarget);
                    rb2d.velocity = dir * speed;

                    //Standard Inputs
                    if (player.GetButtonDown("AAction")) { AAction(); }
                    if (player.GetButtonDown("BAttack")) { BAction(); }
                    if (player.GetButtonDown("XAttack")) { XAction(); }
                    if (player.GetButtonDown("YAttack")) { YAction(); }

                    if (player.GetAxis("HoriMove") != 0 || player.GetAxis("VertMove") != 0) { anim.SetFloat("Movement", 1); }
                    else { anim.SetFloat("Movement", 0); }
                }
                else
                {
                    dir = Vector3.zero;
                }
                break;

            case State.lockedOn:

                walkDirection.position = dir + transform.position;

                anim.SetBool("LockOn", true);
                if (player.GetAxis("LockOn") <= 0.4f) { state = State.normal; }

                if (!prone && !acting)
                {
                    rb2d.velocity = dir * speed;

                    if (player.GetButtonDown("AAction")) { AAction(); }
                    if (player.GetButtonDown("BAttack")) { BAction(); }
                    if (player.GetButtonDown("XAttack")) { XAction(); }
                    if (player.GetButtonDown("YAttack")) { YAction(); }

                    if (player.GetAxis("HoriMove") != 0 || player.GetAxis("VertMove") != 0) { anim.SetFloat("Movement", 1); }
                    else { anim.SetFloat("Movement", 0); }

                    anim.SetFloat("Movement_X", -Vector3.SignedAngle(dir.normalized, visuals.transform.forward.normalized, Vector3.up) * 0.09f);
                    anim.SetFloat("Movement_ZY", -Vector3.SignedAngle(dir.normalized, visuals.transform.forward.normalized, Vector3.up) * 0.09f);
                }

                aimTarget.LookAt(lookAtTarget.position + lookAtVariant);
                visuals.transform.forward = Vector3.Lerp(visuals.transform.forward, aimTarget.forward, 0.3f);

                break;

            case State.dodging:

                if (aTimer < 0)
                {
                    DodgeSliding(dir);
                }
                break;

            case State.knockback:
                KnockbackContinual();
                break;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (thisPlayer == "P2")
            {
                TakeDamage(3000, true);
            }
        }

    }

    #region Input Actions
    public virtual void AAction()
    {
        if (aTimer < 0 && dir != Vector3.zero)
        {

            anim.SetTrigger("AAction");

            state = State.dodging;

            Invoke("EndDodge", dodgeDur);

            universe.PlaySound(aSound);
        }
    }
    private void EndDodge()
    {
        state = State.normal;
        aTimer = aCooldown;
    }

    public virtual void BAction() { }
    public virtual void XAction() { }
    public virtual void YAction() { }
    #endregion

    #region Common Events
    public virtual void TakeDamage(int damageInc, bool fromAttack)
    {
        if (!counterFrames && !iFrames)
        {
            ControllerRumble(0.2f, 0.1f);
            universe.CameraRumbleCall();
            if (fromAttack)
            {
                StartCoroutine(HitStun(0.01f));
            }
            HealthChange(Mathf.RoundToInt(-damageInc * incomingMult));
            if (currentHealth > 0) { anim.SetTrigger("Stagger"); }
            universe.PlaySound(ouchSound);
        }
    }
    IEnumerator HitStun(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        EndTimeScale();
    }

    private void EndTimeScale() { Time.timeScale = 1; }

    public void BecomeStunned(float duration)
    {
        state = State.stun;
        Invoke("EndStun", duration);
        anim.SetBool("Stunned", false);
    }
    void EndStun()
    {
        state = State.normal;
    }

    public virtual void Death()
    {
        universe.PlayerDeath(gameObject, lookAtTarget.gameObject);
        GainIFrames();

        dir = Vector3.zero;
        rb2d.velocity = Vector3.zero;
        anim.SetFloat("Movement", 0);

        Time.timeScale = 1;
        anim.SetTrigger("Death");
        this.enabled = false;
    }
    public virtual void KnockbackContinual()
    {
        transform.position += knockbackForce * knockBackPower * Time.deltaTime;
        visuals.transform.LookAt(lookAtTarget.position + lookAtVariant);
    }
    public virtual void Knockback(int power, Vector3 direction)
    {
        knockbackForce = direction;
        knockBackPower = power * 10;
        state = State.knockback;
        Invoke("StopKnockback", power * 0.01f);
    }
    public void StopKnockback() { knockbackForce = Vector3.zero; state = State.normal; }
    #endregion

    #region Utility Functions
    public virtual void HealthChange(int healthChange) { currentHealth += healthChange; if (currentHealth <= 0) { Death(); } }

    public void GainHA() { hyperArmour = true; }
    public void LoseHA() { hyperArmour = false; }

    public void GainIFrames() { iFrames = true; }
    public void LoseIFrames() { iFrames = false; }

    public virtual void Respawn()
    {

        currentHealth = healthMax;
        cursed = false;
        curseTimer = 0;
        poison = 0;
        prone = false;
        gameObject.SetActive(true);
        GainIFrames();
        Invoke("LoseIFrames", 3);
        anim.SetTrigger("Respawn");
        damageMult = 1;
        incomingMult = 1;

        EndActing();
        anim.SetFloat("Movement", 0);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        visuals.transform.localRotation = new Quaternion(0, 0, 0, 0);
    }
    protected void PoisonTick() { if (poison > 0) { currentHealth -= poisonPerSec; ControllerRumble(0.1f, 0.05f); } }

    public virtual void BeginActing() { acting = true; rb2d.velocity = Vector3.zero; state = State.attack; }
    public void EndActing() { acting = false; rb2d.velocity = Vector3.zero; state = State.normal; }

    public void ControllerRumble(float intensity, float dur)
    {
        player.SetVibration(1, intensity, dur);
        player.SetVibration(0, intensity, dur);
    }

    public virtual void DodgeSliding(Vector3 dir) { transform.position += dir * dodgeSpeed * Time.deltaTime; visuals.transform.LookAt(aimTarget); }

    #endregion

}