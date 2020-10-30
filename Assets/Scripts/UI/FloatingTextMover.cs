using UnityEngine;

public class FloatingTextMover : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 2f;

    private float _countdownLifeTime;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _countdownLifeTime = _lifeTime;
    }

    private void Update()
    {
        _countdownLifeTime -= Time.deltaTime;

        if (_countdownLifeTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward,
            _mainCamera.transform.rotation * Vector3.up);
    }

}
