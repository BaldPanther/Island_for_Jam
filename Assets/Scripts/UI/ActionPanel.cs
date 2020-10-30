using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ActionPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private TMP_Text _actionButtonText;
    [SerializeField] private Slider _valueSlider;

    private RectTransform _rectTransform;
    private ResourceObject _interactivObject;
    private Home _home;

    private float _widthScreen;
    private float _heightScreen;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _home = FindObjectOfType<Home>();

        _widthScreen = (float)Screen.width;
        _heightScreen = (float)Screen.height;
    }

    public void SetPosition(Vector2 position)
    {
        Vector3[] worldCornres = new Vector3[4];
        _rectTransform.GetWorldCorners(worldCornres);

        float halfSizePanelX = (worldCornres[2].x - worldCornres[0].x) / 2;
        float halfSizePanelY = (worldCornres[2].y - worldCornres[0].y) / 2;

        float minX = halfSizePanelX;
        float maxX = _widthScreen - halfSizePanelX;
        float minY = halfSizePanelY;
        float maxY = _heightScreen - halfSizePanelY;

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        _rectTransform.position = position;
    }

    public bool IsMouseInPanel(Vector2 mousePosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, mousePosition);
    }

    public void DisplayInfo(ResourceObject objectInfo)
    {
        _interactivObject = objectInfo;

        _labelText.text = objectInfo.P_Label;
        _infoText.text = " Left - " + objectInfo.P_ResourcesAmount;
        _actionButtonText.text = objectInfo.P_Action;
    }

    public void OnActionButtonClick()
    {
        int numberInt = Convert.ToInt32(_valueSlider.value * _home.FreeWorkers);
        if (numberInt < 1)
            numberInt = 1;

        _home.TrySendToTarget(_interactivObject, numberInt);

        gameObject.SetActive(false);
    }
}
