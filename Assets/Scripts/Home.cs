using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(WorkerSpawner))]

public class Home : MonoBehaviour
{
    [SerializeField] private int _allWorkers = 10;
    [SerializeField] private UIMenu _uiMenu;

    public int AllWorkers { get; private set; }
    public int FreeWorkers { get; private set; }

    private WorkerSpawner _workerSpawner;
    private ResourceWarehouse _resourceWarehouse;

    public event UnityAction WorkersChanged;

    private void Awake()
    {
        _workerSpawner = GetComponent<WorkerSpawner>();
        _resourceWarehouse = FindObjectOfType<ResourceWarehouse>();
        
        FreeWorkers = AllWorkers = _allWorkers;
    }

    private void Start()
    {
        WorkersChanged?.Invoke();

        _resourceWarehouse.UpdateResources();
    }

    public void RemoveWorker()
    {
        if (AllWorkers <= 0) 
            return;
        AllWorkers--;
        WorkersChanged?.Invoke();
    }

    public void TrySendToTarget(ResourceObject target, int count)
    {
        if (FreeWorkers > 0)
            StartCoroutine(_workerSpawner.GoToTarget(target, count));
        else
            News.Show("Не хватает рабочих!");
    }

    public void ReduseFreeWorker() => FreeWorkers--;

    public void KillWorker(int number)
    {
        if (number >= AllWorkers)
            GameOver();
        else
        {
            AllWorkers -= number;
            News.Show($"От нехватки ресурсов умерло {number} человек");
        }
        FreeWorkers = AllWorkers;
        WorkersChanged?.Invoke();
    }

    private void GameOver()
    {
        AllWorkers = 0;
        News.Show("GameOver - все мертвы");
        
        _uiMenu.OpenGameOverScreen();
    }

    public void TakeHome(GameObject character)
    {
        character.SetActive(false);
        if (character.TryGetComponent(out Worker _) && AllWorkers > FreeWorkers)
            FreeWorkers++;
    }

}
