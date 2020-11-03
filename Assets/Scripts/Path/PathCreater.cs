using UnityEngine;
using UnityEngine.AI;

public class PathCreater : MonoBehaviour
{
    public NavMeshPath pathToTargetNavMesh;
    public NavMeshPath pathToHomeNavMesh;
    public Vector3[] pathToTarget;

    private Home _home;

    private void Awake()
    {
        _home = FindObjectOfType<Home>();
        pathToTargetNavMesh = new NavMeshPath();
        pathToHomeNavMesh = new NavMeshPath();
    }

    public void CreatePath()
    {
        Vector3 homePosition = _home.transform.GetComponent<Transform>().position;
        Vector3 targetPosition = transform.GetComponent<Transform>().position;

        if (NavMesh.CalculatePath(homePosition, targetPosition, NavMesh.AllAreas, pathToTargetNavMesh))
        {
            pathToTarget = pathToTargetNavMesh.corners;

            NavMesh.CalculatePath(targetPosition, homePosition, NavMesh.AllAreas, pathToHomeNavMesh);
        }
        else
        {
            print("Ошибка создания пути!");
        }
    }
}
