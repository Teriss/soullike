using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ItemInteractionPlayableClip : PlayableAsset, ITimelineClipAsset
{
    public ItemInteractionPlayableBehaviour template = new ItemInteractionPlayableBehaviour ();
    public ExposedReference<ItemManager> itemManager;
    public ExposedReference<ActorManager> actorManager;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ItemInteractionPlayableBehaviour>.Create (graph, template);
        ItemInteractionPlayableBehaviour clone = playable.GetBehaviour ();
        clone.itemManager = itemManager.Resolve (graph.GetResolver ());
        clone.actorManager = actorManager.Resolve(graph.GetResolver());
        return playable;
    }
}
