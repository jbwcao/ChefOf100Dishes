using UnityEngine;

public class ParalaxingBackground : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxFactor = 0.5f;
    private Vector3 lastCameraPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        lastCameraPosition = cameraTransform.position;
    }   
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(
            deltaMovement.x * parallaxFactor, deltaMovement.y * parallaxFactor, 0f);

        lastCameraPosition = cameraTransform.position;
    }
}
