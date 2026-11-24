using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] private CinemachineCamera virtualCamera;

    private Transform playerTransform;

    void Awake()
    {
        instance = this;
        if(virtualCamera.Follow != null)
        {
            playerTransform = virtualCamera.Follow;
        }
    }
    public void FreezeCamera(bool freeze)
    {
        if (freeze) virtualCamera.Follow = null;
        else virtualCamera.Follow = playerTransform;
    }
}
