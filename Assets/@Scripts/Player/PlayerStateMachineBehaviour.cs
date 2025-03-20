using UnityEngine;

public class PlayerStateMachineBehaviour : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        float currentTime = stateInfo.normalizedTime;
        bool isNextCombo = animator.GetBool(Define.isNextCombo);

        // animation�� 70% �ʰ� ~ 90% �̸� ����ư� ���� ������ ������ ��
        if (currentTime < 0.9f && currentTime > 0.7 && isNextCombo)
        {
            int comboCount = animator.GetInteger(Define.ComboCount);
            comboCount = comboCount < 2 ? ++comboCount : 0;
            animator.SetInteger(Define.ComboCount, comboCount);
        }
        // animation�� 90% �̻� ����ƴٸ� ���� �޺� ���� �Ұ���
        if (currentTime >= 0.9f)
        {
            animator.SetInteger(Define.ComboCount, 0);
            animator.SetBool(Define.isAttacking, false);
            animator.SetBool(Define.isNextCombo, false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(Define.isNextCombo, false);
    }
}
