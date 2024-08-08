using UnityEngine;

public class Spin : MonoBehaviour
{
    public float minSpinSpeed = 5f;  // Minimum speed in degrees per second
    public float maxSpinSpeed = 20f; // Maximum speed in degrees per second

    private float spinSpeed;
    private int direction;

    void Start()
    {
        
        spinSpeed = Random.Range(minSpinSpeed, maxSpinSpeed);

        //1 for clockwise, -1 for counterclockwise
        direction = Random.Range(0, 2) == 0 ? 1 : -1;
    }

    void Update()
    {
        // Rotate around the z-axis with the randomized speed and direction
        transform.Rotate(0f, 0f, direction * spinSpeed * Time.deltaTime);
    }
}
