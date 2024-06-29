using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public GUIStyle ClockStyle;

    private float _timer;
    private float _minutes;
    private float _seconds;
    
    private const float VirtualWidth = 480f;
    private const float VirtualHeight = 854f;

    private bool _stopTimer;
    private Matrix4x4 _matrix;
    private Matrix4x4 _oldMatrix;
    void Start()
    {
        _stopTimer = false;
        _matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width/VirtualWidth,Screen.height/VirtualHeight,1f));
        _oldMatrix = GUI.matrix;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_stopTimer)
        {
            _timer += Time.deltaTime;
        }
    }
    private void OnGUI()
    {
        GUI.matrix = _matrix;

        _minutes = Mathf.Floor(_timer / 60);
        _seconds = Mathf.Floor(_timer % 60);

        GUI.Label(new Rect(Camera.main.rect.x + 20, 10, 120, 50),"" +   _minutes.ToString("00") + ":" + _seconds.ToString("00"),ClockStyle);
        GUI.matrix = _oldMatrix;
    }
}
