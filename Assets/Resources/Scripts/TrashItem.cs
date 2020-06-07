using UnityEngine;

public class TrashItem : MonoBehaviour {

    [SerializeField] public TrashType type;
    public bool pickedUp;
    public bool pickable;
    public bool disabled;
    public bool bounded;
    public bool byNPC;
    public bool thrown;
    public TrashImpact impact;

    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        impact = new TrashImpact(type);
        pickedUp = false;
        pickable = true;
        disabled = false;
        bounded = false;
        byNPC = false;
        thrown = false;
    }

    void Update() {
        if (pickedUp) GetComponent<TrailRenderer>().enabled = false;
        if (disabled) {
            pickable = false;
            if (rb.IsSleeping()) {
                rb.isKinematic = true;
            }
            //if (pickedUp) {
            //    GetComponent<MeshRenderer>().material.color = Color.white;
            //} else {
            //    GetComponent<MeshRenderer>().material.color = Color.black;
            //}
        }
    }
	
    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "detectionRadius") {
            if (pickedUp) Physics.IgnoreCollision(col.gameObject.GetComponent<CharacterController>(), GetComponent<Collider>());
        } else if (col.gameObject.tag == "Boundary") {
            //if (pickedUp) Physics.IgnoreCollision(col.gameObject.GetComponent<MeshCollider>(), GetComponent<Collider>());
        } else {
            if (!disabled) CameraManager.instance.Remove(transform);
            if (!byNPC && thrown && !disabled) GameManager.instance.LitterTrash(this);
            pickedUp = false;   thrown = false;
            GetComponent<TrailRenderer>().enabled = false;
        }
    }
    public bool Free() {
        return !pickedUp && pickable && !disabled;
    }
}
