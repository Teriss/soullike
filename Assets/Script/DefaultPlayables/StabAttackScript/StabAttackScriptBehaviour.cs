using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class StabAttackScriptBehaviour : PlayableBehaviour
{

    private PlayableDirector pd;
    private int times = 0;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }

    public override void OnGraphStart(Playable playable) {
        pd =(PlayableDirector) playable.GetGraph().GetResolver();
        foreach(var track in pd.playableAsset.outputs) {
            if (track.streamName == "Attacker Script" || track.streamName == "Victim Script") {
                ActorManager am = pd.GetGenericBinding(track.sourceObject) as ActorManager;
                am.LockActorController(true);
            }
        }
    }

    public override void OnGraphStop(Playable playable) {
        foreach (var track in pd.playableAsset.outputs) {
            if (track.streamName == "Victim Script") {
                ActorManager am = pd.GetGenericBinding(track.sourceObject) as ActorManager;
                am.LockActorController(false);
            }
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info) {
        if (times > 0) {
            foreach (var track in pd.playableAsset.outputs) {
                if (track.streamName == "Attacker Script") {
                    ActorManager am = pd.GetGenericBinding(track.sourceObject) as ActorManager;
                    am.LockActorController(false);
                }
            }
        }
        else
            times++;

    }


}
