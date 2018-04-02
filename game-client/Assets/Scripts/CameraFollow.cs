using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public float smoothing = 5f;
    public InputField playerNameInput;
    private Transform target;
    private Vector3 offset;

    public void InitializateCamera()
    {
        this.target = (GameObject.Find(this.playerNameInput.text) as GameObject).transform;
        this.offset = transform.position - this.target.position;
    }

    private void FixedUpdate()
    {
        if(this.target != null)
        {
            Vector3 targetCamPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, this.smoothing * Time.deltaTime);
        }
    }
}
