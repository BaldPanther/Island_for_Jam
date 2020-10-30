using TMPro;
using UnityEngine;

public class UIInfoShower : MonoBehaviour
{
    [SerializeField] private TMP_Text _allWorkersText;
    [SerializeField] private TMP_Text _materialsText;
    [SerializeField] private TMP_Text _timeText;

    private Home _home;
    private ResourceWarehouse _resourceWarehouse;
    private TimeCounter _timeCounter;

    private void Awake()
    {
        _home = FindObjectOfType<Home>();
        _resourceWarehouse = FindObjectOfType<ResourceWarehouse>();
        _timeCounter = FindObjectOfType<TimeCounter>();
    }

    private void OnEnable()
    {
        _home.WorkersChanged += OnWorkersChanged;
        _resourceWarehouse.MaterialsChanged += OnMaterialsChanged;
        _timeCounter.TimeChanged += OnTimeChanged;
    }

    private void OnDisable()
    {
        _home.WorkersChanged -= OnWorkersChanged;
        _resourceWarehouse.MaterialsChanged -= OnMaterialsChanged;
        _timeCounter.TimeChanged -= OnTimeChanged;
    }

    private void OnWorkersChanged()
    {
        _allWorkersText.text = $"Выживших - {_home.AllWorkers}";
    }

    private void OnMaterialsChanged(int value)
    {
        _materialsText.text = $"Материалов - {value}";
    }

    private void OnTimeChanged()
    {
        int hoursLeft = _timeCounter.LeftHours;
        string hours = $"осталось {hoursLeft} часов";

        if (hoursLeft == 1)
            hours = "остался 1 час!";

        _timeText.text = $"День {_timeCounter.CurrentDay} - {hours}";
    }
}
