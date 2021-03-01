using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : ActorManagerInterface
{
    public List<CasterEventManager> overlapCasterEMs = new List<CasterEventManager>();

    private CapsuleCollider interCol;

    private void Start() {
        interCol = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider col) {
        CasterEventManager[] casterEMs = col.GetComponents<CasterEventManager>();
        foreach(var casterEM in casterEMs) {
            if (!overlapCasterEMs.Contains(casterEM))
                overlapCasterEMs.Add(casterEM);
        }
    }

    private void OnTriggerExit(Collider col) {
        CasterEventManager[] casterEMs = col.GetComponents<CasterEventManager>();
        foreach (var casterEM in casterEMs) {
            if (overlapCasterEMs.Contains(casterEM))
                overlapCasterEMs.Remove(casterEM);
        }
    }

}
