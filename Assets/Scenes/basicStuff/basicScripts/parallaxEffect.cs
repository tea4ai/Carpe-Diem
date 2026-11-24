using UnityEngine;

public class parallaxEffect : MonoBehaviour
{
    private float length, startPos;
    [SerializeField] private Transform cam;          
    [SerializeField] private float parallaxFactor;   
    [SerializeField] private bool lockVertical = true; 

    void Start()
    {
        startPos = transform.position.x - (cam.position.x * parallaxFactor);

        if (GetComponent<SpriteRenderer>() != null)
        {
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }
    }

    void LateUpdate()
    {
        // Calculate the relative distance for the infinite loop check
        float temp = (cam.position.x * (1 - parallaxFactor));

        // Calculate how much the layer should actually move
        float dist = (cam.position.x * parallaxFactor);

        // Apply movement
        float newY = lockVertical ? cam.position.y : transform.position.y;
        transform.position = new Vector3(startPos + dist, newY, transform.position.z);

        // Horizontal Infinite Loop Logic
        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        }
    }
}
