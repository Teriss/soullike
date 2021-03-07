using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ItemInteractionPlayableBehaviour : PlayableBehaviour
{
    public PlayerManager actorManager;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info) {
        if(actorManager != null)
            actorManager.LockActorController(true);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info) {
        if (actorManager != null)
            actorManager.LockActorController(false);
    }
}
