public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerMovement playerMovement);

    public abstract void UpdateState(PlayerMovement playerMovement);

    public abstract void CheckSwitchState(PlayerMovement playerMovement);

    public abstract void InitializeSubState(PlayerMovement playerMovement);

    public abstract void FixedUpdateState(PlayerMovement playerMovement);

    public abstract void ExitState(PlayerMovement playerMovement);

    void UpdateStates() {}

    void SwitchState() {}

    void SetSuperState() {}

    void SetSubState() {}
}
