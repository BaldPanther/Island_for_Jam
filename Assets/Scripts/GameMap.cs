using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{ 
}

public class Map
{
    private static List<ResourceObject> _resourceObjectList = new List<ResourceObject>();

    private static float _nearestFoodDistance;
    private static float _nearestWaterDistance;

    public static ResourceObject NearestFood { get; private set; }
    public static ResourceObject NearestWater { get; private set; }

    private static void SetNearestObject(ResourceObject resourceObject)
    {
        float distance = Vector3.Distance(Vector3.zero, resourceObject.transform.position);

        switch (resourceObject.P_Type)
        {
            case "Food":
                if (_nearestFoodDistance <= 0 || _nearestFoodDistance > distance)
                {
                    _nearestFoodDistance = distance;
                    NearestFood = resourceObject;
                }
                break;

            case "Water":
                if (_nearestWaterDistance <= 0 || _nearestWaterDistance > distance)
                {
                    _nearestWaterDistance = distance;
                    NearestWater = resourceObject;
                }
                break;
        }
    }

    private static void FindNearestObjectInList()
    {
        _nearestFoodDistance = _nearestWaterDistance = 0;
        
        foreach (var item in _resourceObjectList)
        {
            SetNearestObject(item);
        }
    }

    public static void NewResourceObject(ResourceObject resourceObject)
    {
        News.Show($"NEWS: Найден новый объект - {resourceObject.P_Label}");

        _resourceObjectList.Add(resourceObject);
        SetNearestObject(resourceObject);
    }

    public static void ResourceObjectFull(ResourceObject resourceObject)
    {
        if (_resourceObjectList.Contains(resourceObject))
            return;
        _resourceObjectList.Add(resourceObject);
        SetNearestObject(resourceObject);
    }

    public static void ResourceObjectEmpty(ResourceObject resourceObject)
    {
        News.Show($"NEWS: {resourceObject.P_Label} закончились ресурсы");

        _resourceObjectList.Remove(resourceObject);

        if (resourceObject == NearestFood || resourceObject == NearestWater)
            FindNearestObjectInList();
    }
}
