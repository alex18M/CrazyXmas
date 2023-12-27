using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;

public class PlayerAnimations : MonoBehaviour
{
    private const string WALK = "Walk";
    private const string IDLE = "Idle";
    
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject playerMovement;
    
    private BaseCharacterController _characterController;
    
    private void Awake()
    {
        _animator.GetComponent<Animator>();
        _characterController = GetComponent<BaseCharacterController>();
        
    }

    private void Update()
    {
        _animator.SetTrigger(_characterController.movement.velocity.magnitude > 0.1f ? WALK : IDLE);
    }
}
