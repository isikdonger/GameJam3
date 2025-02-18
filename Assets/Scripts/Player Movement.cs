using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private float horizontalInput;
    private float verticalInput;

    // Camera boundaries
    private float minX = -7.75f;
    private float maxX = 7.75f;
    private float minY = -3.9f;
    private float maxY = 3.9f;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Animator animator = GetComponent<Animator>();

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
            animator.SetFloat("move", -horizontalInput);
            audioManager.PlaySFX(audioManager.playerWalking);
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
            animator.SetFloat("move", horizontalInput);
            audioManager.PlaySFX(audioManager.playerWalking);

        }
        else
        {
            animator.SetFloat("move", 0);
        }
    }

    private void FixedUpdate()
    {
        // Move the player
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * moveSpeed * Time.deltaTime);

        // Clamp the player's position to stay within the camera boundaries
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}

