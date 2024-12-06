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
    float translationAngle;
    EBooState state;
    protected Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        translationAngle = 0.0f;
        state = EBooState.Unknown;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Mario mario = Game.Instance.GetMario;

        translationAngle = Mathf.Atan2(mario.transform.localPosition.y - transform.localPosition.y, mario.transform.localPosition.x - transform.localPosition.x);


        if (mario.transform.localScale.x == -1 && transform.localPosition.x > mario.transform.localPosition.x)
        {
            state = EBooState.Chasing;
            transform.localScale = new Vector2(1, transform.localScale.y);
        }

        else if (mario.transform.localScale.x == 1 && transform.localPosition.x < mario.transform.localPosition.x)
        {
            state = EBooState.Chasing;
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }

        else
        {
            state = EBooState.Stopped;
        }

        if (state == EBooState.Chasing)
        {
            Vector2 velocity = new Vector2(Mathf.Cos(translationAngle), Mathf.Sin(translationAngle)) * EnemyConstants.BooSpeed;
            Vector2 displacement = velocity * Time.deltaTime;
            transform.position = new Vector2(transform.position.x + displacement.x, transform.position.y + displacement.y);
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
