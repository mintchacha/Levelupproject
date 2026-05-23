using UnityEngine;
using UnityEngine.InputSystem;


public class InputPrivider : MonoBehaviour
{
    private string gamePlayMapName = "Player";
    private string moveActionName = "Move";
    private string jumpActionName = "Jump";
    private string sprintActionName = "Sprint";
    private string lookPositionActionName = "Look";
    private string attackActionName = "Attack";

    public Vector2 lookInputValue { get; private set; }

    [Header("Action Asset")]
    [SerializeField] private InputActionAsset actionAsset;

    private InputActionMap inputActionMap;

    public InputAction moveAction { get; private set; }
    public InputAction sprintAction { get; private set; }
    public InputAction jumpAction { get; private set; }
    public InputAction lookAction { get; private set; }
    public InputAction attackAction { get; private set; }

    private void Awake()
    {
        if(actionAsset == null)
        {
            Debug.LogError("InputActionMap is not assigned in the inspector.");
            return;
        }
        EnsureInputActionsInitialized();
    }
    private void OnEnable()
    {
        inputActionMap.Enable();
        lookAction.performed += Focusing;
        lookAction.canceled += Focusing;
    }
    private void OnDisable()
    {
        lookAction.performed -= Focusing;
        lookAction.canceled -= Focusing;
        inputActionMap.Disable();
    }

    private void EnsureInputActionsInitialized()
    {
        inputActionMap = actionAsset.FindActionMap(gamePlayMapName, true);
        moveAction = inputActionMap.FindAction(moveActionName, true);
        jumpAction = inputActionMap.FindAction(jumpActionName, true);
        sprintAction = inputActionMap.FindAction(sprintActionName, true);
        lookAction = inputActionMap.FindAction(lookPositionActionName, true);
        attackAction = inputActionMap.FindAction(attackActionName, true);
    }
    private void Focusing(InputAction.CallbackContext context)
    {
        lookInputValue = context.ReadValue<Vector2>();
    }
}
