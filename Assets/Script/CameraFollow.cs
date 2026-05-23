using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private InputPrivider inputProvider;
    [SerializeField] private Transform followTarget;
    private Camera cam;

    [Header("캐릭터와의 거리")]
    [SerializeField] private Vector3 followOffset = new Vector3(0, 2, -3);
    [SerializeField] private float distance = 3f;
    [Header("캐릭터와의 각도")]
    [SerializeField] private Vector3 angleOffset = new Vector3(0, 0, 0);
    private float positionSharpness = 20f;
    [Header("마우스 회전 속도")]
    [SerializeField] private float aimSpeed = 1f;

    //[SerializeField] bool playerLool = false;

    private void Awake()
    {
        if(followTarget == null)
        {
            Debug.Log("FollowTarget reference is missing!");
        }
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Vector2 lookfocus = inputProvider.lookInputValue;

        angleOffset.x -= lookfocus.y * aimSpeed;
        angleOffset.x = Mathf.Clamp(angleOffset.x, -40f, 70f);

        Vector3 camPosition = followTarget.position +
            followTarget.forward * followOffset.z 
            + followTarget.up * followOffset.y 
            + followTarget.right * followOffset.x;

        Vector3 direction = (followTarget.position - transform.position).normalized;
        Quaternion lookAngle = Quaternion.LookRotation(direction);
        Quaternion offsetAngle = Quaternion.Euler(angleOffset);        

        float smooth = 1 - Mathf.Exp(-positionSharpness * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, camPosition, smooth);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAngle * offsetAngle, smooth);

    }

}

