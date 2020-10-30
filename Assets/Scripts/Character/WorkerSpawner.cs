using System.Collections;
using UnityEngine;

public class WorkerSpawner : ObjectsPool
{
    [SerializeField] private GameObject _workerPrefab;
    [SerializeField] private float _timeBetweenSpawn = .3f;

    private Home _home;

    private void Awake()
    {
        _home = FindObjectOfType<Home>();
        Initialize(_workerPrefab);
    }

    public IEnumerator GoToTarget(ResourceObject target, int countBugs)
    {
        var waitForSeconds = new WaitForSeconds(_timeBetweenSpawn);

        for (int i = 0; i < countBugs; i++)
        {
            if (_home.FreeWorkers <= 0)
            {
                News.Show("Закончились рабочие!");
                break;
            }

            if (TryGetObject(out GameObject bugObject))
            {
                SetObject(bugObject, transform.position);

                if (target.TryGetComponent(out PathCreater pathToTarget))
                {
                    Worker bug = bugObject.GetComponent<Worker>();

                    bug.SetTarget(target);
                    bug.FollowToPath(pathToTarget.pathToTargetNavMesh);
                }
                _home.ReduseFreeWorker();
            }

            yield return waitForSeconds;
        }
    }

    public Worker GetWorker(int index)
    {
        GameObject resultObject = GetObjectForIndex(index);

        return resultObject.GetComponent<Worker>();
    }
}
