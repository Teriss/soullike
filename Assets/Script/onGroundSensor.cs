using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onGroundSensor : MonoBehaviour
{
    public CapsuleCollider capcol;

    private Vector3 point0;
    private Vector3 point1;
    private float radius;
    public float offset = 0.3f;

    private void Awake()
    {
        radius = capcol.radius - 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IsGround()
    {
        point0 = transform.position + transform.up * (radius - offset);
        point1 = point0 + transform.up * capcol.height - 2 * transform.up * (radius - offset);

        Collider[] cols = Physics.OverlapCapsule(point0, point1, radius,LayerMask.GetMask("Ground"));
        if (cols.Length != 0)
            //SendMessageUpwards("onTheGround");
            return true;
        else
            //SendMessageUpwards("inTheAir");
            return false;
    }
}
