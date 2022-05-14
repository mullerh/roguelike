public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerMovement playerMovement);

    public abstract void UpdateState(PlayerMovement playerMovement);

    // probably unused
    public abstract void CheckSwitchState(PlayerMovement playerMovement);

    // probably unused
    public abstract void InitializeSubState(PlayerMovement playerMovement);

    public abstract void FixedUpdateState(PlayerMovement playerMovement);

    public abstract void ExitState(PlayerMovement playerMovement);

    // ignore all these for now:
    void UpdateStates() {}

    void SwitchState() {}

    void SetSuperState() {}

    void SetSubState() {}
}
