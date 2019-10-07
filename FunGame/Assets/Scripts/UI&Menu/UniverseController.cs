﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class UniverseController : BlankMono
{
    [Header("GameObjects")]
    public CharacterSelector charSelector1;
    public CharacterSelector charSelector2;
    public CharacterSelector charSelector3;
    public CharacterSelector charSelector4;

    [Header("Character Info")]
    public GameObject[] selectedChars = new GameObject[4];
    public int numOfPlayers;
    private int lockedInPlayers;

    [Header("Instantiation Info")]
    public List<spawnPositions> allSpawnPositions = new List<spawnPositions>();

    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("UniverseController").Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            //if (Input.GetButtonDown("AllAButton")) { SceneManager.LoadScene("3CharacterSelector"); numOfPlayers = 3; }
            //if (Input.GetButtonDown("AllBButton")) { SceneManager.LoadScene("4CharacterSelector"); numOfPlayers = 4; }
            if (Input.GetButtonDown("AllXButton")) { SceneManager.LoadScene("2CharacterSelector"); numOfPlayers = 2; }
            if (Input.GetButtonDown("AllYButton")) { SceneManager.LoadScene("Bio"); }
        }
        else if (SceneManager.GetActiveScene().name == "Bio")
        {
            if (Input.GetButtonDown("AllBButton")) { SceneManager.LoadScene("MainMenu"); }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 2)
        {
            charSelector1 = GameObject.FindGameObjectWithTag("P1Selector").GetComponent<CharacterSelector>();
            charSelector1.SetUniverse(this);
            charSelector2 = GameObject.FindGameObjectWithTag("P2Selector").GetComponent<CharacterSelector>();
            charSelector2.SetUniverse(this);
        }
        else if (level == 3)
        {
            charSelector1 = GameObject.FindGameObjectWithTag("P1Selector").GetComponent<CharacterSelector>();
            charSelector1.SetUniverse(this);
            charSelector2 = GameObject.FindGameObjectWithTag("P2Selector").GetComponent<CharacterSelector>();
            charSelector2.SetUniverse(this);
            charSelector3 = GameObject.FindGameObjectWithTag("P3Selector").GetComponent<CharacterSelector>();
            charSelector3.SetUniverse(this);
        }
        else if (level == 4)
        {
            charSelector1 = GameObject.FindGameObjectWithTag("P1Selector").GetComponent<CharacterSelector>();
            charSelector1.SetUniverse(this);
            charSelector2 = GameObject.FindGameObjectWithTag("P2Selector").GetComponent<CharacterSelector>();
            charSelector2.SetUniverse(this);
            charSelector3 = GameObject.FindGameObjectWithTag("P3Selector").GetComponent<CharacterSelector>();
            charSelector3.SetUniverse(this);
            charSelector4 = GameObject.FindGameObjectWithTag("P4Selector").GetComponent<CharacterSelector>();
            charSelector4.SetUniverse(this);
        }
        else if (level >= 6)
        {
            Vector3 targetScale = new Vector3(0.3f, 0.3f, 0.3f);
            Quaternion targetLook = new Quaternion(0, 0, 0, 0);

            #region Player 1
            GameObject p1 = selectedChars[1];
            p1.GetComponent<PlayerBase>().enabled = true;
            p1.GetComponent<PlayerBase>().thisPlayer = "P1";
            //p1.GetComponent<Rigidbody>().isKinematic = false;
            p1.transform.parent = null;

            GameObject parent1 = GameObject.Find("Player2Base");
            p1.transform.SetParent(parent1.transform);
            parent1.transform.position = allSpawnPositions[level - 6].spawnPos[0];
            p1.transform.localPosition = Vector3.zero;
            p1.transform.localScale = targetScale;
            p1.transform.rotation = targetLook;
            #endregion

            #region Player 2
            GameObject p2 = selectedChars[1];
            p2.GetComponent<PlayerBase>().enabled = true;
            p2.GetComponent<PlayerBase>().thisPlayer = "P2";
            //p2.GetComponent<Rigidbody>().isKinematic = false;
            p2.transform.parent = null;

            GameObject parent2 = GameObject.Find("Player2Base");
            p2.transform.SetParent(parent2.transform);
            parent2.transform.position = allSpawnPositions[level - 6].spawnPos[1];
            p2.transform.localPosition = Vector3.zero;
            p2.transform.localScale = targetScale;
            p2.transform.rotation = targetLook;
            #endregion

            #region Player 3
            if (selectedChars[2] != null)
            {
                GameObject p3 = selectedChars[2];
                p3.GetComponent<PlayerBase>().enabled = true;
                p3.GetComponent<PlayerBase>().thisPlayer = "P3";
                //p3.GetComponent<Rigidbody>().isKinematic = false;
                p3.transform.parent = null;

                GameObject parent3 = GameObject.Find("Player2Base");
                p3.transform.SetParent(parent3.transform);
                parent3.transform.position = allSpawnPositions[level - 6].spawnPos[2];
                p3.transform.localPosition = Vector3.zero;
                p3.transform.localScale = targetScale;
                p3.transform.rotation = targetLook;
            }
            #endregion

            #region Player 4
            if (selectedChars[3] != null)
            {
                GameObject p4 = selectedChars[3];
                p4.GetComponent<PlayerBase>().enabled = true;
                p4.GetComponent<PlayerBase>().thisPlayer = "P4";
                //p4.GetComponent<Rigidbody>().isKinematic = false;
                p4.transform.parent = null;

                GameObject parent4 = GameObject.Find("Player4Base");
                p4.transform.SetParent(parent4.transform);
                parent4.transform.position = allSpawnPositions[level - 6].spawnPos[3];
                p4.transform.localPosition = Vector3.zero;
                p4.transform.localScale = targetScale;
                p4.transform.rotation = targetLook;

            }
            #endregion

            Destroy(gameObject);
        }
    }

    public void CheckReady(int arrayIndex, GameObject gobject)
    {
        print(gobject.transform.localScale);

        selectedChars[arrayIndex] = gobject;
        gobject.transform.parent = gameObject.transform;
        lockedInPlayers++;

        print(gobject.transform.localScale);

        if (lockedInPlayers == numOfPlayers)
        {
            SceneManager.LoadScene("ArenaSelector");
        }
    }

    public void ChooseArena(string arena)
    {
        SceneManager.LoadScene(arena);
    }

    [Serializable] public struct spawnPositions { public List<Vector3> spawnPos; }

}