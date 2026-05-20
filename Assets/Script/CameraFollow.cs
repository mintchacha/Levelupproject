using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private InputPrivider inputProvider;
    [SerializeField] private Transform followTarget;
    private Camera cam;

    [Header("캐릭터와의 거리")]
    [SerializeField] private Vector3 followOffset = new Vector3(0, 1, 3);
    //[Header("캐릭터와의 각도")]
    //[SerializeField] private Vector3 angleOffset = new Vector3(60f, 0, 0);
    private float positionSharpness = 10f;
    [Header("마우스 회전 속도")]
    [SerializeField] private float aimSpeed = 1f;
    


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
        Vector2 mouseVector = inputProvider.lookInputValue;

        float mouseX = mouseVector.x / Screen.width;
        float mouseY = mouseVector.y / Screen.height;

        float yaw = (mouseX - 0.5f) * 120f * aimSpeed;
        float pitch = (mouseY - 0.5f) * 80f * aimSpeed;

        gameObject.transform.rotation = Quaternion.Euler(-pitch, yaw, 0);
        if (followTarget != null)
        {
            Vector3 camPosition = followTarget.position + followOffset;

            float smooth = 1 - Mathf.Exp(-positionSharpness * Time.deltaTime);

            gameObject.transform.position = Vector3.Lerp(transform.position, camPosition, smooth);
            //gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(angleOffset), smooth);

            
        }
    }
}

