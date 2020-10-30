
public class FoodObject : ResourceObject
{
    protected override void Start()
    {
        Type = "Food";
        Action = "Collect";

        base.Start();
    }
}
