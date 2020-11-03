using System.Collections;
using TMPro;
using UnityEngine;

public class NewsPanel : MonoBehaviour
{
    [SerializeField] private float _viewTime = 3f;
    [SerializeField] private GameObject _newsPanelPrefab;

    private GameObject[] _panelsPool;
    private int _maxPanelsCount = 10;

    private void OnEnable()
    {
        News.ShowNews += OnShowNews;
    }

    private void OnDisable()
    {
        News.ShowNews -= OnShowNews;
    }

    private void Awake()
    {
        _panelsPool = new GameObject[_maxPanelsCount];
        Initialize(_newsPanelPrefab);
    }

    private void Initialize(GameObject prefab)
    {
        for (int i = 0; i < _maxPanelsCount; i++)
        {
            GameObject spawned = Instantiate(prefab, gameObject.transform);
            spawned.SetActive(false);

            _panelsPool[i] = spawned;
        }
    }

    private void OnShowNews(string newsText)
    {
        GameObject panel = GetFirstInactivPanel();

        if (panel != null)
        {
            StartCoroutine(Printing(newsText, panel));
        }
    }

    private GameObject GetFirstInactivPanel()
    {
        for (int i = 0; i < _maxPanelsCount; i++)
        {
            if (!_panelsPool[i].activeInHierarchy)
            {
                return _panelsPool[i];
            }
        }
        return null;
    }

    private IEnumerator Printing(string someText, GameObject panel)
    {
        var waitForSeconds = new WaitForSeconds(_viewTime);

        panel.SetActive(true);

        TMP_Text _infoText = panel.GetComponentInChildren<TMP_Text>();
        _infoText.text = someText;

        yield return waitForSeconds;

        panel.SetActive(false);
    }
}
