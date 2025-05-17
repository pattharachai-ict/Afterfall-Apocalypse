using UnityEngine;

public class SetBoolBehavior : StateMachineBehaviour
{
    public string boolName;
    public bool updateOnState;
    public bool updateOnStateMachine;
    public bool valueOnEnter, valueOnExit;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState && !string.IsNullOrEmpty(boolName))
        {
            animator.SetBool(boolName, valueOnEnter);
        }
        else if (updateOnState && string.IsNullOrEmpty(boolName))
        {
            Debug.LogWarning($"[SetBoolBehavior] Missing boolName on StateEnter in object '{animator.gameObject.name}'");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState && !string.IsNullOrEmpty(boolName))
        {
            animator.SetBool(boolName, valueOnExit);
        }
        else if (updateOnState && string.IsNullOrEmpty(boolName))
        {
            Debug.LogWarning($"[SetBoolBehavior] Missing boolName on StateExit in object '{animator.gameObject.name}'");
        }
    }

    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine && !string.IsNullOrEmpty(boolName))
        {
            animator.SetBool(boolName, valueOnEnter);
        }
        else if (updateOnStateMachine && string.IsNullOrEmpty(boolName))
        {
            Debug.LogWarning($"[SetBoolBehavior] Missing boolName on StateMachineEnter in object '{animator.gameObject.name}'");
        }
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine && !string.IsNullOrEmpty(boolName))
        {
            animator.SetBool(boolName, valueOnExit);
        }
        else if (updateOnStateMachine && string.IsNullOrEmpty(boolName))
        {
            Debug.LogWarning($"[SetBoolBehavior] Missing boolName on StateMachineExit in object '{animator.gameObject.name}'");
        }
    }
}
