using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private Vector2Int _zoneSize;
    [SerializeField] private Vector2Int _gredSize;

    [SerializeField] private float _offsetDown = 0.4f;
    [SerializeField] private float _offsetUp = 0.4f;
    [SerializeField] private int _installationСhance = 5;

    [SerializeField] private GameObject _container;
    [SerializeField] private bool _rotate;
    [SerializeField] private GameObject[] _prefabs;

    private List<GameObject> _spawnedPool = new List<GameObject>();

    public Vector2Int ZoneSize => _zoneSize;
    public Vector2Int GredSize => _gredSize;
    public float OffsetDown => _offsetDown;
    public float OffsetUp => _offsetUp;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        float startPosX = transform.position.x - (_zoneSize.x / 2) + (_gredSize.x / 2);
        float startPosY = transform.position.z - (_zoneSize.y / 2) + (_gredSize.y / 2);
        
        for (float x = startPosX; x < startPosX + _zoneSize.x; x += _gredSize.x)
        {
            for (float y = startPosY; y < startPosY + _zoneSize.y; y += _gredSize.y)
            {
                GroundCheck(x, y);
            }
        }
    }

    private void GroundCheck(float x, float y)
    {
        Vector3 position = new Vector3(x, transform.position.y, y);

        Vector3 offsetPosition = new Vector3(position.x, position.y + _offsetUp, position.z);
        Vector3 endPosition = new Vector3(position.x, position.y - _offsetDown, position.z);

        if (Physics.Linecast(offsetPosition, endPosition, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Ground _))
                PlaceObject(hit.point);
        }
    }

    private bool TryGetObject(out GameObject spawned)
    {
        int randomIndex = Random.Range(0, _prefabs.Length);

        spawned = Instantiate(_prefabs[randomIndex], _container.transform);
        spawned.SetActive(false);

        _spawnedPool.Add(spawned);

        return spawned != null;
    }

    private void PlaceObject(Vector3 position)
    {
        bool canPlace = Random.Range(0, 100) < _installationСhance;
        if (!canPlace)
            return;

        if (TryGetObject(out GameObject spawnObject))
        {
            if (_rotate)
            {
                int rotateY = Random.Range(0, 360);
                spawnObject.transform.rotation = Quaternion.Euler(spawnObject.transform.rotation.x, rotateY, spawnObject.transform.rotation.z);
            }

            spawnObject.transform.position = position;
            spawnObject.SetActive(true);
        }
    }
}
