using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera mainCamera;
    public float minDistance = 8;
    public float offset = 3;
    private float initScale;

    private void Start()
    {
        initScale = transform.localScale.x;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("CameraUI").GetComponent<Camera>();
        }
        else
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
            transform.localScale = CalculateSizeCanvas();
        }
    }

    private Vector3 CalculateSizeCanvas()
    {
        float distance = Mathf.Abs(mainCamera.transform.position.z - transform.position.z) + offset;//Vector3.Distance(camera.transform.position, transform.position);
        float total = (initScale * distance) / minDistance;
        return new Vector3(total, total, total);
    }
}
