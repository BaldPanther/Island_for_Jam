using UnityEngine;

public class FPS : MonoBehaviour
{
    private float _updateInterval = 0.5F;
    private double _lastInterval;
    private int _frames = 0;
    private float _fps;
    private float _minFps = 3000;

    private void Start()
    {
        _lastInterval = Time.realtimeSinceStartup;
        _frames = 0;
    }

    private void OnGUI()
    {
        GUILayout.Label("FPS = " + _fps.ToString("f0"));
        GUILayout.Label("Min = " + _minFps.ToString("f0"));
    }

    private void Update()
    {
        ++_frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > _lastInterval + _updateInterval)
        {
            _fps = (float)(_frames / (timeNow - _lastInterval));
            _frames = 0;
            _lastInterval = timeNow;

            if (_fps < _minFps && timeNow > 5)
                _minFps = _fps;
        }
    }
}


