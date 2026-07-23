using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SwordMaster : Player
{
    [field: Header("Animations")]
    [field: SerializeField] public SwordMasterAnimData animationData { get; private set; }

    [field: Header("Data")]
    [field: SerializeField] public PlayerData playerData { get; private set; }

    public SwordMasterAttackController attackController { get; private set; }
    private StateMachine<SwordMaster> stateMachine;
    
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
    }

    protected override void FixedUpdate()
    {
        stateMachine.FixedUpdate();
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

        stateMachine.ChangeState<SwordMaster_Idle>();
    }

    public void OnAnimationEnd()
    {
        stateMachine.currState.OnAnimationEnd();
    }
}
