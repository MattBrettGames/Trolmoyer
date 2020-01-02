﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : BlankMono
{
    #region Songbird
    [Header("Songbird Inputs")]
    public GameObject poisonSmokeModel;
    public GameObject cannister;

    //    [HideInInspector] 
    public List<GameObject> poisonSmoke = new List<GameObject>();
    [HideInInspector] public List<GameObject> cannisters = new List<GameObject>();

    public void ReturnToPoisonSmokePool(GameObject gameobject) { poisonSmoke.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }

    #endregion

    #region Carman
    #endregion

    #region Skjegg
    [Header("Skjegg Input")]
    public GameObject ghost;

    [HideInInspector] public List<GameObject> ghostList = new List<GameObject>();
    public void ReturnToGhostList(GameObject gameobject) { ghostList.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }
    #endregion

    #region Wiosna
    [Header("Wisona")]
    public GameObject flamingClone;
    [HideInInspector] public List<GameObject> cloneList = new List<GameObject>();

    #endregion

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            #region Songbird Props
            poisonSmoke.Add(Instantiate<GameObject>(poisonSmokeModel, transform.position, Quaternion.identity, transform));
            poisonSmoke[i].SetActive(false);

            cannisters.Add(Instantiate<GameObject>(cannister, transform.position, Quaternion.identity, transform));
            cannisters[i].SetActive(false);
            #endregion

            #region Carman Props
            #endregion

            #region Wiosna Props
            cloneList.Add(Instantiate(flamingClone, Vector3.zero, Quaternion.identity));
            cloneList[i].SetActive(false);
            #endregion
        }

        for (int i = 0; i < 15; i++)
        {
            #region Skjegg & WarBanner Props
            ghostList.Add(Instantiate<GameObject>(ghost, transform.position, Quaternion.identity));
            ghostList[i].SetActive(false);
            #endregion
        }
    }

    public GameObject ReturnSmokeCloud(int listIndex)
    {
        return poisonSmoke[listIndex];
    }
}