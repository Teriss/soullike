using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : ActorManagerInterface
{
    public List<CasterEvent> overlapCasterEMs = new List<CasterEvent>();

    private CapsuleCollider interCol;

    private void Start() {
        interCol = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider col) {
        CasterEvent[] casterEMs = col.GetComponents<CasterEvent>();
        foreach(var casterEM in casterEMs) {
            print(casterEM.eventName);
            if (!overlapCasterEMs.Contains(casterEM))
                overlapCasterEMs.Add(casterEM);
        }
    }

    private void OnTriggerExit(Collider col) {
        CasterEvent[] casterEMs = col.GetComponents<CasterEvent>();
        foreach (var casterEM in casterEMs) {
            if (overlapCasterEMs.Contains(casterEM))
                overlapCasterEMs.Remove(casterEM);
        }
    }

}
