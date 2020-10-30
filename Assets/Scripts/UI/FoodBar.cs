
public class FoodBar : Bar
{
    private void OnEnable()
    {
        Warehouse.FoodChanged += OnValueChanged;
        FilledImage.fillAmount = 0;
    }

    private void OnDisable()
    {
        Warehouse.FoodChanged -= OnValueChanged;
    }
}
