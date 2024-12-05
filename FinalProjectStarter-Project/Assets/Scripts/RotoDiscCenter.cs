using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotoDiscCenter : MonoBehaviour
{
    public RotoDisc rotoDiscPrefab;
    private RotoDisc rotoDisc = null;
    // Start is called before the first frame update
    void Start()
    {
        rotoDisc = Instantiate(rotoDiscPrefab, new Vector2(transform.position.x, transform.position.y + 3.0f), Quaternion.identity);
        rotoDisc.SetAnchorPosition(new Vector2(transform.localPosition.x, transform.localPosition.y));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
