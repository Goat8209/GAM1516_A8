using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPodobooState : byte
{
    Unknown,
    Down,
    Up,
    FlipUp,
    FlipDown
}

public class Podoboo : MonoBehaviour
{
    private float timeSinceStart;
    private EPodobooState state = EPodobooState.Unknown;

    // Start is called before the first frame update
    void Start()
    {
        state = EPodobooState.Up;
        timeSinceStart = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceStart += Time.deltaTime;
        transform.position = new Vector2(transform.position.x, transform.position.y + Mathf.Sin(timeSinceStart) * Time.deltaTime * EnemyConstants.PodobooSpeed);
    
        if(state == EPodobooState.Up)
        {
            if(Mathf.Sin(timeSinceStart) < 0)
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
            if (Mathf.Sin(timeSinceStart) >= 0)
            {
                state = EPodobooState.FlipUp;
            }
        }

        else if (state == EPodobooState.FlipUp)
        {
            transform.localScale = new Vector2(1, 1);
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - 1);
            state = EPodobooState.Up;
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
