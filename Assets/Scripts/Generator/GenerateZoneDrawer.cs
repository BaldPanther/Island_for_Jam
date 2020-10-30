using UnityEngine;

#if UNITY_EDITOR

[RequireComponent(typeof(Generator))]

public class GenerateZoneDrawer : MonoBehaviour
{
    [SerializeField] private Color _yellow = new Color(1, 1, 0, 0.5F);
    [SerializeField] private Color _red = new Color(1, 0, 0, 0.5F);
    [SerializeField] private Color _green = new Color(0, 1, 0, 0.5F);

    private Vector2Int _zoneSize;
    private Vector2Int _gredSize;
    private Generator _generator;

    void OnDrawGizmosSelected()
    {
        _generator = GetComponent<Generator>();
        _zoneSize = _generator.ZoneSize;
        _gredSize = _generator.GredSize;

        Gizmos.color = _yellow;

        Vector3Int ZoneSizeVector3 = new Vector3Int(_zoneSize.x, 1, _zoneSize.y);
        Gizmos.DrawCube(transform.position, ZoneSizeVector3);

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
        Gizmos.color = _red;

        int maxColliders = 5;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapBoxNonAlloc(position, new Vector3(0.4f, 0.4f, 0.4f), hitColliders);

        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].TryGetComponent(out Ground _))
                IsGround = true;
            if (hitColliders[i].TryGetComponent(out GeneratedUnit _))
                IsFree = false;
        }
        if (IsGround && IsFree)
        {
            Gizmos.color = _green;
        }

        Gizmos.DrawCube(position, new Vector3(_gredSize.x, 1, _gredSize.y));
    }
}
#endif