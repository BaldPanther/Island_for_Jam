using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : Character
{
    [SerializeField] private int _maxWeight = 45;
    [SerializeField] private int _extractionInSecond = 5;

    private int _currentWeight;
    private int _food;
    private int _water;
    private int _materials;

    private ResourceWarehouse _resourceWarehouse;
    

    protected override void Awake()
    {
        base.Awake();

        _resourceWarehouse = FindObjectOfType<ResourceWarehouse>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _currentWeight = _food = _water = _materials = 0;
    }

    private void Update()
    {
        if (ResourceObject == null && !IsGoToHome)
        {
            IsGoToHome = true;
            BackToHome();
        }
    }

    public void FollowToPath(NavMeshPath path)
    {
        Agent.SetPath(path);
        StartCoroutine(TrackingToTarget());
    }

    private IEnumerator TrackingToTarget()
    {
        var waitForSeconds = new WaitForSeconds(2f);
        yield return waitForSeconds;  

        yield return new WaitWhile(() => Agent.hasPath);

        if (ResourceObject != null)
            ResourceObject.StartJob();

        if (ResourceObject != null)
            yield return StartCoroutine(DoingJob());

        if (ResourceObject != null)
            ResourceObject.FinishJob();

        BackToHome();
    }

    private IEnumerator DoingJob()
    {
        while (ResourceObject.P_ResourcesAmount > 0 && _currentWeight < _maxWeight)
        {
            var waitForSeconds = new WaitForSeconds(1f);
            yield return waitForSeconds;

            if (_currentWeight + _extractionInSecond <= _maxWeight)
                _currentWeight += ResourceObject.GetResources(_extractionInSecond);
            else
                _currentWeight += ResourceObject.GetResources(_maxWeight - _currentWeight);
        }

        if (ResourceObject.P_Type == "Food")
            _food = _currentWeight;
        if (ResourceObject.P_Type == "Water")
            _water = _currentWeight;
        if (ResourceObject.P_Type == "Materials")
            _materials = _currentWeight;
    }

    private void BackToHome()
    {
        Agent.SetPath(PathToHome);

        StartCoroutine(TrackingToHomeWorker());
    }

    protected override void OnDayIsOver()
    {
        ResourceObject.FinishJob();
        Home.TakeHome(gameObject);
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }

    private IEnumerator TrackingToHomeWorker()
    {
        var waitForSeconds = new WaitForSeconds(2f);
        yield return waitForSeconds; 

        yield return new WaitWhile(() => Agent.hasPath);

        _resourceWarehouse.AddResources(_food, _water, _materials);
        Home.TakeHome(gameObject);
    }
}
