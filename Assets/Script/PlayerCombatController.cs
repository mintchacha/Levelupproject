using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : MonoBehaviour
{
    PlayerMove playerMove;
    InputPrivider inputProvider;

    public event Action attackEvent;
    private void Awake()
    {
        inputProvider = GetComponent<InputPrivider>();
        playerMove = GetComponent<PlayerMove>();
    }
    private void Start()
    {
        inputProvider.attackAction.performed += Attack;
    }
    private void OnDisable()
    {
        inputProvider.attackAction.performed -= Attack;
    }

    private void Attack(InputAction.CallbackContext context) 
    {
        attackEvent?.Invoke();
        playerMove.DashForWard(1.5f);
    }
}
