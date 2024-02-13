using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerOriginManager : MonoBehaviour
{
    public GameObject XROrigin;
    // Start is called before the first frame update
    void Start() {
        transform.SetPositionAndRotation(XROrigin.transform.position, XROrigin.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
