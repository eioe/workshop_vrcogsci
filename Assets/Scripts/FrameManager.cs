using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameManager : MonoBehaviour
{

    public GameObject TargetObject;
    public Material matHighlight;
    public Material _matDefault;
    public Light SpotLight;
    private bool _isHighlit = false; 
    private GameObject _curTarget;

    // Start is called before the first frame update
    void Start()
    {
        _matDefault = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleHighlight(){
        if (_isHighlit) {
            GetComponent<Renderer>().material = _matDefault;
            SpotLight.gameObject.SetActive(false);
            _isHighlit = false;
        } else {
            GetComponent<Renderer>().material = matHighlight;
            SpotLight.gameObject.SetActive(true);
            _isHighlit = true;
        }
    }

    public void ShowCurrentTarget(bool show){
        _curTarget.SetActive(show);
    }

    public void HideCurrentTarget(){
        _curTarget.SetActive(false);
    }

    public void ToggleTarget() {
        _curTarget.SetActive(!_curTarget.activeSelf);
    }

    public void SpawnCurrentTarget() {
        // Set the position of the CurrentTarget to the position of the FrameManager.
        Transform spwnPt;
        foreach (Transform child in transform)
        {
            if (child.name == "SpawnPoint")
            {
                spwnPt = child;
                _curTarget = Instantiate(_curTarget, spwnPt.position, spwnPt.rotation);
                _curTarget.SetActive(false);
            }
        }
    }


    public void ActivateCollider(bool activate) {
        GetComponent<CapsuleCollider>().enabled = activate;
    }


    public void SetCurrentObject(GameObject target)
    {
        _curTarget = target;
    }
}
