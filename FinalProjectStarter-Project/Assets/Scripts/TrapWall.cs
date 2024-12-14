using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapWall : MonoBehaviour
{
    public TrapWallSegment trapWallSegmentPrefab;
    private TrapWallSegment trapWallSegment = null;
    private bool hasBeenTriggered;

    // Start is called before the first frame update
    void Start()
    {
        hasBeenTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasBeenTriggered)
        {
            if (collision.gameObject.CompareTag("Mario"))
            {
                for (int i = 0; i < 4; i++)
                {
                    trapWallSegment = Instantiate(trapWallSegmentPrefab, new Vector2(transform.position.x - 4.0f, transform.position.y + (5.0f + i * 1.1f)), Quaternion.identity);
                    trapWallSegment.SetFallingDistance(i + 2);
                }

                hasBeenTriggered = true;
            }
        }
    }
}
