using System.Collections;
using UnityEngine;

public class Captaine : Character
{
    private void BackToHome()
    {
        Agent.SetDestination(Home.transform.position);
    }

    public void GoToTarget(Vector3 target)
    {
        Agent.destination = target;

        StartCoroutine(TrackingToTarget());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ResourceObject resourceObject))
        {
            if (!resourceObject.P_IsFound)
            {
                resourceObject.Found();
            }
        }
    }

    protected override void OnDayIsOver()
    {
        transform.position = Vector3.zero;
    }

    private IEnumerator TrackingToTarget()
    {
        var waitForSeconds = new WaitForSeconds(2f);
        yield return waitForSeconds;  

        yield return new WaitWhile(() => Agent.hasPath);
        
        BackToHome();
    }
}
