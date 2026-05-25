using UnityEngine;

public enum State 
{
    Idle,
    Move,
    Die
}
public class PlayerStatus : MonoBehaviour
{
    PlayerCombatController playerCombatController;

    public float maxHp;
    public State state { get; private set; }

    private void Awake()
    {
        playerCombatController = GetComponent<PlayerCombatController>();
    }
    private void OnEnable()
    {
        playerCombatController.dieAction += SetDie;
    }
    private void OnDisable()
    {
        playerCombatController.dieAction -= SetDie;
    }

    public void SetDie()
    {
        state = State.Die;
    }
}
