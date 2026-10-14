using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private float lookAtHeight = 1.5f;

    [Header("Camera Distance")]
    [SerializeField] private float distance = 6f;

    [Header("Follow Settings")]
    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private float rotationSpeed = 8f;

    [Header("Mouse Look X")]
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float minXAngle = 10f;
    [SerializeField] private float maxXAngle = 60f;

    private Vector3 positionVelocity;
    private float currentXAngle;
    private float currentYAngle;

    private void Start()
    {
        if (target == null)
            return;

        Vector3 angles = transform.eulerAngles;
        currentXAngle = angles.x;
        currentYAngle = angles.y;

        if (currentXAngle > 180f)
            currentXAngle -= 360f;

        currentXAngle = Mathf.Clamp(currentXAngle, minXAngle, maxXAngle);
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        currentXAngle -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        currentXAngle = Mathf.Clamp(currentXAngle, minXAngle, maxXAngle);
        Vector3 flatForward = Vector3.ProjectOnPlane(target.forward, Vector3.up).normalized;

        if (flatForward.sqrMagnitude > 0.001f)
        {
            float targetYAngle = Quaternion.LookRotation(flatForward).eulerAngles.y;
            currentYAngle = Mathf.LerpAngle(currentYAngle, targetYAngle, rotationSpeed * Time.deltaTime);
        }

        Quaternion cameraRotation = Quaternion.Euler(currentXAngle, currentYAngle, 0f);
        Vector3 pivot = target.position + Vector3.up * lookAtHeight;
        Vector3 desiredPosition = pivot - cameraRotation * Vector3.forward * distance;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref positionVelocity, smoothTime);
        transform.rotation = cameraRotation;
    }
}