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

        // SightRadius 범위 내의 TargetMask 레이어를 지닌 오브젝트 모두 찾기
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, SightRadius, TargetMask);


        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            // 적을 향하는 방향벡터
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // 두 벡터 사이의 각을 구하는 함수
            if (Vector3.Angle(transform.forward, dirToTarget) < SightAngle / 2) // 0 ~ 180도 값만 리턴하므로 반으로 나눠줌
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                // Obstaclemask 레이어를 가진 물체에 의해 빛이 안 막혔으면
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, Obstaclemask))
                {
                    _visibleTargets.Add(target);

                    // 가장 가까운 타겟으로 설정된 적이 없거나, 현재 가장 가까운 적까지의 거리보다 새로운 타겟의 거리가 더 짧으면
                    if (_nearestTarget == null || _distanceToTarget > dstToTarget)
                    {
                        _nearestTarget = target;
                    }
                    _distanceToTarget = dstToTarget;
                }
            }
        }
    }

    // 각도에 대한 방향 벡터
    public Vector3 DirFromAngle(float angleInDegree, bool angleIsGlobal)
    {
        // global에서 y축 기준으로 회전한 만큼 더해서 생각해주어야 한다.
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
