using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Scriptable Object/Player Data", order = int.MaxValue)]
public class PlayerData : ScriptableObject
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float dashForce;
    [SerializeField] public float jumpForce;
    [SerializeField] public float doubleJumpForce; 
    [SerializeField] public float knockbackForce;
    [SerializeField] public float speedAdjustmentOffsetInAir; // ���߿����� �ӵ� = �������� �ӵ� * �ش� ����
    [SerializeField] public float maxAngle; // �̵� ������ �ִ� ����
    [SerializeField] public float wallJumpPower;
}
