using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(ActionController))]
public class PlayerController : MonoBehaviour {

    private CharacterController character;
    private ActionController action;
    private LineRenderer innerRadius;
    public float moveSpeed = 30f;
    [Range(0, 1)] public float gravityScale = 0.5f;
    [Range(0, 1)] public float rotationScale = 0.25f;
    public int lineResolution = 30;
    public Transform targetterPos;

    private Vector3 moveDirection;
    private Vector2 targetDirection;

    void Start() {
        character = GetComponent<CharacterController>();
        action = GetComponent<ActionController>();  action.isNPC = false;
        innerRadius = GetComponentInChildren<LineRenderer>();
    }

    void Update() {
        // calculate move direction
        float horizontalMove = CrossPlatformInputManager.GetAxis("Horizontal");
        float verticalMove = CrossPlatformInputManager.GetAxis("Vertical");
        float newSpeed = action.UpdateMobility(moveSpeed);
        moveDirection = new Vector3(horizontalMove * newSpeed, moveDirection.y, verticalMove * newSpeed);

        // calculate target point position
        float horizontalTarget = CrossPlatformInputManager.GetAxis("Horizontal2");
        float verticalTarget = CrossPlatformInputManager.GetAxis("Vertical2");
        action.Target(horizontalTarget, verticalTarget);
        targetDirection = new Vector2(horizontalTarget, verticalTarget);

        // preserve gravity impact
         moveDirection.y = moveDirection.y + Physics.gravity.y * gravityScale;

        // action button pressed
        if (CrossPlatformInputManager.GetButtonUp("Fire1")) action.ThrowTrash();

        // draw movement vector line
        Debug.DrawLine(transform.position, transform.position + new Vector3(moveDirection.x, 0, moveDirection.z), Color.blue);
    }

    void FixedUpdate() {
        // move player
        character.Move(Vector3.Lerp(character.velocity, moveDirection, (moveDirection.x == 0 && moveDirection.z == 0) ? 0.2f : 0.6f) * Time.fixedDeltaTime);
        if (targetDirection != Vector2.zero) {
            // targetter rotation
            if (action.HasTrash()) transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.y)), transform.rotation, rotationScale);
            else transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.y)), transform.rotation, rotationScale);
        } else if (moveDirection.x != 0 || moveDirection.z != 0) {
            // movement rotation
            transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z)), transform.rotation, rotationScale);
        }
    }
}
