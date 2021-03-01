using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TranformHelper
{
    public static Transform DFSforWeapon(this Transform parent, string targerName) {

        Transform temp = null;
        foreach(Transform child in parent) {
            if (child.name == targerName)
                return child;
            temp = DFSforWeapon(child, targerName);
            if (temp != null)
                return temp;
        }
        return temp;
    }
}
