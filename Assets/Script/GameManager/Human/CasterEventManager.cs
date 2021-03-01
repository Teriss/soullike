using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEventManager : ActorManagerInterface
{
    public string eventName;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        am = GetComponentInParent<ActorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
