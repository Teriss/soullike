using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class StabAttackScriptClip : PlayableAsset, ITimelineClipAsset
{
    public StabAttackScriptBehaviour template = new StabAttackScriptBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<StabAttackScriptBehaviour>.Create (graph, template);
        StabAttackScriptBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
