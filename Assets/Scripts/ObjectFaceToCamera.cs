using UnityEngine;

public class ObjectFaceToCamera : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward,
            _mainCamera.transform.rotation * Vector3.up);
    }
}
