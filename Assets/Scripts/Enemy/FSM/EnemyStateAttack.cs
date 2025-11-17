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

        // face target
        if (sensor != null && sensor.target != null)
        {
            Vector3 toTarget = sensor.target.position - enemy.transform.position;
            toTarget.y = 0f;
            enemy.RotateTowards(toTarget, dt);
        }

        if (timer < enemy.attackInterval)
            return;

        timer = 0f;

        bool inRange = enemy.IsTargetInAttackRange();              // keep range »ç¿ë
        bool recentlySeen = sensor != null && sensor.HasRecentlySeenTarget(0.7f);

        if (recentlySeen && inRange)
        {
            // stay in this state and attack again
            DoAttack(enemy);
        }
        else if (recentlySeen && !inRange)
        {
            // still know player, but too far ¡æ chase
            enemy.ChangeState(EnemyStateId.Chase);
        }
        else
        {
            // lost player for a while ¡æ patrol
            enemy.ChangeState(EnemyStateId.Patrol);
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
