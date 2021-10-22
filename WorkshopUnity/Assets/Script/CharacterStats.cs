using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStatistics", menuName ="CharacterStatisitcs", order =1)]
public class CharacterStats : ScriptableObject
{
    public float speed;
    public float angularSpeed;
    [Range(1,10)]
    public float jumpForce;
    public AnimationCurve horizontalSpeed;
    //jump
    public int maxJumpNumber;
    public float jumpCoolDown;
    [Range(1, 10)] public float jumpFactorMultiplicator;

    public float accelerationTriggerTreshold;
}
