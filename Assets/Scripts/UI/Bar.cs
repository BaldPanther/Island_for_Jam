using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public abstract class Bar : MonoBehaviour
{
    [SerializeField] protected Image FilledImage;
    [SerializeField] protected TMP_Text DaysText;
    [SerializeField] private float _lerpDuration = 2f;

    protected float _currentValue;
    protected float _targetValue;
    protected Coroutine _fillingCoroutine;
    protected ResourceWarehouse Warehouse;

    protected virtual void Awake()
    {
        Warehouse = FindObjectOfType<ResourceWarehouse>();
    }

    protected void OnValueChanged(int value, int maxValue)
    {
        if (value > maxValue)
        {
            DaysText.text = value.ToString();
            value = maxValue;
        }
        else
        {
            DaysText.text = " ";
        }

        _currentValue = FilledImage.fillAmount;
        _targetValue = (float)value / maxValue;

        if (_currentValue != _targetValue)
        {
            if (_fillingCoroutine != null)
                StopCoroutine(_fillingCoroutine);
            _fillingCoroutine = StartCoroutine(Filling(_currentValue, _targetValue, _lerpDuration));
        }
    }

    private IEnumerator Filling(float startValue, float endValue, float duration)
    {
        float elapsed = 0;
        float nextValue;

        while (elapsed < duration)
        {
            nextValue = Mathf.Lerp(startValue, endValue, elapsed / duration);
            FilledImage.fillAmount = nextValue;
            elapsed += Time.deltaTime;
            yield return null;
        }
        FilledImage.fillAmount = endValue;
    }

}
