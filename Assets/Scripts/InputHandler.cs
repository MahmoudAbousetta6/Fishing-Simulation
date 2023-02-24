using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private FishingLine line;
    private bool isGrap;

    private List<UnityEngine.XR.InputDevice> inputDevices;

    private void Start() => inputDevices = new List<UnityEngine.XR.InputDevice>();

    private void HandlePrimary2DAxis(Vector2 pos)
    { 
        if(pos.y == 0.0f) return;
        var step = pos.y < 0?-1.0f:1.0f;
        line.LineLength += (0.005f * step);
        line.LineLength = Mathf.Clamp(line.LineLength, 0, 1);
    }

    private void HandleGripButton(bool isPressed)
    {
        if (!isPressed) return;
        isGrap = true;
        line.LinePointInit(10);
        line.LineLength = 0.22f;
        line.DT = .0888f;
    }

    private void Update()
    {
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        foreach (var device in inputDevices)
        {
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis,
                    out var posValue))
                HandlePrimary2DAxis(posValue);

            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton,
                    out var g) && g && !isGrap)
                HandleGripButton(g);
        }
    }
}