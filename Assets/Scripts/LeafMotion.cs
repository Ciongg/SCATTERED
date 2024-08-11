using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafMotion : MonoBehaviour
{
     public float fallSpeed = 0.5f; // speed leaf falls
    public float swayAmplitude = 0.5f; // leaf sways
    public float swayFrequency = 1.0f; // frequency of sway

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = fallSpeed; 
    }

    void Update()
    {
        
        Vector3 position = transform.position;
        position.x += Mathf.Sin(Time.time * swayFrequency) * swayAmplitude * Time.deltaTime;
        transform.position = position;
    }

}
