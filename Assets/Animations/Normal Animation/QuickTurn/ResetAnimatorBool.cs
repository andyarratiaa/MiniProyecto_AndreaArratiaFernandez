using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    [Header("Disable Root Motion")]
    public string disableRootMotion = "disableRootMotion";
    public bool disableRootMotionStatus = false;

    [Header("Is Preforming Action Bool")]
    public string isPreformingAction = "isPreformingAction";
    public bool isPreformingActionStatus = false;

    [Header("Is Preforming Quick Turn")]
    public string isPreformingQuickTurn = "isPreformingQuickTurn";
    public bool isPreformingQuickTurnStatus = false;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(disableRootMotion, disableRootMotionStatus);
        animator.SetBool(isPreformingAction, isPreformingActionStatus);
        animator.SetBool(isPreformingQuickTurn, isPreformingQuickTurnStatus);
    }
}


