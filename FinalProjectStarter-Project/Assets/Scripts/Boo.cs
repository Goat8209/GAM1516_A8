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
<<<<<<< HEAD
    float translationAngle;
=======
>>>>>>> 877700b04f85f8d5adb4cbb398c1e39e5fcd5e39
    EBooState state;
    protected Animator animator;

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        speed = 2.0f;
        translationAngle = 0.0f;
=======
        speed = 3.0f;
>>>>>>> 877700b04f85f8d5adb4cbb398c1e39e5fcd5e39
        state = EBooState.Unknown;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Mario mario = Game.Instance.GetMario;

<<<<<<< HEAD
        translationAngle = Mathf.Atan2(mario.transform.localPosition.y - transform.localPosition.y, mario.transform.localPosition.x - transform.localPosition.x);

        
        if(mario.transform.localScale.x == -1 && transform.localPosition.x > mario.transform.localPosition.x)
        {
            state = EBooState.Chasing;
            transform.localScale = new Vector2(1, transform.localScale.y);
=======
        if(mario.transform.localScale.x == -1 && transform.localPosition.x > mario.transform.localPosition.x)
        {
            state = EBooState.Chasing;
>>>>>>> 877700b04f85f8d5adb4cbb398c1e39e5fcd5e39
        }

        else if(mario.transform.localScale.x == 1 && transform.localPosition.x < mario.transform.localPosition.x)
        {
            state = EBooState.Chasing;
<<<<<<< HEAD
            transform.localScale = new Vector2(-1, transform.localScale.y);
=======
>>>>>>> 877700b04f85f8d5adb4cbb398c1e39e5fcd5e39
        }

        else
        {
            state = EBooState.Stopped;
        }

        if (state == EBooState.Chasing)
        {
<<<<<<< HEAD
            Vector2 velocity = new Vector2(Mathf.Cos(translationAngle), Mathf.Sin(translationAngle)) * speed;
            Vector2 displacement = velocity * Time.deltaTime;
            transform.position = new Vector2(transform.position.x + displacement.x, transform.position.y + displacement.y);
=======
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
>>>>>>> 877700b04f85f8d5adb4cbb398c1e39e5fcd5e39
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
