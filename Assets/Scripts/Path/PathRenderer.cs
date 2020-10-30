using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]

public class PathRenderer : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawPath(NavMeshPath path)
    {
        _lineRenderer.positionCount = path.corners.Length;
        _lineRenderer.SetPositions(path.corners);
    }

    public void DrawPath(Vector3[] path)
    {
        _lineRenderer.positionCount = path.Length;
        _lineRenderer.SetPositions(path);
    }
}
