using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundedMask;
    private Vector3 velocity;
    bool grounded;
    float playerLength;
    Rigidbody playerRb;
    Transform cameraTransform;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        playerLength = this.gameObject.transform.localScale.y;
    }

    private void Update()
    {
        checkGrounded();
    }

    private void FixedUpdate()
    {
        move();
    }

    public void jump(float jumpForce) {
        if (grounded)
        {
            playerRb.AddForce(transform.up * jumpForce);
        }
    }

    //check ground for jump
    private void checkGrounded() {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, playerLength+.1f, groundedMask))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    public void setVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    public void rotatePlayerY(Vector3 mouseInputX) {
        transform.Rotate(mouseInputX * Time.deltaTime);
    }

    public void rotatePlayerX(float verticalLookRotation) {
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    private void move()
    {
        playerRb.MovePosition(playerRb.position + velocity * Time.fixedDeltaTime);
    }
}
