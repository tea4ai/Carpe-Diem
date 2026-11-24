using UnityEngine;

public class floatEffect : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float frequency = 2f;
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    void Start() => posOffset = transform.localPosition;

    void Update()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.localPosition = tempPos;
    }
}
