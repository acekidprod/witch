using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float walkSpeed = 5;
    public float runSpeed = 10;
    public float gravity = -12;

    public float turnSmoothTime = 0.08f;
    float turnSmoothVelocity;
    float velocityY;

    Animator animator;
    Transform cameraT;
    CharacterController controller;

	void Start () {
        animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();
	}

	void Update () {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        bool running = Input.GetKey(KeyCode.LeftShift);
        float speed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * speed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
        speed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded)
        {
            velocityY = 0;
        }

        float animationSpeedPercent = ((running) ? speed / runSpeed : speed / walkSpeed * .5f);
        animator.SetFloat("speedPercent", animationSpeedPercent);
	}
}
