using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBoolZombie : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ZombieManager zombie = animator.GetComponent<ZombieManager>();

        if(zombie != null)
        {
            zombie.isPerformingAction = false;
        }
    }
}
