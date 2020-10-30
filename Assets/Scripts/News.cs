using UnityEngine.Events;

public class News
{
    public static event UnityAction<string> ShowNews;

    public static void Show(string newsText)
    {
        ShowNews?.Invoke(newsText);
    }
}
