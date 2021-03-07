using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEvent : ActorManagerInterface
{
    public string eventName;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        cm = GetComponentInParent<CharactorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
