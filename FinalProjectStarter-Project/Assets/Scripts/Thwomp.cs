using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EThwompState
{
    Unknown = 0,
    Up,
    AnimatingUp,
    Down,
    AnimatingDown,
}
public class Thwomp : Enemy
{
    private EThwompState state = EThwompState.Unknown;
    private Vector2 upLocation = Vector2.zero;
    private float holdTimer = 0.0f;

    public EThwompState State
    {
        get { return state; }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyType = EEnemyType.Thwomp;
        upLocation = transform.position;

        SetState(EThwompState.Up);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EThwompState.Up)
        {
            if (Game.Instance.GetMario.transform.position.x >= transform.position.x - GetComponent<BoxCollider2D>().size.x && Game.Instance.GetMario.transform.position.x <= transform.position.x + GetComponent<BoxCollider2D>().size.x && Game.Instance.GetMario.transform.position.y < transform.position.y)
            {
                SetState(EThwompState.AnimatingDown);
            }
        }
        else if (state == EThwompState.AnimatingUp)
        {
            transform.localPosition += new Vector3(0.0f, EnemyConstants.ThwompRisingSpeed * Time.deltaTime, 0.0f);

            if(transform.position.y >= upLocation.y)
            {
                transform.position = upLocation;
                SetState(EThwompState.Up);
            }
        }
        else if (state == EThwompState.Down)
        {
            holdTimer -= Time.deltaTime;

            if (holdTimer < 0.0f)
            {
                SetState(EThwompState.AnimatingUp);
            }
        }
        else if (state == EThwompState.AnimatingDown)
        {
            transform.localPosition -= new Vector3(0.0f, EnemyConstants.ThwompFallingSpeed * Time.deltaTime, 0.0f);
        
/*            //Hard coded stopping point
            if(transform.localPosition.y <= upLocation.y -5)
            {
                SetState(EThwompState.Down);
            }*/
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("World"))
        {
            if (state == EThwompState.AnimatingDown)
            {
                SetState(EThwompState.Down);
            }
        }
        else if(other.gameObject.CompareTag("Mario") && state == EThwompState.AnimatingDown)
        {
            Game.Instance.GetMario.HandleDamage();
        }
    }

    private void SetState(EThwompState newState)
    {
        if (state != newState)
        {
            state = newState;

            if (state == EThwompState.Up)
            {
                transform.position = upLocation;
            }
            else if (state == EThwompState.AnimatingUp)
            {

            }
            else if (state == EThwompState.Down)
            {
                holdTimer = EnemyConstants.ThwompHoldTimer;
            }
            else if (state == EThwompState.AnimatingDown)
            {

            }
        }
    }
}