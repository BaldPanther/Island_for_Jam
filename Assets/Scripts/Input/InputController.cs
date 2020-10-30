using UnityEngine;
using UnityEngine.EventSystems;


public class InputController : MonoBehaviour
{
    [SerializeField] private GameObject _interactivObjectPanel;
    [SerializeField] private float _cameraMoveSpeed = 30;

    private Captaine _captaine;
    private ActionPanel _actionPanel;
    private Camera _mainCamera;
    private float _timeDoubleTap = .25f;
    private float _countdownDoubleTap;
    private float _countdownQuickTap;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _captaine = FindObjectOfType<Captaine>();
        _actionPanel = _interactivObjectPanel.GetComponent<ActionPanel>();
    }

    private void Update()
    {
        float horizontalDirection = Input.GetAxis("Horizontal");
        float verticalDirection = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontalDirection, verticalDirection);
        MoveCamera(direction);

        if (_countdownDoubleTap > -1)
            _countdownDoubleTap -= Time.deltaTime;
        if (_countdownQuickTap > -1)
            _countdownQuickTap -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Vector2 mousePosition = Input.mousePosition;

            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider == null)
                    return;
            }
            if (_countdownDoubleTap < 0)
            {
                _countdownDoubleTap = _timeDoubleTap;
                Click(hit, mousePosition);
            }
            else
            {
                DoubleTap(hit);
            }
        }
    }

    private void Click(RaycastHit hit, Vector2 mousePosition)
    {
        if (_interactivObjectPanel.activeInHierarchy && !_actionPanel.IsMouseInPanel(mousePosition))
        {
            _interactivObjectPanel.SetActive(false);
        }

        int maxColliders = 3;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(hit.point, 1, hitColliders);
        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].TryGetComponent(out ResourceObject interactivObject))       
            {
                if (!_interactivObjectPanel.activeInHierarchy && interactivObject.P_IsFound)
                {
                    _interactivObjectPanel.SetActive(true);
                    _actionPanel.SetPosition(mousePosition);
                    _actionPanel.DisplayInfo(interactivObject);
                }
                break;
            }
        }
    }

    private void DoubleTap(RaycastHit hit)
    {
        _captaine.GoToTarget(hit.point);
    }

    private void MoveCamera(Vector2 direction)
    {
        Vector3 moveDireection = new Vector3(direction.x, 0, direction.y);
        _mainCamera.transform.position += moveDireection * (_cameraMoveSpeed * Time.deltaTime);
    }

}
