using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class TargetManager : MonoBehaviour
{

    public GameObject FrameRight;
    public GameObject FrameLeft;
    [HideInInspector]
    public GameObject FrameDistractorLeft;
    [HideInInspector]
    public GameObject FrameDistractorRight;

    public List<GameObject> targetObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 GetTargetPosition(string dir) {
        FrameManager curFrameManager = null;
        switch (dir) {
            case "left":
                curFrameManager = FrameLeft.GetComponent<FrameManager>();
                break;
            case "right":
                curFrameManager = FrameRight.GetComponent<FrameManager>();
                break;
            default:
                Debug.LogError("Invalid direction: " + dir);
                break;
        }
        return curFrameManager.gameObject.GetNamedChild("SpawnPoint").transform.position;
    }


    public void SetDistractorPosition(string dir, float xOffset, float yOffset=0, float zOffset=0) {
        GameObject curDistractor = null;
        switch (dir) {
            case "left":
                curDistractor = FrameDistractorLeft;
                // We want to offset always towards the fix cross
                //  and scene is rotated by 180 degree, so we 
                // switch dir for distractors on the left
                xOffset *= -1;
                break;
            case "right":
                curDistractor = FrameDistractorRight;
                break;
            default:
                Debug.LogError("Invalid direction: " + dir);
                break;
        }
        Vector3 targPos = GetTargetPosition(dir);
        curDistractor.transform.position = new Vector3(targPos.x + xOffset, targPos.y + yOffset, targPos.z + zOffset);
    }


    public void ToggleHighlightFrame(string dir) {
        FrameManager _curFrameManager = null;
        switch (dir) {
            case "left":
                _curFrameManager = FrameLeft.GetComponent<FrameManager>();
                break;
            case "right":
                _curFrameManager = FrameRight.GetComponent<FrameManager>();
                break;
            default:
                Debug.LogError("Invalid direction: " + dir);
                break;
        }
        _curFrameManager.ToggleHighlight();
        return;
    }

    public void ShowObject(bool show, string type, string dir) {
        if (type != "target" && type != "distractor") {
            Debug.LogError("Invalid type: " + type);
            return;
        }
        FrameManager _curFrameManager = null;
        switch (dir) {
            case "left":
                var fl = (type == "target") ? FrameLeft : FrameDistractorLeft;
                _curFrameManager = fl.GetComponent<FrameManager>();
                break;
            case "right":
                var fr = (type == "target") ? FrameRight : FrameDistractorRight;
                _curFrameManager = fr.GetComponent<FrameManager>();
                break;
            default:
                Debug.LogError("Invalid direction: " + dir);
                break;
        }
        if (show)
            _curFrameManager.SpawnCurrentTarget();
        _curFrameManager.ShowCurrentTarget(show);
        if (type == "target")
            _curFrameManager.ActivateCollider(show);
        return;
    }

    public void SetObject(int objIdx, string type, string dir){
        if (type != "target" && type != "distractor") {
            Debug.LogError("Invalid type: " + type);
            return;
        }
        FrameManager _curFrameManager = null;
        switch (dir) {
            case "left":
                var fl = (type == "target") ? FrameLeft : FrameDistractorLeft;
                _curFrameManager = fl.GetComponent<FrameManager>();
                break;
            case "right":
                var fr = (type == "target") ? FrameRight : FrameDistractorRight;
                _curFrameManager = fr.GetComponent<FrameManager>();
                break;
            default:
                Debug.LogError("Invalid direction: " + dir);
                break;
        }
        _curFrameManager.SetCurrentObject(targetObjects[objIdx]);
        return;
    }

}
