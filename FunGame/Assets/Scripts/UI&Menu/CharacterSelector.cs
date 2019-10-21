﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Rewired;
public class CharacterSelector : BlankMono
{
    public string thisPlayer;
    public int thisPInt;

    private string horiPlayerInput;
    private string vertPlayerInput;
    
    int currentChar;
    int currentSkin;

    private bool inputCooldown;

    [Header("Skin Inputter")]
    public List<ModelList> characters = new List<ModelList>();

    [Header("Readying Up")]
    public bool locked;
    public UniverseController universe;

    [Header("UI Elements")]
    public Text characterText;
    public Text skinText;
    public GameObject displayChar;

    void Start()
    {
        transform.tag = thisPlayer + "Selector";
        horiPlayerInput = thisPlayer + "Horizontal";
        vertPlayerInput = thisPlayer + "Vertical";
        displayChar.SetActive(true);
        skinText.text = characters[currentChar].skins[currentSkin].name.ToString();
        characterText.text = characters[currentChar].name.ToString();
    }

    void Update()
    {
        //Debug.Log(string.Format("GO: {0} | InputName:  {1} | InVal: {2}", gameObject.name, horiPlayerInput, Input.GetAxis(horiPlayerInput)));
        if (!locked)
        {
            if (Input.GetAxis(horiPlayerInput) >= 0.4f && !inputCooldown)
            {
                inputCooldown = true;
                if (currentChar < characters.Count - 1)
                {
                    for (int i = 0; i < 80; i++) { StartCoroutine(SpinTrigger(5, i)); }
                    currentChar++;
                    UpdateDisplay();
                }
                else
                {
                    for (int i = 0; i < 80; i++) { StartCoroutine(SpinTrigger(5, i)); }
                    currentChar = 0;
                    UpdateDisplay();
                }

                Invoke("EndCooldown", 0.3f);
                currentSkin = 0;
            }
            if (Input.GetAxis(horiPlayerInput) <= -0.4f && !inputCooldown)
            {
                inputCooldown = true;
                if (currentChar != 0)
                {
                    for (int i = 0; i < 80; i++) { StartCoroutine(SpinTrigger(-5, i)); }
                    currentChar--;
                    currentSkin = 0;
                    UpdateDisplay();
                }
                else
                {
                    for (int i = 0; i < 80; i++) { StartCoroutine(SpinTrigger(-5, i)); }
                    currentChar = characters.Count - 1;
                    currentSkin = 0;
                    UpdateDisplay();
                }
                Invoke("EndCooldown", 0.3f);
            }

            if (Input.GetAxis(vertPlayerInput) >= 0.4f && !inputCooldown)
            {
                inputCooldown = true;
                if (currentSkin < characters[currentChar].skins.Count - 1)
                {
                    currentSkin++;
                    UpdateDisplay();
                }
                else
                {
                    currentSkin = 0;
                    UpdateDisplay();
                }
                Invoke("EndCooldown", 0.3f);
            }
            if (Input.GetAxis(vertPlayerInput) <= -0.4f && !inputCooldown)
            {
                inputCooldown = true;
                if (currentSkin != 0)
                {
                    currentSkin--;
                    UpdateDisplay();
                }
                else
                {
                    currentSkin = characters[currentChar].skins.Count - 1;
                    UpdateDisplay();
                }
                Invoke("EndCooldown", 0.3f);
            }

            if (Input.GetButtonDown(thisPlayer + "XButton"))
            {
                print(thisPlayer + "XButton");
                displayChar.transform.rotation = new Quaternion(0, 0, 0, 0);
                universe.CheckReady(thisPInt, displayChar);
                locked = true;
            }
        }
        if (Input.GetButtonDown(thisPlayer + "BButton"))
        {
            locked = false;
        }
    }

    private IEnumerator SpinTrigger(float angle, float time)
    {
        yield return new WaitForSeconds(time / 200);
        SpinJuice(angle);
    }
    private void SpinJuice(float angle)
    {
        displayChar.transform.Rotate(new Vector3(0, angle, 0));
    }

    public void UpdateDisplay()
    {
        skinText.text = characters[currentChar].skins[currentSkin].name.ToString();
        characterText.text = characters[currentChar].name.ToString();
        displayChar.SetActive(false);
        displayChar = characters[currentChar].skins[currentSkin].Skin;
        displayChar.SetActive(true);
    }

    public void SetUniverse(UniverseController universeTemp)
    {
        universe = universeTemp;
    }

    void EndCooldown()
    {
        inputCooldown = false;
    }

    [Serializable]
    public struct ModelList
    {
        public string name;
        public List<SkinFo> skins;
    }

    [Serializable]
    public struct SkinFo
    {
        public string name;
        public GameObject Skin;
    }

}
