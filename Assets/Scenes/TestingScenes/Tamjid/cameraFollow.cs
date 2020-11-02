
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;

    public Vector3 offset;

    //same as update but runs right after
    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
