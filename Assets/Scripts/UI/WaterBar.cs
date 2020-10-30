
public class WaterBar : Bar
{
    private void OnEnable()
    {
        Warehouse.WaterChanged += OnValueChanged;
        FilledImage.fillAmount = 0;
    }

    private void OnDisable()
    {
        Warehouse.WaterChanged -= OnValueChanged;
    }
}
