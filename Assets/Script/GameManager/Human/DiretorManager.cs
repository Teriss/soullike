using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class DiretorManager :ActorManagerInterface
{
    PlayableDirector pd;

    [Header("Timeline Setting")]
    private TimelineAsset fronStab;
    private TimelineAsset openBox;


    private void Start() {
        fronStab = Resources.Load("Timeline/stabTimeline") as TimelineAsset;
        openBox = Resources.Load("Timeline/openBoxTimeline") as TimelineAsset;
        pd = GetComponent<PlayableDirector>();
        pd.playOnAwake = false;
    }


    public void FrontStun(ActorManager attacker, ActorManager victim) {
        if (pd.state == PlayState.Playing)
            return;
        //set timeline
        pd.playableAsset = Instantiate(fronStab);
        //set actor
        TimelineAsset timeline = (TimelineAsset)pd.playableAsset;
        foreach (var track in timeline.GetOutputTracks()) {
            if (track.name == "Attacker track")
                pd.SetGenericBinding(track, attacker.ac.GetAnimator());
            else if (track.name == "Victim track")
                pd.SetGenericBinding(track, victim.ac.GetAnimator());
            else if (track.name == "Attacker Script")
                pd.SetGenericBinding(track, attacker);
            else if (track.name == "Victim Script")
                pd.SetGenericBinding(track, victim);
        }
        //set postion
        attacker.ac.model.transform.forward = -victim.ac.model.transform.forward;
        attacker.ac.transform.position = victim.ac.transform.position - attacker.ac.model.transform.forward;

        pd.Play();
    }

    public void OpenBox(ActorManager player, ItemManager box) {
        if (pd.state == PlayState.Playing)
            return;
        //set timeline
        pd.playableAsset = Instantiate(openBox);
        //set actor
        TimelineAsset timeline = (TimelineAsset)pd.playableAsset;
        foreach (var track in timeline.GetOutputTracks()) {
            if (track.name == "Player")
                pd.SetGenericBinding(track, player.ac.GetAnimator());
            else if (track.name == "Box")
                pd.SetGenericBinding(track, box.ic.GetAnimator());
            else if (track.name == "Player Script") { 
                pd.SetGenericBinding(track, player);
                foreach(var clip in track.GetClips()) {
                    ItemInteractionPlayableClip myclip = (ItemInteractionPlayableClip)clip.asset;
                    ItemInteractionPlayableBehaviour mybehav = myclip.template;
                    myclip.actorManager.exposedName = System.Guid.NewGuid().ToString();
                    pd.SetReferenceValue(myclip.actorManager.exposedName, player);
                }
            }
            else if (track.name == "Box Script") {
                pd.SetGenericBinding(track, box);
                foreach (var clip in track.GetClips()) {
                    ItemInteractionPlayableClip myclip = (ItemInteractionPlayableClip)clip.asset;
                    ItemInteractionPlayableBehaviour mybehav = myclip.template;
                    myclip.itemManager.exposedName = System.Guid.NewGuid().ToString();
                    pd.SetReferenceValue(myclip.itemManager.exposedName, box);
                }
            }
        }

        //set postion
        player.ac.model.transform.forward = box.ic.model.transform.forward;
        player.ac.transform.position = box.ic.transform.position - player.ac.model.transform.forward * 1.2f;
        pd.Evaluate();
        pd.Play();
    }

}