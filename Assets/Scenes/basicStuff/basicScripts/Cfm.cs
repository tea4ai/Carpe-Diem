using UnityEngine;

public class Cfm : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = -200f;
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed);
    }
}
