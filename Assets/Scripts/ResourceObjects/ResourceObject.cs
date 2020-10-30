using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PathCreater))]
[RequireComponent(typeof(FloatingTextCollector))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(TMP_Text))]

public class ResourceObject : MonoBehaviour
{
    protected string Type = "Box";

    [SerializeField] protected string Label;
    protected string Action = "See";
    [SerializeField] protected int ResourcesAmount = 1000;
    [SerializeField] protected int DamageInSecond = 1;
    [SerializeField] protected bool IsFound = false;

    protected int WorkersAtWork = 0;

    protected ParticleSystem ParticleSystem;
    protected PathCreater PathManager;
    protected GameObject MeshObject;
    protected GameObject ResourcesTextObject;
    protected TMP_Text ResourcesText;
    protected FloatingTextCollector FloatingTextCollector;
    protected bool IsResorcesFull;

    public bool P_IsFound => IsFound;
    public string P_Type => Type;
    public string P_Label => Label;
    public string P_Action => Action;
    public int P_ResourcesAmount => ResourcesAmount;

    protected virtual void Awake()
    {
        FloatingTextCollector = GetComponent<FloatingTextCollector>();
        PathManager = GetComponent<PathCreater>();

        ParticleSystem = GetComponentInChildren<ParticleSystem>();
        MeshObject = gameObject.transform.GetChild(0).gameObject;
        ResourcesTextObject = gameObject.transform.GetChild(2).gameObject;
        ResourcesText = ResourcesTextObject.GetComponent<TMP_Text>();
    }

    protected virtual void Start()
    {
        StartCoroutine(Damaging());
    }

    protected virtual void OnEnable()
    {
        MeshObject.SetActive(false);
        ResourcesTextObject.SetActive(false);
        IsResorcesFull = true;
    }

    protected virtual void Update()
    {
        if (ParticleSystem.isPlaying && WorkersAtWork <= 0)
        {
            ParticleSystem.Stop();
        }
        if (ResourcesAmount <= 0 && IsResorcesFull)
        {
            IsResorcesFull = false;
            Map.ResourceObjectEmpty(this);

            if (ResourcesAmount <= 0 && Type != "Water")
                Destroy(gameObject);
        }
    }

    public void Found()
    {
        if (!IsFound)
        {
            IsFound = true;
            MeshObject.SetActive(true);
            PathManager.CreatePath();
            ResourcesTextObject.SetActive(true);

            Map.NewResourceObject(this);
        }
    }

    public NavMeshPath GetPathToHome()
    {
        return PathManager.pathToHomeNavMesh;
    }

    public int GetResources(int extractionInSecond)
    {
        if (extractionInSecond <= ResourcesAmount)
        {
            ResourcesAmount -= extractionInSecond;

            FloatingTextCollector.Collect(extractionInSecond);

            return extractionInSecond;
        }
        else
        {
            return ResourcesAmount = 0;
        }
    }

    public void StartJob()
    {
        WorkersAtWork++;

        if (!ParticleSystem.isPlaying)
            ParticleSystem.Play();
    }
    public void FinishJob() => WorkersAtWork--;

    protected IEnumerator Damaging()
    {
        while (ResourcesAmount > 0)
        {
            var waitForSeconds = new WaitForSeconds(1f);
            yield return waitForSeconds;

            ResourcesAmount -= DamageInSecond;
            if (ResourcesAmount < 0)
                ResourcesAmount = 0;

            ResourcesText.text = ResourcesAmount.ToString();
        }
    }

}
