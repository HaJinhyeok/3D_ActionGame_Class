using UnityEngine;

public class GolemStateMachineBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        int comboCount = animator.GetInteger(Define.ComboCount);
        if (stateInfo.normalizedTime > 0.9f)
        {
            comboCount = comboCount < 2 ? comboCount + 1 : 0;
            animator.SetInteger(Define.ComboCount, comboCount);
        }
    }
}
