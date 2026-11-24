using UnityEngine;

public class startAudioOnVisible : MonoBehaviour
{
    private Enemy parent;
    void Awake()
    {
        parent = GetComponentInParent<Enemy>();
    }

    void OnBecameVisible()
    {
        if (parent != null) parent.isVisible = true;
    }

    void OnBecameInvisible()
    {
        if (parent != null) parent.isVisible = false;
    }
}
