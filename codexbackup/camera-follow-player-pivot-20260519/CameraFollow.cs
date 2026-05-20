using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private PlayerManger playerManger;
    [SerializeField] private Transform followTarget;

    [Header("Camera position from player")]
    [SerializeField] private Vector3 targetOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] private float followDistance = 3f;
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 60f;
    [SerializeField] private float positionSharpness = 10f;

    [Header("Look sensitivity")]
    [SerializeField] private float aimSpeed = 1f;

    private float yaw;
    private float pitch = 15f;

    private void Awake()
    {
        ResolveReferences();

        Vector3 eulerAngles = transform.eulerAngles;
        yaw = eulerAngles.y;
        pitch = Mathf.Clamp(NormalizeAngle(eulerAngles.x), minPitch, maxPitch);
    }

    private void LateUpdate()
    {
        if (followTarget == null)
        {
            ResolveReferences();
        }

        if (followTarget == null)
        {
            return;
        }

        UpdateRotationInput();
        FollowTarget();
    }

    private void ResolveReferences()
    {
        if (playerManger == null)
        {
            playerManger = FindFirstObjectByType<PlayerManger>();
        }

        if (followTarget == null && playerManger != null)
        {
            followTarget = playerManger.transform;
        }

        if (followTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            followTarget = player != null ? player.transform : null;
        }

        if (followTarget == null)
        {
            GameObject unityChan = GameObject.Find("unitychan");
            followTarget = unityChan != null ? unityChan.transform : null;
        }
    }

    private void UpdateRotationInput()
    {
        if (playerManger == null || playerManger.inputProvider == null)
        {
            return;
        }

        Vector2 mouseVector = playerManger.inputProvider.lookInputValue;
        if (mouseVector == Vector2.zero)
        {
            return;
        }

        float mouseX = Mathf.Clamp01(mouseVector.x / Screen.width);
        float mouseY = Mathf.Clamp01(mouseVector.y / Screen.height);

        yaw = (mouseX - 0.5f) * 120f * aimSpeed;
        pitch = Mathf.Clamp((0.5f - mouseY) * 80f * aimSpeed, minPitch, maxPitch);
    }

    private void FollowTarget()
    {
        Vector3 pivotPosition = followTarget.position + targetOffset;
        Quaternion orbitRotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 targetPosition = pivotPosition - orbitRotation * Vector3.forward * followDistance;
        float smooth = 1f - Mathf.Exp(-positionSharpness * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smooth);
        transform.rotation = Quaternion.LookRotation(pivotPosition - transform.position, Vector3.up);
    }

    private float NormalizeAngle(float angle)
    {
        return angle > 180f ? angle - 360f : angle;
    }
}
