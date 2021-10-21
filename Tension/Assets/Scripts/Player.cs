using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{   //player input class
    [Header ("Options")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpForce = 200f;
    [SerializeField] private float mouseSensitivityX = 250f;
    [SerializeField] private float mouseSensitivityY = 250f;
    float verticalLookRotation;
    PlayerController playerController;
    GunController gunController;

    protected override void Start()
    {
        base.Start();
        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
    }
    private void Update()
    {
        movementInput();    //press shift for run
        weaponInput();
        mouseRotateInputX();
        mouseRotateInputY();
        jumpInput();        //press space for jump
    }

    private void weaponInput()
    {
        if (Input.GetMouseButton(0))
        {
            gunController.shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gunController.reload();
        }
    }

    private void jumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            playerController.jump(jumpForce);
        }
    }

    private void mouseRotateInputY()
    {
        verticalLookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivityY;
        playerController.rotatePlayerX(verticalLookRotation);
    }

    private void mouseRotateInputX()
    {
        Vector3 mouseInputX = Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX;
        playerController.rotatePlayerY(mouseInputX);
    }

    private void movementInput()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 velocity = moveInput.normalized;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            velocity *= runSpeed;
        }
        else
        {
            velocity *= walkSpeed;
        }
        velocity = transform.TransformDirection(velocity);
        playerController.setVelocity(velocity);
    }
}
