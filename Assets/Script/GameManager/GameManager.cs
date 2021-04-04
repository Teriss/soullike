using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerHandller;

    private static GameManager instance;

    private GameDatabsase weaponDB;
    private WeaponFactory weaponFactory;
    private WeaponManager weaponManager;
    // Start is called before the first frame update
    void Awake()
    {
        CheckGameObject();
        CheckSingle();

    }

    private void Start() {
        weaponDB = new GameDatabsase();
        weaponFactory = new WeaponFactory(weaponDB);

        weaponManager = playerHandller.GetComponent<PlayerManager>().wm;

        weaponFactory.CreateWeapon("WPA0280", "R",weaponManager);

    }

    //private void OnGUI() {
    //    if(GUI.Button(new Rect (10,10,150,30),"weapon1"))
    //        weaponFactory.CreateWeapon("WPA0280", "R", weaponManager);
    //    if (GUI.Button(new Rect(10, 50, 150, 30), "weapon2"))
    //        weaponFactory.CreateWeapon("WPA0201", "R", weaponManager);
    //    if (GUI.Button(new Rect(10, 90, 150, 30), "weapon3"))
    //        weaponFactory.CreateWeapon("WPA0603", "R", weaponManager);
    //}

    private void CheckSingle() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
            Destroy(this);
    }


    private void CheckGameObject() {
        if (tag == "GM")
            return;
        Destroy(this);
    }
}
