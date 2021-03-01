using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemcontroller : MonoBehaviour {

    public enum itemType {Box,Lever };

    public GameObject model;
    private Animator anim;


    private void Awake() {
        anim = model.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update() {

    }

    private void FixedUpdate() {

    }

    public Animator GetAnimator() {
        return anim;
    }

  
}
