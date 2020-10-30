using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuExitScreen; 
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _introScreen;
    [SerializeField] private GameObject _winScreen;

    [SerializeField] private TMP_Text _daysPassedText1;
    [SerializeField] private TMP_Text _daysPassedText2;

    private TimeCounter _timeCounter;
    private ResourceWarehouse _resourceWarehouse;

    private void Awake()
    {
        _timeCounter = FindObjectOfType<TimeCounter>();
        _resourceWarehouse = FindObjectOfType<ResourceWarehouse>();
    }

    private void OnEnable()
    {
        _resourceWarehouse.MaterialsChanged += OnMaterialsChanged;
    }

    private void OnDisable()
    {
        _resourceWarehouse.MaterialsChanged -= OnMaterialsChanged;
    }

    public void LoadGame()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }  

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenMenuScreen()
    {
        _menuExitScreen.SetActive(true);
        _daysPassedText1.text = $"{_timeCounter.CurrentDay}";
    }

    public void OpenGameOverScreen()
    {
        Time.timeScale = 0;
        _gameOverScreen.SetActive(true);
        _daysPassedText2.text = $"{_timeCounter.CurrentDay}";
    }

    public void OpenWinScreen()
    {
        Time.timeScale = 0;
        _winScreen.SetActive(true);
    }

    public void CloseIntroScreen()
    {
        Time.timeScale = 1;
    }

    private void OnMaterialsChanged(int value)
    {
        if (value >= 10000)
            OpenWinScreen();
    }
}
