using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueManager : MonoBehaviour
{

    string _currentDir = "left";    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleDir() {
        if (_currentDir == "left") {
                _currentDir = "right";
        } else {
             _currentDir = "left";
        }
        PointTo(_currentDir);
    }

    void ToggleCue() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
        } else {
            gameObject.SetActive(true);
        }
    }

    void PointTo(string dir) {
        switch (dir) {
            case "left":
                transform.rotation = Quaternion.Euler(0, 0, 210);
                break;
            case "right":
                transform.rotation = Quaternion.Euler(0, 0, 30);
                break;
            default:
                Debug.LogError("Invalid direction: " + dir);
                break;
        }
    }
}
