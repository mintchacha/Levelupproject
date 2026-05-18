using UnityEngine;
using UnityEngine.InputSystem;

public class InputPrivider : MonoBehaviour
{
    public enum ControlDeviceKind
    {
        None,
        Keyboard,
        Gamepad,
        Other
    }

    public enum LookDeviceKind
    {
        None,
        Pointer,
        Gamepad,
        Other
    }

    private string gamePlayMapName = "Player";
    private string moveActionName = "Move";
    private string jumpActionName = "Jump";
    private string sprintActionName = "Sprint";
    private string lookPositionActionName = "Look";

    public Vector2 moveInputValue { get; private set; }
    public Vector2 lookInputValue { get; private set; }
    public bool jumpInputValue { get; private set; }
    public bool sprintInputValue { get; private set; }

    [Header("Action Asset")]
    [SerializeField] private InputActionAsset actionAsset;

    private InputActionMap inputActionMap;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    private void OnEnable()
    {
        if (!EnsureInputActionsInitialized()) return;
        inputActionMap.Enable();

        bindCallbacks();
    }
    private void OnDisable()
    {
        inputActionMap.Disable();
        UnbindCallbacks();
        ClearRuntimeState();
    }

    private void bindCallbacks() 
    { 
        moveAction.performed += MoveInput;
        moveAction.canceled += MoveInput;
        jumpAction.performed += JumpInput;
        sprintAction.performed += SprintInput;
        sprintAction.canceled += SprintInput;

        lookAction.performed += LookInput;
        lookAction.canceled += LookInput;
    }
    private void UnbindCallbacks()
    { 
        moveAction.performed -= MoveInput;
        moveAction.canceled -= MoveInput;
        jumpAction.performed -= JumpInput;
        sprintAction.performed -= SprintInput;
        sprintAction.canceled -= SprintInput;

        lookAction.performed -= LookInput;
        lookAction.canceled -= LookInput;
    }

    private bool EnsureInputActionsInitialized()
    {
        inputActionMap = actionAsset.FindActionMap(gamePlayMapName, true);
        moveAction = inputActionMap.FindAction(moveActionName, true);
        jumpAction = inputActionMap.FindAction(jumpActionName, true);
        sprintAction = inputActionMap.FindAction(sprintActionName, true);
        lookAction = inputActionMap.FindAction(lookPositionActionName, true);

        return true;
    }
    private void MoveInput(InputAction.CallbackContext context)
    {
        moveInputValue = context.ReadValue<Vector2>();
    }
    private void JumpInput(InputAction.CallbackContext context)
    {
        jumpInputValue = true;       
    }
    public void ConsumeJumpInput()
    {
        jumpInputValue = false;
    }
    private void SprintInput(InputAction.CallbackContext context)
    {
        sprintInputValue = context.ReadValue<float>() > 0;
    }
    private void ClearRuntimeState()
    {
        moveInputValue = Vector2.zero;
    }
    private void LookInput(InputAction.CallbackContext context)
    {
        lookInputValue = context.ReadValue<Vector2>();
    }
}
