using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;

public class LogRayCastHit : MonoBehaviour
{

    // Set up a raycast:
    private RaycastHit _hit;
    private Ray _ray;
    private int _layerMask;

    public GameObject HitPoint;

    // Start is called before the first frame update
    void Start()
    {
        _layerMask = ~LayerMask.GetMask("Ignore Raycast");
    }

    // Update is called once per frame
    void Update()
    {
        _layerMask = ~LayerMask.GetMask("Ignore Raycast");
        _ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(_ray, out _hit, 20.0f, _layerMask)) {
            HitPoint.transform.position = _hit.point;
        }
    }
}
