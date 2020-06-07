using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour {

    public static NPCManager instance = null;

    public NPCControllerThird[] npcs;
    public LinkedList<GameObject> followingNPCs;
    public float charismaMargin;

    private GameObject attachedNPC;

    void Awake() {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        InitNPCs();
    }

    public void InitNPCs() {
        npcs = null;
        followingNPCs = new LinkedList<GameObject>();
        attachedNPC = null;
        charismaMargin = GamePoint.Charisma.marginInitial;
    }

    void Update() {
        if (attachedNPC != null) {
            if (attachedNPC.GetComponent<ActionController>().HasTrash()) {
                ReleaseNPC();
                AttachNPC();
            }
        }
    }

    public bool DeHostile(Transform location) {
        float random = Random.value;
        if (GameManager.instance.charisma >= charismaMargin) {
            if (random > GamePoint.Charisma.fixedChance) {
                // NPC neutralized
                GameManager.instance.NeutralizeNPC(true, charismaMargin, random, location);
                charismaMargin *= GamePoint.Charisma.marginStep;
                return false;
            }
        }
        //NPC not neutralized
        GameManager.instance.NeutralizeNPC(false, charismaMargin, random, location);
        return true;
    }

    public Transform GetNextTarget(GameObject current) {
        if (current == attachedNPC) {
            current.GetComponent<NPCControllerThird>().queueing = false;
            return GameManager.instance.player.targetterPos;
        }
        current.GetComponent<NPCControllerThird>().queueing = true;
        LinkedListNode<GameObject> target = followingNPCs.Find(current);
        return target.Next == null ? GameManager.instance.player.transform : target.Next.Value.transform;
    }

    public void AttachNPC() {
        if (followingNPCs.Count == 0) return;
        attachedNPC = followingNPCs.Last.Value;
        followingNPCs.RemoveLast();
    }

    public void ReleaseNPC() {
        attachedNPC = null;
    }

    public void AddToLine(GameObject npc) {
        if (!followingNPCs.Contains(npc) && attachedNPC != npc) followingNPCs.AddFirst(npc);
    }

    public void RemoveFromLine(GameObject npc) {
        if (followingNPCs.Contains(npc)) followingNPCs.Remove(npc);
    }

    public void AttachNPCs(NPCControllerThird[] npcs) {
        this.npcs = npcs;
    }

    public void SetAllHostile(bool hostile) {
        foreach (NPCControllerThird npc in npcs) npc.hostile = hostile;
    }

    public bool AllDehostiled() {
        foreach (NPCControllerThird npc in npcs) {
            if (npc.hostile) return false;
        }
        return true;
    }
}
