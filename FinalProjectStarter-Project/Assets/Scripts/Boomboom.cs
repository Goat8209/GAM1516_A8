using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum EBoomboomState : byte
{
    Unknown,
    Hiding,
    HidingAfterStun,
    Walking,
    Stunned,
    Jumping,
    Dead,
}

public enum EBoomboomDamageState : byte
{
    Unknown,
    Unharmed,
    SingleHit,
    DoubleHit,
    Lethal
}

public enum EBoomboomDirection : byte
{
    Random,
    Left,
    Right
}

public class Boomboom : Enemy
{
    public BoxCollider2D primaryCollider;

    public EBoomboomDirection initialDirection = EBoomboomDirection.Random;

    private new Rigidbody2D rigidbody;
    protected Animator animator;

    private EBoomboomState state = EBoomboomState.Unknown;
    private EBoomboomDamageState damageState = EBoomboomDamageState.Unknown;
    private float squishedDuration = 0.0f;
    private float deadDuration = 0.0f;
    private float hidingDuration = 0.0f;
    private float jumpTimer = 0.0f;
    private float directionTimer = 0.0f;
    private Vector2 velocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Set the enemy type
        enemyType = EEnemyType.Boomboom;
        damageState = EBoomboomDamageState.Unharmed;

        ApplyInitialVelocity();

        // Set the state to walking
        SetState(EBoomboomState.Hiding);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EBoomboomState.Hiding)
        {
            if (Mathf.Sqrt(Mathf.Pow((Game.Instance.GetMario.transform.position.x - transform.position.x), 2) + Mathf.Pow((Game.Instance.GetMario.transform.position.y - transform.position.y), 2)) < EnemyConstants.BoomboomActivationDistance)
            {
                SetState(EBoomboomState.Walking);
            }
        }
        else if (state == EBoomboomState.Walking)
        {
            directionTimer -= Time.deltaTime;
            jumpTimer -= Time.deltaTime;

            if (jumpTimer <= 0.0f)
            {
                SetState(EBoomboomState.Jumping);
            }
            if (directionTimer <= 0.0f)
            {
                velocity.x = velocity.x * -1;

                directionTimer = UnityEngine.Random.Range(EnemyConstants.BoomboomDirectionTimerMin, EnemyConstants.BoomboomDirectionTimerMax);
            }

            if (damageState == EBoomboomDamageState.DoubleHit)
            {
                transform.localPosition += new Vector3(velocity.x, velocity.y, 0.0f) * 2 * Time.deltaTime;
            }
            else
            {
                transform.localPosition += new Vector3(velocity.x, velocity.y, 0.0f) * Time.deltaTime;
            }
        }
        else if (state == EBoomboomState.Jumping)
        {
            jumpTimer -= Time.deltaTime;

            if (jumpTimer <= 0.0f)
            {
                GetComponent<Rigidbody2D>().AddForce(transform.up * EnemyConstants.BoomboomJumpForce);
                SetState(EBoomboomState.Walking);
            }
        }
        else if (state == EBoomboomState.Stunned)
        {
            squishedDuration -= Time.deltaTime;

            if (squishedDuration <= 0.0f)
            {
                SetState(EBoomboomState.HidingAfterStun);
            }
        }
        else if (state == EBoomboomState.HidingAfterStun)
        {
            hidingDuration -= Time.deltaTime;

            if (hidingDuration <= 0.0f)
            {
                SetState(EBoomboomState.Walking);
            }
        }
        else if (state == EBoomboomState.Dead)
        {
            deadDuration -= Time.deltaTime;

            if (deadDuration <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ApplyInitialVelocity()
    {
        if (initialDirection == EBoomboomDirection.Random)
        {
            int index = UnityEngine.Random.Range(0, 10) % 2;
            float[] speeds = { EnemyConstants.BoomboomSpeed, -EnemyConstants.BoomboomSpeed };
            velocity.x = speeds[index];
        }
        else if (initialDirection == EBoomboomDirection.Right)
        {
            velocity.x = EnemyConstants.BoomboomSpeed;
        }
        else if (initialDirection == EBoomboomDirection.Left)
        {
            velocity.x = -EnemyConstants.BoomboomSpeed;
        }
    }

    public void SetState(EBoomboomState newState)
    {
        if (state != newState)
        {
            state = newState;

            if (state == EBoomboomState.Hiding)
            {
            }
            else if (state == EBoomboomState.Walking)
            {
                jumpTimer = EnemyConstants.BoomboomJumpTimer * 2;

                if (directionTimer <= 0.0f)
                {
                    directionTimer = UnityEngine.Random.Range(EnemyConstants.BoomboomDirectionTimerMin, EnemyConstants.BoomboomDirectionTimerMax);
                }
            }
            else if (state == EBoomboomState.Jumping)
            {
                jumpTimer = EnemyConstants.BoomboomJumpTimer;
            }
            else if (state == EBoomboomState.Stunned)
            {
                squishedDuration = EnemyConstants.BoomboomStunnedDuration;
            }
            else if (state == EBoomboomState.HidingAfterStun)
            {
                hidingDuration = EnemyConstants.BoomboomHidingDuration;
            }
            else if (state == EBoomboomState.Dead)
            {
                deadDuration = EnemyConstants.BoomboomDeadDuration;
            }

            UpdateAnimator();
        }
    }

    public void UpdateAnimator()
    {
        if (state == EBoomboomState.Hiding)
        {
            animator.Play("BoomboomHiding");
        }
        else if (state == EBoomboomState.Walking)
        {
            animator.Play("BoomboomRunning");
        }
        else if (state == EBoomboomState.Stunned)
        {
            animator.Play("BoomboomStunned");
        }
        else if (state == EBoomboomState.HidingAfterStun)
        {
            animator.Play("BoomboomHiding");
        }
        else if (state == EBoomboomState.Dead)
        {
            animator.Play("BoomboomDead");
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
                if (state == EBoomboomState.Walking || state == EBoomboomState.Jumping)
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

                        damageState++;
                        if (damageState == EBoomboomDamageState.Lethal)
                        {
                            SetState(EBoomboomState.Dead);
                        }
                        else
                        {
                            SetState(EBoomboomState.Stunned);
                        }
                    }
                }
            }
        }
        else if (collision.gameObject.CompareTag("World"))
        {
            if (collision.contacts.Length > 0)
            {
                // Get the normal from the first contact object
                Vector2 normal = collision.contacts[0].normal;

                if (state == EBoomboomState.Walking || state == EBoomboomState.Jumping)
                {
                    if (normal.x <= -0.8f)
                    {
                        velocity.x = -EnemyConstants.BoomboomSpeed;
                    }
                    else if (normal.x >= 0.8f)
                    {
                        velocity.x = EnemyConstants.BoomboomSpeed;
                    }
                }
            }
        }
    }
}