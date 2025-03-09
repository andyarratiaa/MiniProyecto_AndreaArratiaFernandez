using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    public PlayerManager playerManager;

    [Header("Hand IK Constrains")]
    public TwoBoneIKConstraint rightHandIK; //These constrains enable our character to hold the current weapon properly
    public TwoBoneIKConstraint leftHandIK;

    [Header("Aiming Constrains")]
    public MultiAimConstraint spine01; //These constrains turn the character toward the aiming target
    public MultiAimConstraint spine02;
    public MultiAimConstraint head;


    RigBuilder rigBuilder;

    float snappedHorizontal;
    float snappedVertical;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigBuilder = GetComponent<RigBuilder>();
        playerManager = GetComponent<PlayerManager>();
    }

    public void HandleAnimatorValues(float horizontalMovement, float verticalMovement, bool isRunning)
    {
        if (horizontalMovement > 0)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalMovement < 0)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }

        if (verticalMovement > 0)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }

        if (isRunning)
        {
            snappedVertical = 2;
        }

        animator.SetFloat("Horizontal", snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat("Vertical", snappedVertical, 0.1f, Time.deltaTime);
    }

    public void AssignHandIK(RightHandIKTarget rightTarget, LeftHandIKTarget leftTarget)
    {
        rightHandIK.data.target = rightTarget.transform;
        leftHandIK.data.target = leftTarget.transform;
        rigBuilder.Build();
    }

    //While aiming our character will turn towards the center of the screen
    public void UpdateAimConstraints()
    {
        if (playerManager.isAiming)
        {
            spine01.weight = 0.3f;
            spine02.weight = 0.3f;
            head.weight = 0.85f;
        }
        else
        {
            spine01.weight = 0f;
            spine02.weight = 0.3f;
            head.weight = 0f;
        }
    }

    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
    }
}



