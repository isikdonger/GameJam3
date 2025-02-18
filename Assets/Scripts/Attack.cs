using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float distance = 2f;
    public int hit = 0, oreStrength = 3;
    private GameObject ore;
    private bool mined = false;
    public static Dictionary<int, bool> ores = new Dictionary<int, bool>() { { 0, false }, { 1, false }, { 2, false } };

    AudioManager audioManager;
    // Start is called before the first frame update

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();    
    }
    void Start()
    {
        ore = GameObject.FindGameObjectWithTag("Ore");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Animator animator = GetComponent<Animator>();
            animator.SetBool("Attack", true);
            
        }
        else if (!Input.GetKeyDown(KeyCode.X))
        {
            Animator animator = GetComponent<Animator>();
            animator.SetBool("Attack", false);
            animator.SetTrigger("Idle");
        }
    }

    public void Mine()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Vector3.Distance(enemy.transform.position, this.transform.position) <= distance)
            {
                Destroy(enemy);
                //enemy.SetActive(false);
            }
        }
        if (!mined && Vector2.Distance(this.transform.position, ore.transform.position) <= distance)
        {
            if (hit < oreStrength)
            {
                audioManager.PlaySFX(audioManager.hittingStone);
                hit++;
                ore.transform.localScale *= 0.75f;
            }
            else
            {
                ores[CombatMovement.currentSceneIndex] = true;
                Destroy(ore);
                mined = true;
            }
        }
    }
}
