// EnemyStateAttack.cs
using UnityEngine;

public class EnemyStateAttack : IEnemyState
{
    float timer;

    public void Enter(EnemyBase enemy)
    {
        timer = 0f;
        DoAttack(enemy);                 // 들어오자마자 1타
        enemy.SetVelocity(Vector3.zero); // 공격 상태에서는 기본적으로 멈춤
    }

    public void Exit(EnemyBase enemy)
    {
        enemy.SetVelocity(Vector3.zero); // 상태 나갈 때도 속도 정리
    }

    public void Tick(EnemyBase enemy, float dt)
    {
        var sensor = enemy.sensor;

        timer += dt;

        // 항상 플레이어 쪽 바라보게 회전
        if (sensor != null && sensor.target != null)
        {
            Vector3 toTarget = sensor.target.position - enemy.transform.position;
            toTarget.y = 0f;
            enemy.RotateTowards(toTarget, dt);
        }

        float cooldown = enemy.attackInterval; // EnemyBase에서 설정한 간격 사용

        if (timer >= cooldown)
        {
            timer = 0f;

            // 아직도 보고 있고 사거리 안이면 같은 상태에서 또 공격
            if (sensor != null && sensor.CanSeeTarget && enemy.IsTargetInAttackRange())
            {
                DoAttack(enemy);  // Attack 상태 유지 + 한 번 더 휘두름
            }
            else
            {
                // 시야나 사거리 벗어나면 추격으로 복귀
                enemy.ChangeState(EnemyStateId.Chase);
            }
        }
    }

    void DoAttack(EnemyBase enemy)
    {
        if (enemy.animator != null)
        {
            enemy.animator.SetTrigger("Attack");
        }
    }
}
