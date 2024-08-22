using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    public float speed = 100.0f;  // Speed of rotation
    private Vector3 randomDirection;

    void Start()
    {
        // Generate a random rotation direction at start
        randomDirection = new Vector3(
            Random.Range(-1.0f, 1.0f),  // Random x-axis rotation
            Random.Range(-1.0f, 1.0f),  // Random y-axis rotation
            Random.Range(-1.0f, 1.0f)   // Random z-axis rotation
        ).normalized;  // Normalize to keep uniform speed in any direction
    }

    void Update()
    {
        // Rotate the object around the random direction at the specified speed
        transform.Rotate(randomDirection * speed * Time.deltaTime);
    }
}