using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class TimeCounter : MonoBehaviour
{
    [SerializeField] private float _secondsInDay = 180f;
    [SerializeField] private GameObject _fadeScreen;

    private Image _fadeScreenImage;

    private float _currentTime;
    private float _secondsInHour;
    private bool _isTimeRunning;
    private int _hoursInDaylight = 14;
    private int _previousHour;
    private float _fadeDuration = 3f;

    private ResourceWarehouse _resourceWarehouse;

    public int CurrentDay { get; private set; }
    public int LeftHours { get; private set; }

    public event UnityAction TimeChanged;
    public event UnityAction DayIsOver;

    private void Awake()
    {
        Time.timeScale = 0;
        _resourceWarehouse = FindObjectOfType<ResourceWarehouse>();
        _fadeScreenImage = _fadeScreen.GetComponent<Image>();
    }

    private void Start()
    {
        _secondsInHour = _secondsInDay / _hoursInDaylight;
        _currentTime = _secondsInDay;

        StartNewDay();
    }

    private void Update()
    {
        if (!_isTimeRunning)
            return;

        LeftHours = Mathf.CeilToInt(_currentTime / _secondsInHour);
        
        if (LeftHours < _previousHour)
        {
            _previousHour = LeftHours;
            TimeChanged?.Invoke();
        }

        if (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
        }
        else
        {
            _isTimeRunning = false;

            StartCoroutine(DayCommingEnd());
        }
    }

    private void StartNewDay()
    {
        CurrentDay++;
        _currentTime = _secondsInDay;
        LeftHours = _previousHour = _hoursInDaylight;
        _isTimeRunning = true;

        TimeChanged?.Invoke();

        News.Show($"Начался {CurrentDay} день.");
    }

    private IEnumerator DayCommingEnd()
    {
        var waitForSeconds = new WaitForSeconds(_fadeDuration);

        News.Show($"День {CurrentDay} закончен.");

        _fadeScreenImage.CrossFadeAlpha(0, 0, true);
        _fadeScreen.SetActive(true);
        _fadeScreenImage.CrossFadeAlpha(1, _fadeDuration, true);

        yield return waitForSeconds;

        DayIsOver?.Invoke();
        
        _fadeScreenImage.CrossFadeAlpha(0, _fadeDuration, true);

        yield return waitForSeconds;

        _fadeScreen.SetActive(false);
        _resourceWarehouse.SpendResources();
        StartNewDay();
    }
}
