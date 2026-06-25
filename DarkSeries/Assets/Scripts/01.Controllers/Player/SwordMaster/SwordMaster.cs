using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SwordMaster : Player
{
    [field: Header("Animations")]
    [field: SerializeField] public SwordMasterAnimData animationData { get; private set; }

    [field: Header("Data")]
    [field: SerializeField]public PlayerData playerData { get; private set; }

    private StateMachine<SwordMaster> stateMachine;
    
    protected override void Awake()
    {
        base.Awake();

        animationData.Initialize();
        InitializeStates();
    }

    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        stateMachine.HandleInput();
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
        stateMachine.ChangeState<SwordMaster_Idle>();
    }
}
