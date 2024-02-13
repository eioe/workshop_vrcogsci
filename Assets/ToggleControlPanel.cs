using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleControlPanel : MonoBehaviour
{

    public Canvas ControlPanelCanvas;
    // Start is called before the first frame update

    public void TogglePanel()
    {
        Debug.Log("TogglePanel");
        // Hide the Canvas
        ControlPanelCanvas.enabled = !ControlPanelCanvas.enabled;

    }
}
