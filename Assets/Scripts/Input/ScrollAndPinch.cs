using UnityEngine;
using UnityEngine.EventSystems;

class ScrollAndPinch : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Terrain _terrain;
    [SerializeField] private bool _rotate;

    [SerializeField] private float _minZoom = 15;
    [SerializeField] private float _maxZoom = 150;

    private Vector3 _cameraNewPosition;

    private float _terrainOffset = 500;

    private Plane _plane;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount >= 1)
            _plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        if (Input.touchCount >= 1)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                _camera.transform.Translate(Delta1, Space.World);

                _cameraNewPosition.x = Mathf.Clamp(_camera.transform.position.x, 0 - _terrainOffset, 0 + _terrainOffset);
                _cameraNewPosition.y = _camera.transform.position.y;
                _cameraNewPosition.z = Mathf.Clamp(_camera.transform.position.z, 0 - _terrainOffset, 0 + _terrainOffset);
                _camera.transform.position = _cameraNewPosition;
            }
        }

        if (Input.touchCount >= 2)
        {
            var pos1  = PlanePosition(Input.GetTouch(0).position);
            var pos2  = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            var zoom = Vector3.Distance(pos1, pos2) /
                       Vector3.Distance(pos1b, pos2b);
            

            if (zoom == 0 || zoom > 10)
                return;

            _cameraNewPosition = Vector3.LerpUnclamped(pos1, _camera.transform.position, 1 / zoom);

            _cameraNewPosition.y = Mathf.Clamp(_cameraNewPosition.y, _minZoom, _maxZoom);

            _camera.transform.position = _cameraNewPosition;
            
            if (_rotate && pos2b != pos2)
                _camera.transform.RotateAround(pos1, _plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, _plane.normal));
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            float mouseScroll = Input.mouseScrollDelta.y * 3;
            
            _cameraNewPosition = new Vector3(_camera.transform.position.x, _camera.transform.position.y - mouseScroll, _camera.transform.position.z);

            _cameraNewPosition.y = Mathf.Clamp(_cameraNewPosition.y, _minZoom, _maxZoom);

            _camera.transform.position = _cameraNewPosition;
        }
    }

    private Vector3 PlanePositionDelta(Touch touch)
    {
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;
        if (EventSystem.current.IsPointerOverGameObject())
            return Vector3.zero;

        var rayBefore = _camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = _camera.ScreenPointToRay(touch.position);
        if (_plane.Raycast(rayBefore, out var enterBefore) && _plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private Vector3 PlanePosition(Vector2 screenPos)
    {
        var rayNow = _camera.ScreenPointToRay(screenPos);
        if (_plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }
}