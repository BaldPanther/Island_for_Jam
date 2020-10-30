using System.Collections;
using UnityEngine;

public class WaterObject : ResourceObject
{
    [SerializeField] private float _hightLevelY;
    [SerializeField] private float _lowLevelY;
    private float _levelStep;
    private int _resourceStep;
    private int _fullResourceLevel;
    private int _previousResourceLevel;
    private int _maxResourcesAmount;

    private GameObject _child1;
    private Coroutine _levelWatchingCoroutine;
    private Coroutine _damagingCoroutine;

    private RandomEvents _randomEvents;

    protected override void Awake()
    {
        base.Awake();

        _child1 = gameObject.transform.GetChild(0).gameObject;
        _randomEvents = FindObjectOfType<RandomEvents>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _randomEvents.WasRain += OnWasRain;

        _previousResourceLevel = ResourcesAmount;

        if (_levelWatchingCoroutine != null)
            StopCoroutine(_levelWatchingCoroutine);

        _levelWatchingCoroutine = StartCoroutine(LevelWatching());
    }

    private void OnDisable()
    {
        _randomEvents.WasRain -= OnWasRain;
    }

    protected override void Start()
    {
        Type = "Water";
        Action = "Collect";
        _levelStep = (_hightLevelY - _lowLevelY) / 10;
        _maxResourcesAmount = _fullResourceLevel = ResourcesAmount;
        _resourceStep = _fullResourceLevel / 10;

        if (_damagingCoroutine != null)
            StopCoroutine(_damagingCoroutine);
        _damagingCoroutine = StartCoroutine(Damaging());
    }

    private void OnWasRain()
    {
        ResourcesAmount = _maxResourcesAmount;
        ResourcesText.text = ResourcesAmount.ToString();
        _previousResourceLevel = ResourcesAmount;
        IsResorcesFull = true;

        Map.ResourceObjectFull(this);

        if (IsFound)
        {
            _child1.SetActive(true);
            _child1.transform.localPosition = new Vector3(_child1.transform.localPosition.x, _hightLevelY, _child1.transform.localPosition.z);

            if (_levelWatchingCoroutine != null)
                StopCoroutine(_levelWatchingCoroutine);

            _levelWatchingCoroutine = StartCoroutine(LevelWatching());

            if (_damagingCoroutine != null)
                StopCoroutine(_damagingCoroutine);
            _damagingCoroutine = StartCoroutine(Damaging());
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    private IEnumerator LevelWatching()
    {
        while (true)
        {
            var waitForSeconds = new WaitForSeconds(0.5f);
            yield return waitForSeconds;

            if (ResourcesAmount <= 0)
                break;

            if (ResourcesAmount <= (_previousResourceLevel - _resourceStep))
            {
                _previousResourceLevel -= _resourceStep;

                _child1.transform.localPosition = new Vector3(_child1.transform.localPosition.x, _child1.transform.localPosition.y - _levelStep, _child1.transform.localPosition.z);
            }
        }
        _child1.SetActive(false);
    }

}
