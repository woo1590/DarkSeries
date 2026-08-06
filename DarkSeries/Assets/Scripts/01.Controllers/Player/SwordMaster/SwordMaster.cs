using Unity.VisualScripting;
using UnityEngine;

public class SwordMaster : Player
{
    [field: Header("Animations")]
    [field: SerializeField] public SwordMasterAnimData animationData { get; private set; }

    [field: Header("Data")]
    [field: SerializeField] public PlayerData playerData { get; private set; }

    public SwordMasterAttackController attackController { get; private set; }
    private StateMachine<SwordMaster> stateMachine;
    private SwordMasterAnimationState currentAnimationState;

    public string currentStateName => stateMachine?.currState?.GetType().Name;
    public bool isAnimationSynchronized => animationData.IsCurrentState(
        animator,
        currentAnimationState,
        attackController.slashComboIndex);
    public bool isCurrentAnimationFinished => animationData.IsCurrentStateFinished(
        animator,
        currentAnimationState,
        attackController.slashComboIndex);

    protected override void Awake()
    {
        base.Awake();

        attackController = GetComponent<SwordMasterAttackController>();
        animationData.Initialize();
    }

    protected override void Start()
    {
        InitializeStates();
    }

    protected override void Update()
    {
        inputController.HandleInput();
        stateMachine.Update();
    }

    protected override void LateUpdate()
    {
        stateMachine.LateUpdate();
        EnsureAnimationSynchronization();
    }

    protected override void FixedUpdate()
    {
        stateMachine.FixedUpdate();

        float direction = inputController.movementInput.x;

        moveController.Move(direction);
    }

    void InitializeStates()
    {
        stateMachine = new StateMachine<SwordMaster>(this);

        stateMachine.AddState(new SwordMaster_Idle(stateMachine));
        stateMachine.AddState(new SwordMaster_Walk(stateMachine));
        stateMachine.AddState(new SwordMaster_SlashAttack(stateMachine));
        stateMachine.AddState(new SwordMaster_Jump(stateMachine));
        stateMachine.AddState(new SwordMaster_JumpToFall(stateMachine));
        stateMachine.AddState(new SwordMaster_Fall(stateMachine));
        stateMachine.AddState(new SwordMaster_Land(stateMachine));
        stateMachine.AddState(new SwordMaster_CrouchStart(stateMachine));
        stateMachine.AddState(new SwordMaster_CrouchHold(stateMachine));
        stateMachine.AddState(new SwordMaster_CrouchEnd(stateMachine));

        stateMachine.ChangeState<SwordMaster_Idle>();
    }

    public void OnAnimationEnd()
    {
        if (!isAnimationSynchronized || !isCurrentAnimationFinished)
            return;

        stateMachine.OnAnimationEnd();
    }

    public void SyncAnimation(SwordMasterAnimationState animationState, bool restartAnimation = true)
    {
        currentAnimationState = animationState;

        int comboIndex = attackController.slashComboIndex;
        animationData.ApplyParameters(animator, animationState, comboIndex);

        int stateHash = animationData.GetStateHash(animationState, comboIndex);
        if (stateHash == 0)
            return;

        if (!animator.HasState(0, stateHash))
        {
            Debug.LogError($"Animator state for {animationState} does not exist.", this);
            return;
        }

        if (restartAnimation || !animationData.IsCurrentState(animator, animationState, comboIndex))
            animator.Play(stateHash, 0, 0f);
    }

    private void EnsureAnimationSynchronization()
    {
        if (currentAnimationState == SwordMasterAnimationState.None || isAnimationSynchronized)
            return;

        SyncAnimation(currentAnimationState);
    }

    /* Debug */
    private void OnGUI()
    {
        if (stateMachine?.currState == null)
            return;

        string currStateName = stateMachine.currState.GetType().Name;
        Rect rect = new Rect(10f, 10f, 500f, 100f);

        Color prevBackground = GUI.backgroundColor;
        Color prevContent = GUI.contentColor;

        GUI.backgroundColor = Color.green;
        GUI.Box(rect, GUIContent.none);

        GUI.contentColor = Color.black;

        GUIStyle textStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 24,
            fontStyle = FontStyle.Bold
        };

        GUI.Label(rect, currStateName, textStyle);

        GUI.backgroundColor = prevBackground;
        GUI.contentColor = prevContent;
    }
}
