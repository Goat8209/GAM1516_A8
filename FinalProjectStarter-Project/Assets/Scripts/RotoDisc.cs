using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RotoDisc : Enemy
{
    private float angularVelocity;
    private float rotation;
    private Vector2 anchorPosition;

    // Start is called before the first frame update
    void Start()
    {
        angularVelocity = 3.0f;
        rotation = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        rotation += angularVelocity * Time.deltaTime;
        transform.localPosition = new Vector2(anchorPosition.x + Mathf.Cos(rotation) * 3.0f, (anchorPosition.y + Mathf.Sin(rotation) * 3.0f));
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

    public void SetAnchorPosition(Vector2 position)
    {
        anchorPosition = position;
    }
}
