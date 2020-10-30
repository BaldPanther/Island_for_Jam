using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FloatingTextCollector))]

public class ResourceWarehouse : MonoBehaviour
{
    [SerializeField] private int _workerFoodUsePerDay = 3;
    [SerializeField] private int _workerWaterUsePerDay = 5;
    [SerializeField] private int _food;
    [SerializeField] private int _water;
    [SerializeField] private int _materials;

    private int _foodForDays;
    private int _waterForDays;
    private int _allWorkers;

    private Home _home;
    private FloatingTextCollector _floatingTextCollector;

    public event UnityAction<int, int> FoodChanged;
    public event UnityAction<int, int> WaterChanged;
    public event UnityAction<int> MaterialsChanged;

    private void Awake()
    {
        _home = FindObjectOfType<Home>();
        _floatingTextCollector = GetComponent<FloatingTextCollector>();
    }

    private void Start()
    {
        UpdateResources();
    }

    public void UpdateResources()
    {
        _allWorkers = _home.AllWorkers;
        
        if (_allWorkers <= 0)
            return;

        if (_food >= 0)
        {
            _foodForDays = _food / (_workerFoodUsePerDay * _allWorkers);
            FoodChanged?.Invoke(_foodForDays, 10);
        }
        if (_water >= 0)
        {
            _waterForDays = _water / (_workerWaterUsePerDay * _allWorkers);
            WaterChanged?.Invoke(_waterForDays, 10);
        }
        MaterialsChanged?.Invoke(_materials);
    }

    public void AddResources(int food, int water, int materials)
    {
        if (food > 0)
            _food += food;

        if (water > 0)
            _water += water;

        if (materials > 0)
            _materials += materials;

        UpdateResources();

        _floatingTextCollector.Collect(food + water + materials);
    }

    public void SpendResources()
    {
        _allWorkers = _home.AllWorkers;

        int needFood = _workerFoodUsePerDay * _allWorkers;
        int needWater = _workerWaterUsePerDay * _allWorkers;
        int needKill = 0;

        if (_food >= needFood)
        {
            _food -= needFood;
        }
        else
        {
            needKill = _allWorkers - (_food / _workerFoodUsePerDay);
            _food = 0;
        }

        if (_water >= needWater)
        {
            _water -= needWater;
        }
        else
        {
            if (needKill < _allWorkers - (_water / _workerWaterUsePerDay))
                needKill = _allWorkers - (_water / _workerWaterUsePerDay);
            _water = 0;
        }
        
        if (needKill > 0)
            _home.KillWorker(needKill);

        UpdateResources();
    }

}
