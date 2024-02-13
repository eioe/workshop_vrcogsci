using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueFixManager : MonoBehaviour
{
    public GameObject Cue;
    public GameObject FixationCross;

    public void ShowFixationCross(bool show){
       FixationCross.SetActive(show);
    }

    public void ShowCue(bool show) {
        Cue.SetActive(show);
    }


    public void SetCueDir(string dir) {
        var curRot = Cue.transform.rotation.eulerAngles;
        switch (dir) {
            case "left":
                // rotate rect transform 180 degrees
                Cue.transform.rotation = Quaternion.Euler(curRot.x, curRot.y, 0f);
                break;
            case "right":
                Cue.transform.rotation = Quaternion.Euler(curRot.x, curRot.y, 180f);
                break;
            default:
                Debug.LogError("Invalid direction: " + dir);
                break;
        }
    }
}
