using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.


    void Update()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            Debug.Log("2 touches");
        }
    }
}