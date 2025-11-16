using UnityEngine;

public class EnemyStateAttack : IEnemyState
{
    float timer;

    public void Enter(EnemyBase enemy)
    {
        timer = 0f;
        DoAttack(enemy);
        enemy.SetVelocity(Vector3.zero);
    }

    public void Exit(EnemyBase enemy)
    {
        enemy.SetVelocity(Vector3.zero);
    }

    public void Tick(EnemyBase enemy, float dt)
    {
        var sensor = enemy.sensor;

        timer += dt;

        // 항상 플레이어 쪽 바라보기
        if (sensor != null && sensor.target != null)
        {
            Vector3 toTarget = sensor.target.position - enemy.transform.position;
            toTarget.y = 0f;
            enemy.RotateTowards(toTarget, dt);
        }

        float cooldown = enemy.attackInterval;   // ← EnemyBase에서 값 가져오기

        if (timer >= cooldown)
        {
            timer = 0f;

            if (sensor != null && sensor.CanSeeTarget && enemy.IsTargetInAttackRange())
            {
                DoAttack(enemy);   // 같은 상태 안에서 다음 공격
            }
            else
            {
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
