﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : BlankMono
{
    [Header("Healthbar")]
    public GameObject healthBar;
    public GameObject barBorder;
    public Text characterName;

    [Header("Cooldowns")]
    public Image aBanner;
    Text aBannerText;
    public Image bBanner;
    Text bBannerText;
    public Image xBanner;
    Text xBannerText;
    public Image yBanner;
    Text yBannerText;

    [Header("Misc Stuff")]
    public GameObject playerBase;
    public string thisPlayerInt;
    public GameObject[] images = new GameObject[4];
    public GameObject image;
    private PlayerBase targetPlayer;

    public void SetStats(int imageInt, string charName)
    {

        aBannerText = aBanner.GetComponentInChildren<Text>();
        bBannerText = bBanner.GetComponentInChildren<Text>();
        xBannerText = xBanner.GetComponentInChildren<Text>();
        yBannerText = yBanner.GetComponentInChildren<Text>();

        characterName.text = charName;

        print(imageInt + "|" + images[imageInt].name);

        targetPlayer = playerBase.GetComponentInParent<PlayerBase>();
        healthBar.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);
        barBorder.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);


        image.SetActive(false);
        image = images[imageInt];
        image.SetActive(true);
    }

    public void Update()
    {
        healthBar.transform.localScale = Vector3.Lerp(healthBar.transform.localScale, new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1), 0.3f);

        if (targetPlayer.aTimer <= 0)
        {
            aBannerText.text = "";
            aBanner.color = Color.white;
        }
        else
        {
            aBanner.color = Color.grey;
            aBannerText.text = Mathf.RoundToInt(targetPlayer.aTimer) + "";
        }

        if (targetPlayer.bTimer <= 0)
        {
            bBannerText.text = "";
            bBanner.color = Color.white;
        }
        else
        {
            bBanner.color = Color.grey;
            bBannerText.text = Mathf.RoundToInt(targetPlayer.bTimer) + "";
        }

        if (targetPlayer.xTimer <= 0)
        {
            xBannerText.text = "";
            xBanner.color = Color.white;
        }
        else
        {
            xBanner.color = Color.grey;
            xBannerText.text = Mathf.RoundToInt(targetPlayer.xTimer) + "";
        }

        if (targetPlayer.yTimer <= 0)
        {
            yBannerText.text = "";
            yBanner.color = Color.white;
        }
        else
        {
            yBanner.color = Color.grey;
            yBannerText.text = Mathf.RoundToInt(targetPlayer.yTimer) + "";
        }

        if (targetPlayer.currentHealth < 0) targetPlayer.currentHealth = 0;
    }
}