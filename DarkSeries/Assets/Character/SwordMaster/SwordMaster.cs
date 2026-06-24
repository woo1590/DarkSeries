using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SwordMaster : MonoBehaviour
{
    [field: Header("Animations")]
    [field: SerializeField] public SwordMasterAnimData animationData { get; private set; }
    [field: Header("Data")]
    [field: SerializeField]public PlayerData playerData { get; private set; }
    public SpriteRenderer sprite {  get; private set; }
    public Animator animator { get; private set; }
    public Rigidbody rigid {  get; private set; }
    public PlayerInputController inputController { get; private set; }

    private void Awake()
    {
        animationData.Initialize();

        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();  
        inputController = GetComponent<PlayerInputController>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}
