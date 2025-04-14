using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [Header("Components")]
    public Animator playerAnim;
    public Rigidbody playerRigid;
    public Transform playerTrans;

    [Header("Movement Settings")]
    public float w_speed = 5f;
    public float wb_speed = 3f;
    public float olw_speed = 5f; // original walk speed (used to reset after run)
    public float rn_speed = 3f;  // additional speed when running
    public float ro_speed = 100f;

    private bool walking = false;

    void Start()
    {
        // Initialize original walk speed if not set in Inspector
        if (olw_speed == 0) olw_speed = w_speed;
    }

    void FixedUpdate()
    {
        Vector3 velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            velocity += transform.forward * w_speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            velocity += -transform.forward * wb_speed;
        }

        playerRigid.velocity = new Vector3(velocity.x, playerRigid.velocity.y, velocity.z); // maintain vertical momentum
    }

    void Update()
    {
        HandleWalking();
        HandleRotation();
        HandleRunning();
    }

    void HandleWalking()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerAnim.SetTrigger("walk");
            playerAnim.ResetTrigger("idle");
            walking = true;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnim.ResetTrigger("walk");
            playerAnim.SetTrigger("idle");
            walking = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            playerAnim.SetTrigger("walkback");
            playerAnim.ResetTrigger("idle");
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnim.ResetTrigger("walkback");
            playerAnim.SetTrigger("idle");
        }
    }

    void HandleRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            playerTrans.Rotate(0, -ro_speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            playerTrans.Rotate(0, ro_speed * Time.deltaTime, 0);
        }
    }

    void HandleRunning()
    {
        if (walking)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                w_speed = olw_speed + rn_speed;
                playerAnim.SetTrigger("run");
                playerAnim.ResetTrigger("walk");
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                w_speed = olw_speed;
                playerAnim.ResetTrigger("run");
                playerAnim.SetTrigger("walk");
            }
        }
    }
}
