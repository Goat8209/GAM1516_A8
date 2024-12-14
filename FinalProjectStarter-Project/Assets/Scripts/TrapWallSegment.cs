using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapWallSegment : MonoBehaviour
{
    private float fallingDistance;
    Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.y < fallingDistance)
        {
            collider.enabled = true;
        }
    }

    public void SetFallingDistance(float distance)
    {
        fallingDistance = transform.localPosition.y - distance;
    }
}
