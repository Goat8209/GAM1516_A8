using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPodobooState : byte
{
    Unknown,
    Down,
    Up,
    FlipUp,
    FlipDown,
    Waiting
}

public class Podoboo : MonoBehaviour
{
    private float timeSinceStart;
    private float startingY;
    private EPodobooState state = EPodobooState.Unknown;
    private float popupDelay;

    // Start is called before the first frame update
    void Start()
    {
        state = EPodobooState.Up;
        timeSinceStart = 0;
        startingY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != EPodobooState.Waiting)
        {
            timeSinceStart += Time.deltaTime;
            transform.position = new Vector2(transform.position.x, transform.position.y + Mathf.Cos(timeSinceStart) * Time.deltaTime * EnemyConstants.PodobooSpeed);

            if (state == EPodobooState.Up)
            {
                if (Mathf.Cos(timeSinceStart) < 0)
                {
                    state = EPodobooState.FlipDown;
                }
            }

            else if (state == EPodobooState.FlipDown)
            {
                transform.localScale = new Vector2(1, -1);
                transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + 1);
                state = EPodobooState.Down;
            }

            else if (state == EPodobooState.Down)
            {
                if (transform.localPosition.y <= startingY)
                {
                    transform.localScale = new Vector2(1, 1);
                    transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - 1);

                    state = EPodobooState.Waiting;
                    transform.localPosition = new Vector2(transform.localPosition.x, startingY);
                    timeSinceStart = 0.0f;
                    popupDelay = Random.Range(1.5f, 3.5f);
                }
            }
        }

        else if (state == EPodobooState.Waiting)
        {
            timeSinceStart += Time.deltaTime;

            if (timeSinceStart >= popupDelay)
            {
                state = EPodobooState.Up;
                timeSinceStart = 0.0f;
            }
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
