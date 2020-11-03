using UnityEngine;

#if UNITY_EDITOR

[RequireComponent(typeof(Generator))]

public class GenerateZoneDrawer : MonoBehaviour
{
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

        Gizmos.color = _red;

        Vector3 offsetPosition = new Vector3(position.x, position.y + _generator.OffsetUp, position.z);
        Vector3 endPosition = new Vector3(position.x, position.y - _generator.OffsetDown, position.z);

        if (Physics.Linecast(offsetPosition, endPosition, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Ground _))
                Gizmos.color = _green;

            Gizmos.DrawCube(hit.point, new Vector3(_gredSize.x, 1, _gredSize.y));
        }
        Gizmos.DrawCube(position, new Vector3(_gredSize.x, 1, _gredSize.y));
    }
}
#endif