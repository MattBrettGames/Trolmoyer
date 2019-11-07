﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : BlankMono
{
    #region Songbird
    [Header("Songbird Inputs")]
    public GameObject poisonSmokeModel;
    public GameObject adrenalineSmokeModel;
    public GameObject boomSmokeModel;

   [HideInInspector] public List<GameObject> poisonSmoke = new List<GameObject>();
    [HideInInspector] public List<GameObject> adrenalineSmoke = new List<GameObject>();
    [HideInInspector] public List<GameObject> boomSmoke = new List<GameObject>();
    public void ReturnToPoisonSmokePool(GameObject gameobject) { poisonSmoke.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }
    public void ReturnToAdrenalineSmokePool(GameObject gameobject) { adrenalineSmoke.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }
    public void ReturnToBoomSmokePool(GameObject gameobject) { boomSmoke.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }

    #endregion

    #region Carman
    [Header("Carman Inputs")]
    public GameObject curseCircle;
    public GameObject curseTrap;

   [HideInInspector] public List<GameObject> curseCircleList = new List<GameObject>();
    [HideInInspector] public List<GameObject> curseTrapList = new List<GameObject>();
    public void ReturnToCurseCircleList(GameObject gameobject) { curseCircleList.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }
    public void ReturnToCurseTrapList(GameObject gameobject) { curseTrapList.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }
    #endregion

    #region Skjegg
    [Header("Skjegg Input")]
    public GameObject ghost;

    [HideInInspector] public List<GameObject> ghostList = new List<GameObject>();
    public void ReturnToGhostList(GameObject gameobject) { ghostList.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }
    #endregion

    #region enemies
    [Header("Enemies")]
    public GameObject warBanner;
    [HideInInspector] public List<GameObject> warBanners = new List<GameObject>();
    public void ReturnToWarBanner() { }
    #endregion

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            #region Songbird Props
            poisonSmoke.Add(Instantiate<GameObject>(poisonSmokeModel, transform.position, Quaternion.identity));
            poisonSmoke[i].SetActive(false);

            adrenalineSmoke.Add(Instantiate<GameObject>(adrenalineSmokeModel, transform.position, Quaternion.identity));
            adrenalineSmoke[i].SetActive(false);

            boomSmoke.Add(Instantiate<GameObject>(boomSmokeModel, transform.position, Quaternion.identity));
            boomSmoke[i].SetActive(false);
            #endregion

            #region Carman Props
            curseCircleList.Add(Instantiate<GameObject>(curseCircle, transform.position, Quaternion.identity));
            curseCircleList[i].SetActive(false);

            curseTrapList.Add(Instantiate<GameObject>(curseTrap, transform.position, Quaternion.identity));
            curseTrapList[i].SetActive(false);
            #endregion

        }

        for (int i = 0; i < 15; i++)
        {
            #region Skjegg & WarBanner Props
            ghostList.Add(Instantiate<GameObject>(ghost, transform.position, Quaternion.identity));
            ghostList[i].SetActive(false);

            warBanners.Add(Instantiate<GameObject>(warBanner, transform.position, Quaternion.identity));
            warBanners[i].SetActive(false);
            #endregion
        }
    }

}
