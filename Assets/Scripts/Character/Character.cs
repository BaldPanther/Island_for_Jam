using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(MeshRenderer))]

public class Character : MonoBehaviour
{
    [SerializeField] protected float Speed = 3f;

    protected Home Home;
    protected TimeCounter TimeCounter;
    protected NavMeshAgent Agent;
    protected ResourceObject ResourceObject;
    protected NavMeshPath PathToHome;
    protected MeshRenderer Mesh;
    protected float BornTime;
    protected bool IsGoToHome;
    protected bool IsHome;

    protected virtual void Awake()
    {
        Home = FindObjectOfType<Home>();
        TimeCounter = FindObjectOfType<TimeCounter>();
        Agent = GetComponent<NavMeshAgent>();
        Mesh = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        Agent.updateRotation = false;
    }

    protected virtual void OnEnable()
    {
        BornTime = Time.time;
        IsGoToHome = false;
        TimeCounter.DayIsOver += OnDayIsOver;
    }

    private void OnDisable()
    {
        TimeCounter.DayIsOver -= OnDayIsOver;
    }

    private void LateUpdate()
    {
        if (Agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Agent.velocity.normalized), 15f * Time.deltaTime);
        }
    }

    public void StopNavMesh()
    {
        Agent.isStopped = true;
    }

    public void SetTarget(ResourceObject resourceObject)
    {
        ResourceObject = resourceObject;
        PathToHome = ResourceObject.GetPathToHome();
    }

    protected virtual void OnDayIsOver() { }

    protected IEnumerator TrackingToHome()
    {
        var waitForSeconds = new WaitForSeconds(2f);
        yield return waitForSeconds;  

        yield return new WaitWhile(() => Agent.hasPath);

        Home.TakeHome(gameObject);
    }

}
