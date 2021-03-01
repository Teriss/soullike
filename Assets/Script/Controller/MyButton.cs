using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton
{
    private bool currentstate = false;
    private bool laststate = false;
    private string keyValue;

    public MyButton(string keyValue) {
        this.keyValue = keyValue;
    }

    public bool IsPressing() {
        currentstate = Input.GetKey(keyValue);
        return currentstate;
    }

    public bool OnPressed(){
        currentstate = Input.GetKey(keyValue);
        if (currentstate != laststate && currentstate){
            laststate = currentstate;
            return true;
        }
        laststate = currentstate;
        return false;

    }

    public bool OnReleased (){
        currentstate = Input.GetKey(keyValue);
        if (currentstate != laststate && !currentstate)
        {
            laststate = currentstate;
            return true;
        }
        laststate = currentstate;
        return false;
    }
}
