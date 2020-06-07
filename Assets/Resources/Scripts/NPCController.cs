using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;

public class NPCController : MonoBehaviour {

    // public Transform playerTarget;
    private Transform throwTarget;
    private ActionController ac;
    private NavMeshAgent agent;
    private EntityCheck ec;

    private string[] prefabDirectories;
    private Stack<GameObject> litters;
    public GameObject[] barang;

    public bool hostile = true;
    public float wanderInterval = 3;
    public float litterInterval = 5;
    public float wanderRadius = 5;
    public float minDist = 1.5f;
    [Range(0, 5)] public float fuzziness = 2f;
    public bool drawPath = false;

    private GameObject spawned;
    private float litterTimer;
    private float wanderTimer;
    private GameObject currentTarget;
    private bool litterDisposed;
    private bool hasTrashBefore;
    private float defaultSpeed;
    private float defaultAccel;
    private bool checkedPlayer;

    // Use this for initialization
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        ac = GetComponent<ActionController>();  ac.isNPC = true;
        ec = GetComponentInChildren<EntityCheck>();
        //TODO: move reference to GameManager
        prefabDirectories = new string[] { "Prefabs/TrashEntity/TrashSmall", "Prefabs/TrashEntity/TrashMedium", "Prefabs/TrashEntity/TrashLarge" };
        litters = new Stack<GameObject>();
        litterDisposed = true;
        hasTrashBefore = false;
        checkedPlayer = false;
        // playerTarget = GameManager.instance.player.targetterPos;
        throwTarget = GameManager.instance.trashBin.throwTarget;        // cached trashBin position!
        wanderInterval += Random.Range(0, fuzziness);
        litterInterval += Random.Range(0, fuzziness);
        wanderRadius += Random.Range(0, fuzziness);
        defaultSpeed = agent.speed;
        defaultAccel = agent.acceleration;
    }
    
    void Update() {
        // test purposes
        barang = litters.ToArray();

        if (hostile) {
            // NPC is hostile
            if (ec.hitPlayer) { if (!checkedPlayer) { hostile = NPCManager.instance.DeHostile(transform); checkedPlayer = true; } }      // caught by player
            else checkedPlayer = false;
            ac.allowFetch = false; agent.autoBraking = false;
            if (ac.HasTrash()) { ac.DropTrash(); agent.ResetPath(); }
            wanderTimer += Time.deltaTime;
            if (wanderTimer >= wanderInterval) {
                // change wander destination
                Vector3 destination = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(destination);
                wanderTimer = 0;
            }
            litterTimer += Time.deltaTime;
            if (litterTimer >= litterInterval) {
                // spawn new trash entity
                SpawnTrash();
                litterTimer = 0;
            }
        } else {
            // NPC not hostile
            ac.allowFetch = true; agent.autoBraking = true;
            if (spawned != null) Destroy(spawned);
            if (litters.Count == 0 && litterDisposed) {
                // no self-littered trash remaining
                litterDisposed = true; ac.manualFetch = null;
                if (ac.HasTrash()) {
                    // new trash acquired, goto trashcan
                    NPCManager.instance.RemoveFromLine(gameObject);
                    if (!hasTrashBefore) { agent.SetDestination(throwTarget.position); hasTrashBefore = true; }
                    if (Vector3.Distance(throwTarget.position, transform.position) <= ac.GetRadius()) { ac.ThrowTrash(); agent.ResetPath(); }
                } else {
                    // no trash in hand, goto player target
                    NPCManager.instance.AddToLine(gameObject);
                    hasTrashBefore = false;
                    agent.SetDestination(NPCManager.instance.GetNextTarget(gameObject).position);
                }
            } else {
                // collect self-littered trash
                if (!litterDisposed && ac.HasTrash()) {
                    // trash fetched, goto trashcan
                    if (!agent.hasPath) agent.SetDestination(throwTarget.position);
                    if (Vector3.Distance(throwTarget.position, transform.position) <= ac.GetRadius()) { ac.ThrowTrash(); litterDisposed = true; agent.ResetPath(); }
                }
                if (litterDisposed) {
                    // yield next trash item
                    GameObject trashTarget = null;
                    while (true) {
                        trashTarget = litters.Pop();
                        if (trashTarget.GetComponent<TrashItem>().Free() && trashTarget != null) break;
                    }
                    ac.manualFetch = trashTarget;
                    agent.SetDestination(trashTarget.transform.position);
                    litterDisposed = false;
                }
                if (Vector3.SqrMagnitude(agent.destination - transform.position) <= minDist && !ac.HasTrash()) {
                    // target not in location
                    litterDisposed = true;
                    agent.ResetPath();
                }
            }
        }
        agent.speed = ac.UpdateMobility(defaultSpeed);
        agent.acceleration = ac.UpdateMobility(defaultAccel);

        // draw movement vector line
        if (drawPath) DrawPath();
        Debug.DrawLine(transform.position, transform.position + agent.desiredVelocity, Color.green);
    }

    void SpawnTrash() {
        spawned = Instantiate(Resources.Load<GameObject>(prefabDirectories[Random.Range(0, 3)]), transform.position, Quaternion.identity);
        spawned.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
        spawned.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));
        litters.Push(spawned);
    }

    static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    void DrawPath() {
        if (agent.path == null) return;
        LineRenderer line = this.GetComponent<LineRenderer>();
        if (line == null) {
            line = this.gameObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default")) { color = Color.yellow };
            line.startWidth = line.endWidth = 0.05f;
            line.startColor = line.endColor = Color.yellow;
        }
        NavMeshPath path = agent.path;
        line.positionCount = path.corners.Length;
        for (int i = 0; i < path.corners.Length; i++) {
            line.SetPosition(i, path.corners[i]);
        }
    }

    public void SetHostile(bool hostile) {
        this.hostile = hostile;
    }
}
