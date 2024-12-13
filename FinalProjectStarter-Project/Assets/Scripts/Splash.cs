using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("SplashAnim");
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 1.2f);
    }
}
