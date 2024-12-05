using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBooState : byte
{
    Unknown,
    Chasing,
    Stopped
}

public class Boo : Enemy
{
    float speed;
    EBooState state;
    protected Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        speed = 3.0f;
        state = EBooState.Unknown;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Mario mario = Game.Instance.GetMario;

        if(mario.transform.localScale.x == -1 && transform.localPosition.x > mario.transform.localPosition.x)
        {
            state = EBooState.Chasing;
        }

        else if(mario.transform.localScale.x == 1 && transform.localPosition.x < mario.transform.localPosition.x)
        {
            state = EBooState.Chasing;
        }

        else
        {
            state = EBooState.Stopped;
        }

        if (state == EBooState.Chasing)
        {
            if (mario.transform.localPosition.x > transform.localPosition.x)
            {
                transform.localPosition = new Vector2(transform.localPosition.x + (speed * Time.deltaTime), transform.localPosition.y);
            }

            else
            {
                transform.localPosition = new Vector2(transform.localPosition.x - (speed * Time.deltaTime), transform.localPosition.y);
            }

            if (mario.transform.localPosition.y > transform.localPosition.y)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + (speed * Time.deltaTime));
            }

            else
            {
                transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - (speed * Time.deltaTime));
            }
        }

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if (state == EBooState.Chasing)
        {
            animator.Play("BooChasing");
        }
        else if (state == EBooState.Stopped)
        {
            animator.Play("BooStopped");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mario"))
        {
            // Get the Mario component from the GameObject
            Mario mario = collision.gameObject.GetComponent<Mario>();
            mario.HandleDamage();
        }
    }
}
