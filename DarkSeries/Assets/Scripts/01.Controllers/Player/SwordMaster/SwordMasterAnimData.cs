using System;
using UnityEngine;

public enum SwordMasterAnimationState
{
    None,
    Idle,
    Walk,
    SlashAttack,
    Jump,
    JumpToFall,
    Fall,
    Land,
    CrouchStart,
    CrouchHold,
    CrouchEnd
}

[Serializable]
public class SwordMasterAnimData
{
    private const string baseLayer = "Base Layer.";
    private const string airStateMachine = baseLayer + "Air State.";
    private const string slashStateMachine = baseLayer + "Slash Attack.";

    [SerializeField] private string idleParamName = "Idle";
    [SerializeField] private string walkParamName = "Walk";
    [SerializeField] private string runParamName = "Run";
    [SerializeField] private string runFastParamName = "RunFast";
    [SerializeField] private string attackParamName = "Attack";
    [SerializeField] private string isGroundParamName = "IsGround";
    [SerializeField] private string startFallParamName = "StartFall";
    [SerializeField] private string fallParamName = "Fall";
    [SerializeField] private string landParamName = "Land";
    [SerializeField] private string crouchParamName = "Crouch";

    public int idleParamHash {  get; private set; }
    public int walkParamHash { get; private set ; }
    public int runParamHash { get; private set ; }
    public int runFastParamHash { get;private set ; }   
    public int attackParamHash { get; private set ; }
    public int isGroundParamHash { get; private set; }
    public int startFallParamHash { get; private set ; } 
    public int fallParamHash { get; private set ; } 
    public int landParamHash { get; private set ; } 
    public int crouchParamHash { get; private set ; }

    private int idleStateHash;
    private int walkStateHash;
    private int jumpStateHash;
    private int jumpToFallStateHash;
    private int fallStateHash;
    private int landStateHash;
    private int crouchStartStateHash;
    private int crouchEndStateHash;
    private readonly int[] slashStateHashes = new int[4];

    public void Initialize()
    {
        idleParamHash = Animator.StringToHash(idleParamName);
        walkParamHash = Animator.StringToHash(walkParamName);
        runParamHash = Animator.StringToHash(runParamName);
        runFastParamHash = Animator.StringToHash(runFastParamName);
        attackParamHash = Animator.StringToHash(attackParamName);
        isGroundParamHash = Animator.StringToHash(isGroundParamName);
        startFallParamHash = Animator.StringToHash(startFallParamName);
        fallParamHash = Animator.StringToHash(fallParamName);
        landParamHash = Animator.StringToHash(landParamName);
        crouchParamHash = Animator.StringToHash(crouchParamName);

        idleStateHash = Animator.StringToHash(baseLayer + "idle");
        walkStateHash = Animator.StringToHash(baseLayer + "walk");
        jumpStateHash = Animator.StringToHash(airStateMachine + "jump");
        jumpToFallStateHash = Animator.StringToHash(airStateMachine + "jump_to_fall");
        fallStateHash = Animator.StringToHash(airStateMachine + "fall");
        landStateHash = Animator.StringToHash(airStateMachine + "land");
        crouchStartStateHash = Animator.StringToHash(baseLayer + "crouch_start");
        crouchEndStateHash = Animator.StringToHash(baseLayer + "crouch_end");

        for (int index = 0; index < slashStateHashes.Length; index++)
            slashStateHashes[index] = Animator.StringToHash(slashStateMachine + $"slash{index + 1}");
    }

    public int GetStateHash(SwordMasterAnimationState state, int comboIndex)
    {
        return state switch
        {
            SwordMasterAnimationState.Idle => idleStateHash,
            SwordMasterAnimationState.Walk => walkStateHash,
            SwordMasterAnimationState.SlashAttack => slashStateHashes[Mathf.Clamp(comboIndex, 0, slashStateHashes.Length - 1)],
            SwordMasterAnimationState.Jump => jumpStateHash,
            SwordMasterAnimationState.JumpToFall => jumpToFallStateHash,
            SwordMasterAnimationState.Fall => fallStateHash,
            SwordMasterAnimationState.Land => landStateHash,
            SwordMasterAnimationState.CrouchStart => crouchStartStateHash,
            SwordMasterAnimationState.CrouchHold => crouchStartStateHash,
            SwordMasterAnimationState.CrouchEnd => crouchEndStateHash,
            _ => 0
        };
    }

    public void ApplyParameters(Animator animator, SwordMasterAnimationState state, int comboIndex)
    {
        animator.SetBool(idleParamHash, state == SwordMasterAnimationState.Idle);
        animator.SetBool(walkParamHash, state == SwordMasterAnimationState.Walk);
        animator.SetBool(attackParamHash, state == SwordMasterAnimationState.SlashAttack);
        animator.SetBool(startFallParamHash, state == SwordMasterAnimationState.JumpToFall);
        animator.SetBool(fallParamHash, state == SwordMasterAnimationState.Fall);
        animator.SetBool(crouchParamHash,
            state == SwordMasterAnimationState.CrouchStart || state == SwordMasterAnimationState.CrouchHold);
        animator.SetBool(isGroundParamHash, IsGroundState(state));

        if (state == SwordMasterAnimationState.SlashAttack)
            animator.SetInteger(Animator.StringToHash("ComboIndex"), comboIndex);
    }

    public bool IsCurrentState(Animator animator, SwordMasterAnimationState state, int comboIndex)
    {
        int expectedHash = GetStateHash(state, comboIndex);

        if (expectedHash == 0)
            return true;

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.fullPathHash == expectedHash)
            return true;

        return animator.IsInTransition(0) && animator.GetNextAnimatorStateInfo(0).fullPathHash == expectedHash;
    }

    public bool IsCurrentStateFinished(Animator animator, SwordMasterAnimationState state, int comboIndex)
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        int expectedHash = GetStateHash(state, comboIndex);

        if (currentState.fullPathHash == expectedHash && currentState.normalizedTime >= 0.95f)
            return true;

        if (!animator.IsInTransition(0))
            return false;

        AnimatorStateInfo nextState = animator.GetNextAnimatorStateInfo(0);
        return nextState.fullPathHash == expectedHash && nextState.normalizedTime >= 0.95f;
    }

    private static bool IsGroundState(SwordMasterAnimationState state)
    {
        return state is SwordMasterAnimationState.Idle
            or SwordMasterAnimationState.Walk
            or SwordMasterAnimationState.Land
            or SwordMasterAnimationState.CrouchStart
            or SwordMasterAnimationState.CrouchHold
            or SwordMasterAnimationState.CrouchEnd
            or SwordMasterAnimationState.SlashAttack;
    }
}
