using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private Vector2Int _zoneSize;
    [SerializeField] private Vector2Int _gredSize;

    [SerializeField] private int _installationСhance = 5;

    [SerializeField] private GameObject _container;
    [SerializeField] private bool _rotate;
    [SerializeField] private GameObject[] _prefabs;

    private List<GameObject> _spawnedPool = new List<GameObject>();

    public Vector2Int ZoneSize => _zoneSize;
    public Vector2Int GredSize => _gredSize;

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
        bool IsGround = false;
        bool IsFree = true;

        int maxColliders = 5;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapBoxNonAlloc(position, new Vector3(0.4f, 0.4f, 0.4f), hitColliders);

        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].TryGetComponent(out Ground _))
            {
                IsGround = true;
            }

            if (hitColliders[i].TryGetComponent(out GeneratedUnit unit))
                IsFree = false;
        }
        if (IsGround && IsFree)
        {
            PlaceObject(position);
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
