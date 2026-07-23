using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [field: SerializeField][field: Range(0f, 25f)] public float baseSpeed { get; private set; } = 5f;
    [field: SerializeField][field: Range(0f, 10f)] public float baseJumpPower { get; private set; } = 5f;

    [field: Header("Idle Data")]

    [field: Header("Walk Data")]
    [field: SerializeField][field: Range(0f, 2f)] public float walkSpeedModifier { get; private set; } = 0.225f;

    [field: Header("Run Data")]
    [field: SerializeField]
    [field: Range(0f, 2f)] public float runSpeedModifier { get; private set; } = 1f;

    [field: Header("Jump Data")]
    [field: SerializeField]
    [field: Range(0f, 10f)] public float jumpPowerModifier { get; private set; } = 1f;
}
