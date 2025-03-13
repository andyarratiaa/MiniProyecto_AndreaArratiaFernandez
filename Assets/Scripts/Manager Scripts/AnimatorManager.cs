using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    PlayerLocomotionManager playerLocomotionManager;
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
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        rigBuilder = GetComponent<RigBuilder>();
        playerManager = GetComponent<PlayerManager>();
    }

    public void PlayAnimationWithOutRootMotion(string targetAnimation, bool isPreformingAction)
    {
        animator.SetBool("isPreformingAction", isPreformingAction);
        animator.SetBool("disableRootMotion", true);
        animator.applyRootMotion = false;
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void PlayAnimation(string targetAnimation, bool isPreformingAction)
    {
        animator.SetBool("isPreformingAction", isPreformingAction);
        animator.SetBool("disableRootMotion", true);
        animator.CrossFade(targetAnimation, 0.2f);
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

    private void OnAnimatorMove()
    {
        if (playerManager.disableRootMotion)
            return;

        Vector3 animatorDeltaPosition = animator.deltaPosition;
        animatorDeltaPosition.y = 0;

        Vector3 velocity = animatorDeltaPosition / Time.deltaTime;
        playerLocomotionManager.playerRigidbody.drag = 0;
        playerLocomotionManager.playerRigidbody.velocity = velocity;
        transform.rotation *= animator.deltaRotation;
    }

    public void AssignHandIK(RightHandIKTarget rightTarget, LeftHandIKTarget leftTarget)
    {
        rightHandIK.data.target = rightTarget.transform;
        leftHandIK.data.target = leftTarget.transform;
        rigBuilder.Build();
    }

    public void ClearHandIKWeights()
    {
        rightHandIK.data.targetPositionWeight = 0;
        rightHandIK.data.targetRotationWeight = 0;

        leftHandIK.data.targetPositionWeight = 0;
        leftHandIK.data.targetRotationWeight = 0;

    }

    public void RefreshHandIKWeights()
    {
        rightHandIK.data.targetPositionWeight = 1;
        rightHandIK.data.targetRotationWeight = 1;

        leftHandIK.data.targetPositionWeight = 1;
        leftHandIK.data.targetRotationWeight = 1;
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



