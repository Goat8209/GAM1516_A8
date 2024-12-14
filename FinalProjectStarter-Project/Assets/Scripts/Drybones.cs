using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDrybonesState : byte
{
    Unknown,
    Walking,
    Stunned,
    Dead,
    Reviving
}

public enum EDrybonesInitialDirection : byte
{
    Random,
    Left,
    Right
}


public class Drybones : Enemy
{
    public BoxCollider2D primaryCollider;
    public BoxCollider2D frontTrigger;
    public BoxCollider2D backTrigger;
    public EDrybonesInitialDirection initialDirection = EDrybonesInitialDirection.Random;

    private new Rigidbody2D rigidbody;
    protected Animator animator;

    private EDrybonesState state = EDrybonesState.Unknown;
    private float stunnedDuration = 0.0f;
    private float deadDuration = 0.0f;
    private float revivingDuration = 0.0f;
    private Vector2 velocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody and animator components
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        ApplyInitialVelocity();

        // Set the enemy type
        enemyType = EEnemyType.Drybones;

        // Set the state to walking
        SetState(EDrybonesState.Walking);
    }

    // Update is called once per frame
    void Update()
    {
        if(velocity.x<0.0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1.0f;
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = 1.0f;
            transform.localScale = scale;
        }

        if (state == EDrybonesState.Stunned)
        {
            if (stunnedDuration > 0.0f)
            {
                stunnedDuration -= Time.deltaTime;

                if (stunnedDuration <= 0.0f)
                {
                    SetState(EDrybonesState.Dead);
                }
            }
        }
        else if (state == EDrybonesState.Dead)
        {
            deadDuration -= Time.captureDeltaTime;

            if (deadDuration <= 0.0f)
            {
                SetState(EDrybonesState.Reviving);
            }
        }
        else if (state == EDrybonesState.Reviving)
        {
            revivingDuration -= Time.captureDeltaTime;

            if (revivingDuration <= 0.0f)
            {
                SetState(EDrybonesState.Walking);
            }
        }
    }

    private void FixedUpdate()
    {
        if (state == EDrybonesState.Walking)
        {
            Vector2 location = rigidbody.position;
            location += velocity * Time.deltaTime * Game.Instance.LocalTimeScale;
            rigidbody.position = location;
        }
    }

    public void SetState(EDrybonesState newState)
    {
        if (state != newState)
        {
            state = newState;

            if (state == EDrybonesState.Walking)
d            {
                rigidbody.bodyType = RigidbodyType2D.Dynamic;
                primaryCollider.isTrigger = false;
            }
            else if (state == EDrybonesState.Stunned)
            {
                stunnedDuration = EnemyConstants.GoombaSquishedDuration;
                rigidbody.bodyType = RigidbodyType2D.Static;
                primaryCollider.isTrigger = true;
            }
            else if (state == EDrybonesState.Dead)
            {
                deadDuration = EnemyConstants.DrybonesDeadDuration;
                rigidbody.bodyType = RigidbodyType2D.Static;
                primaryCollider.isTrigger = true;
            }
            else if (state == EDrybonesState.Reviving)
            {
                revivingDuration = EnemyConstants.DrybonesRevivingDuration;
                rigidbody.bodyType = RigidbodyType2D.Static;
                primaryCollider.isTrigger = true;
            }

            UpdateAnimator();
        }
    }

    private void UpdateAnimator()
    {
        if (state == EDrybonesState.Walking)
        {
            animator.Play("GoombaWalk");
        }
        else if (state == EDrybonesState.Stunned)
        {
            animator.Play("GoombaSquished");
        }
        else if (state == EDrybonesState.Dead)
        {
            animator.Play("DrybonesDead");
        }
        else if (state == EDrybonesState.Reviving)
        {
            animator.Play("DrybonesReviving");
        }
    }

    private void ApplyInitialVelocity()
    {
        if (initialDirection == EDrybonesInitialDirection.Random)
        {
            int index = UnityEngine.Random.Range(0, 10) % 2;
            float[] speeds = { EnemyConstants.GoombaSpeed, -EnemyConstants.GoombaSpeed };
            velocity.x = speeds[index];
        }
        else if (initialDirection == EDrybonesInitialDirection.Right)
        {
            velocity.x = EnemyConstants.GoombaSpeed;
        }
        else if (initialDirection == EDrybonesInitialDirection.Left)
        {
            velocity.x = -EnemyConstants.GoombaSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Is the Goomba colliding with the Mario GameObject?
        if (collision.gameObject.CompareTag("Mario"))
        {
            // Get the Mario component from the GameObject
            Mario mario = collision.gameObject.GetComponent<Mario>();

            // Check if there's a contact object, in the contacts array
            if (collision.contacts.Length > 0)
            {
                // Get the normal from the first contact object
                Vector2 normal = collision.contacts[0].normal;

                // Ensure the Goomba's state is walking
                if (state == EDrybonesState.Walking)
                {
                    if (normal.x <= -0.8f || normal.x >= 0.8f)
                    {
                        // Goomba collided with Mario on the side
                        mario.HandleDamage();
                    }
                    else if (normal.y >= 0.7f)
                    {
                        // Goomba landed on Mario
                        mario.HandleDamage();
                    }
                    else if (normal.y <= -0.7f)
                    {
                        // Mario landed on the Goomba, make Mario bounce off the enemy
                        MarioMovement marioMovement = collision.gameObject.GetComponent<MarioMovement>();
                        marioMovement.ApplyBounce();

                        SetState(EDrybonesState.Stunned);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool flipMovementDirection = false;

        if (other.gameObject.CompareTag("World"))
        {
            // The Goomba collided with the World
            flipMovementDirection = true;
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            // The Goomba collided with another Enemy
            EEnemyType enemyType = other.gameObject.GetComponent<Enemy>().EnemyType;

            if (enemyType == EEnemyType.Goomba)
            {
                flipMovementDirection = true;
            }
        }
        else if (other.gameObject.CompareTag("Pickup"))
        {
            flipMovementDirection = true;
        }

        // DO we need to flip the Goomba's direction
        if (flipMovementDirection)
        {
            // Get the Goomba's overlapping collider
            ContactFilter2D filter = new ContactFilter2D().NoFilter();
            List<Collider2D> results = new List<Collider2D>();
            other.OverlapCollider(filter, results);

            // Set the Goomba's velocity based on which collider was triggered 
            if (results.Contains(frontTrigger))
            {
                velocity.x = -EnemyConstants.GoombaSpeed;
            }
            else if (results.Contains(backTrigger))
            {
                velocity.x = EnemyConstants.GoombaSpeed;
            }
        }
    }
}
