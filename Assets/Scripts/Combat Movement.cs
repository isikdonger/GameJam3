using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEngine.U2D;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Tilemaps;

public class CombatMovement : MonoBehaviour
{
    public float moveSpeed;
    private float horizontalInput;
    public float jumpForce;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;
    private GameObject ground;
    private BoxCollider2D box;

    // Camera boundaries
    private float minX = -7.75f;
    private float maxX = 7.75f;
    private float minY = -3.9f;
    private float maxY = 3.9f;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
            animator.SetFloat("speed", -horizontalInput);
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
            animator.SetFloat("speed", horizontalInput);
        }
        else
        {
            animator.SetFloat("speed", 0);
        }
        // Check if the player is on the ground
        if (box.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("Jump", true);
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
            else if (!Input.GetKey(KeyCode.Space)) // Check if Space is not pressed
            {
                animator.SetTrigger("Idle");
                animator.SetBool("Jump", false); // Reset the Jump parameter
            }
        }
        else
        {
            Rigidbody2D rg = GetComponent<Rigidbody2D>();
            animator.SetFloat("vertical", rg.velocity.y);
        }
        // Jump when pressing Space and the player is grounded

    }

    private void FixedUpdate()
    {
        // Move the player
        transform.Translate(new Vector3(horizontalInput, 0, 0) * moveSpeed * Time.deltaTime);

        // Clamp the player's position to stay within the camera boundaries
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}