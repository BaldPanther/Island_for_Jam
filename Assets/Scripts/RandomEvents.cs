using UnityEngine;
using UnityEngine.Events;

public class RandomEvents : MonoBehaviour
{
    [SerializeField] private int _rainChance = 15;

    private TimeCounter _timeCounter;

    public event UnityAction WasRain;

    private void Awake()
    {
        _timeCounter = FindObjectOfType<TimeCounter>();
    }

    private void OnEnable()
    {
        _timeCounter.TimeChanged += OnTimeChanged;
    }

    private void OnDisable()
    {
        _timeCounter.TimeChanged -= OnTimeChanged;
    }

    private void OnTimeChanged()
    {
        if (Random.Range(0, 100) < _rainChance)
        {
            News.Show("NEWS: Прошел дождь");
            WasRain?.Invoke();
        }
    }
}
