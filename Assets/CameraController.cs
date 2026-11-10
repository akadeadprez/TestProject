using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject followThisGameObject;
    public Vector3 offset;

    void LateUpdate()
    {
        if (followThisGameObject == null) return;

        var target = followThisGameObject.transform;
        transform.position = target.position + offset;
        transform.LookAt(target); // new: keep the sphere centered in the view
    }
}