﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiosnaEventHandler : BlankMono
{

    public Wiosna wiosna;
    public Weapons melee;

    void Start()
    {
        melee.gameObject.SetActive(false);
    }


    public void BeginMelee() { melee.gameObject.SetActive(true); melee.StartAttack(); }
    public void EndMelee() { melee.EndAttack(); melee.gameObject.SetActive(false); }

    public void BeginActing() { wiosna.BeginActing(); }
    public void EndActing() { wiosna.EndActing(); }

    public void Vibration(float intensity, float dur) { wiosna.ControllerRumble(intensity, dur); }
    public void GainIFrmaes() { wiosna.GainIFrames(); }
    public void LoseIFrmaes() { wiosna.LoseIFrames(); }
}