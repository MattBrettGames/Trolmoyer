﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniverseController : BlankMono
{

    [Header("GameObjects")]
    public CharacterSelector charSelector1;
    public CharacterSelector charSelector2;

    [Header("Scenes")]
    public Scene mainMenu;
    public Scene charSelector;
    public Scene arenaSelector;
    public Scene[] arenas = new Scene[3];

    [Header("Character Info")]
    public GameObject lockedCharacter1;
    public GameObject lockedCharacter2;

    [Header("Instantiation Info")]
    public List<Vector3> spawnPosList = new List<Vector3>();

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (Input.GetButtonDown("AllAButton")) { SceneManager.LoadScene("3CharacterSelector"); }
            if (Input.GetButtonDown("AllBButton")) { SceneManager.LoadScene("4CharacterSelector"); }
            if (Input.GetButtonDown("AllXButton")) { SceneManager.LoadScene("2CharacterSelector"); }
            if (Input.GetButtonDown("AllYButton")) { SceneManager.LoadScene("Bio"); }
        }

    }


    public void CheckReady()
    {
        if (charSelector1.locked && charSelector2.locked)
        {
            lockedCharacter1 = charSelector1.displayChar;
            lockedCharacter2 = charSelector2.displayChar;
            SceneManager.LoadScene(arenaSelector.name);
        }
    }


}