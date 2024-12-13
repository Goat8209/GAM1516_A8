using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum EBoomboomState : byte
{
    Unknown,
    Hiding,
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
    public BoxCollider2D frontTrigger;
    public BoxCollider2D backTrigger;
    public EBoomboomDirection initialDirection = EBoomboomDirection.Random;

    private new Rigidbody2D rigidbody;
    protected Animator animator;

    private EBoomboomState state = EBoomboomState.Unknown;
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

        // Set the state to walking
        SetState(EBoomboomState.Walking);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EBoomboomState.Hiding)
        {
            hidingDuration -= Time.deltaTime;

            if (hidingDuration <= 0.0f)
            {
                SetState(EBoomboomState.Walking);
            }
            else if (hidingDuration <= EnemyConstants.BoomboomHidingDuration)
            {
                UpdateAnimator();
            }
        }
        else if (state == EBoomboomState.Walking)
        {
            directionTimer -= Time.deltaTime;
            jumpTimer -= Time.deltaTime;

            if(jumpTimer <= 0.0f)
            {
                SetState(EBoomboomState.Jumping); 
            }
            if (directionTimer <= 0.0f)
            {
                velocity.x = velocity.x * -1;

                directionTimer = UnityEngine.Random.Range(EnemyConstants.BoomboomDirectionTimerMin, EnemyConstants.BoomboomDirectionTimerMax);
            }

            transform.position += new Vector3(velocity.x, velocity.y, 0.0f) * EnemyConstants.BoomboomSpeed * Time.deltaTime;
        }
    }

    public void SetState(EBoomboomState newState)
    {
        if (state != newState)
        {
            state = newState;

            if (state == EBoomboomState.Hiding)
            {
                hidingDuration = EnemyConstants.BoomboomHidingDuration;
            }
            else if (state == EBoomboomState.Walking)
            {
                if (directionTimer <= 0.0f)
                {
                    directionTimer = UnityEngine.Random.Range(EnemyConstants.BoomboomDirectionTimerMin, EnemyConstants.BoomboomDirectionTimerMax);
                }
                rigidbody.bodyType = RigidbodyType2D.Dynamic;
                primaryCollider.isTrigger = false;*
            }
            else if (state == EBoomboomState.Stunned)
            {
                rigidbody.bodyType = RigidbodyType2D.Static;
                primaryCollider.isTrigger = true;
            }

            UpdateAnimator();
        }
    }

    public void UpdateAnimator()
    {
        if (state == EBoomboomState.Hiding)
        {
            animator.Play("BoomboomHiding");

            if (hidingDuration <= EnemyConstants.BoomboomHidingDuration)
            {
                animator.Play("BoomboomComingOut");
            }
        }
    }
}
