using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("Sight Settings")]
    public float SightRadius = 10f;
    [Range(0, 360)]
    public float SightAngle = 90;
    [Header("Layer")]
    public LayerMask TargetMask;
    public LayerMask Obstaclemask;

    List<Transform> _visibleTargets = new List<Transform>();
    Transform _nearestTarget;
    float _distanceToTarget;

    public Transform NearestTarget => _nearestTarget;

    void Start()
    {
        TargetMask = LayerMask.GetMask(Define.Player);
        Obstaclemask = LayerMask.GetMask(Define.Obstacle);

        StartCoroutine(FindTargetsWithDelay());
    }

    IEnumerator FindTargetsWithDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        _distanceToTarget = 0;
        _nearestTarget = null;
        _visibleTargets.Clear();

        // SightRadius ���� ���� TargetMask ���̾ ���� ������Ʈ ��� ã��
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, SightRadius, TargetMask);


        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            // ���� ���ϴ� ���⺤��
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // �� ���� ������ ���� ���ϴ� �Լ�
            if (Vector3.Angle(transform.forward, dirToTarget) < SightAngle / 2) // 0 ~ 180�� ���� �����ϹǷ� ������ ������
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                // Obstaclemask ���̾ ���� ��ü�� ���� ���� �� ��������
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, Obstaclemask))
                {
                    _visibleTargets.Add(target);

                    // ���� ����� Ÿ������ ������ ���� ���ų�, ���� ���� ����� �������� �Ÿ����� ���ο� Ÿ���� �Ÿ��� �� ª����
                    if (_nearestTarget == null || _distanceToTarget > dstToTarget)
                    {
                        _nearestTarget = target;
                    }
                    _distanceToTarget = dstToTarget;
                }
            }
        }
    }

    // ������ ���� ���� ����
    public Vector3 DirFromAngle(float angleInDegree, bool angleIsGlobal)
    {
        // global���� y�� �������� ȸ���� ��ŭ ���ؼ� �������־�� �Ѵ�.
        if (!angleIsGlobal)
        {
            angleInDegree += transform.eulerAngles.y;
        }
        Vector3 angleDir = new Vector3(Mathf.Sin(angleInDegree * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegree * Mathf.Deg2Rad));
        return angleDir;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, SightRadius);

        Vector3 viewAngleA = DirFromAngle(-SightAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(SightAngle / 2, false);

        float x = Mathf.Sin(SightAngle / 2 * Mathf.Deg2Rad) * SightRadius;
        float y = Mathf.Cos(SightRadius / 2 * Mathf.Deg2Rad) * SightRadius;

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * SightRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * SightRadius);

        Gizmos.color = Color.red;
        foreach(Transform visibletarget in _visibleTargets)
        {
            Gizmos.DrawLine(transform.position, visibletarget.position);
        }
    }
}
