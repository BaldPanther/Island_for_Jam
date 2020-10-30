using TMPro;
using UnityEngine;

public class FloatingTextSpawner : ObjectsPool
{
    [SerializeField] private float _randomOffset = 1f;
    [SerializeField] private GameObject _pfFloatingText;

    private void Start()
    {
        Initialize(_pfFloatingText);
    }

    public void ShowFloatingText(Vector3 position, string plusOrMinus, int value, Color color)
    {
        position += new Vector3(Random.Range(-_randomOffset, _randomOffset), 0, Random.Range(-_randomOffset, _randomOffset));

        if (TryGetObject(out GameObject floatingTextObject))
        {
            SetObject(floatingTextObject, position);
        }
        TMP_Text floatingTextObjectText = floatingTextObject.GetComponent<TMP_Text>();
        floatingTextObjectText.text = plusOrMinus + value.ToString();
        floatingTextObjectText.color = color;
    }

}
