using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDonutLiftState: byte
{
    Unknown,
    Idle,
    Active,
    Falling
}
public class DonutLift : MonoBehaviour
{
    private float timer;
    const float timerDuration = 2.0f;
    Animator animator;
    EDonutLiftState state;
    Collider2D collider;
    private Vector2 originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        state = EDonutLiftState.Idle;
        timer = 0.0f;
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EDonutLiftState.Active)
        {
            timer += Time.deltaTime;

            if (timer > timerDuration)
            {
                state = EDonutLiftState.Falling;
                collider.enabled = false;
                timer = 0.0f;
            }
        }

        else if(state == EDonutLiftState.Falling)
        {
            timer += Time.deltaTime;
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - (10.0f * Time.deltaTime));

            if (timer > 3.0f)
            {
                state = EDonutLiftState.Idle;
                transform.localPosition = originalPosition;
                timer = 0.0f;
                animator.Play("DonutLiftIdle");
                collider.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mario"))
        {
            if (state != EDonutLiftState.Falling)
            {
                state = EDonutLiftState.Active;
                animator.Play("DonutLiftActive");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mario"))
        {
            if (state != EDonutLiftState.Falling)
            {
                state = EDonutLiftState.Idle;
                animator.Play("DonutLiftIdle");
                timer = 0.0f;
            }
        }
    }
}
