using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemy1MovementScript : MonoBehaviour
{
    public float jumpForce;
    private GameObject player;
    private float enemyVelocity = 2f;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player.transform.position.x < this.transform.position.x)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * enemyVelocity * Time.deltaTime;

            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (player.transform.position.x > this.transform.position.x)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * enemyVelocity * Time.deltaTime;

            
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            Time.timeScale = 0;
            SceneManager.LoadScene("Death");

            // Start the coroutine to handle exit after the delay
            StartCoroutine(HandleDeadSceneExit());
        }
        else if (collidedObject.CompareTag("Ore"))
        {
            animator.SetBool("Jump", true);
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        else if (collidedObject.CompareTag("Ground"))
        {
            animator.SetBool("Jump", false);
            animator.SetTrigger("Land");
        }
    }
    private IEnumerator HandleDeadSceneExit()
    {
        // Wait for 5 seconds to show the DeadScene
        yield return new WaitForSecondsRealtime(0.5f);
        Application.Quit();
    }
}
