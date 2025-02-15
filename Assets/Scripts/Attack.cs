using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float distance = 2f;
    public int hit = 0, oreStrength = 3;
    private GameObject ore;
    public static Dictionary<int, bool> ores = new Dictionary<int, bool>() { { 0, true }, { 1, true }, { 2, true } };
    private int index=0;
    // Start is called before the first frame update
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
        if (Vector2.Distance(this.transform.position, ore.transform.position) <= distance)
        {
            if (hit < oreStrength)
            {
                hit++;
                ore.transform.localScale *= 0.75f;
            }
            else
            {
                ores[index] = true;
                Destroy(ore);
            }
        }
    }
}
