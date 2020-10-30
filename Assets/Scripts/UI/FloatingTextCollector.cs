using System.Collections;
using UnityEngine;

public class FloatingTextCollector : MonoBehaviour
{
    [SerializeField] private float _waitTime = .5f;
    [SerializeField] private string _plusOrMinus;
    [SerializeField] private Color _color;

    private FloatingTextSpawner _floatingTextSpawner;
    private int _floatingNumber;
    private Coroutine _showingFloatingTextJob;

    private void Awake()
    {
        _floatingTextSpawner = FindObjectOfType<FloatingTextSpawner>();
    }

    public void Collect(int value)
    {
        _floatingNumber += value;

        if (_showingFloatingTextJob == null && _floatingNumber > 0)
            _showingFloatingTextJob = StartCoroutine(ShowingFloatingText());
    }

    private IEnumerator ShowingFloatingText()
    {
        var waitForSeconds = new WaitForSeconds(_waitTime);
        yield return waitForSeconds;

        _floatingTextSpawner.ShowFloatingText(transform.position, _plusOrMinus, _floatingNumber, _color);

        _floatingNumber = 0;
        _showingFloatingTextJob = null;
    }

}
