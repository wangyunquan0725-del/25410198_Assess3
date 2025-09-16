using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    private Vector2[] points=new Vector2[]
    {
        new Vector2(1f, -1f),
        new Vector2(6f, -1f),
        new Vector2(6f, -5f),
        new Vector2(1f, -5f)
    };
    private int target = 1;
    private Animator animator;
    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        transform.position = points[0];
        audioSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetPoint = points[target];
        Vector2 direction = (targetPoint - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(direction * Time.deltaTime * 2.5f );
        if (Mathf.Abs(direction.x) >Mathf.Abs(direction.y))
        {
            if (direction.x > 0) animator.Play("PacStudent_right");
            else animator.Play("PacStudent_left");
        }
        else
        {
            if (direction.y > 0) animator.Play("PacStudent_up");
            else animator.Play("PacStudent_down");
        }
        if (Vector2.Distance(transform.position, targetPoint) < 0.05f)
        {
            target = (target + 1) % 4;
        }
    }
}