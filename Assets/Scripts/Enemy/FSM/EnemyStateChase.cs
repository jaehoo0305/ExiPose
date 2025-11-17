using UnityEngine;

public class EnemyStateChase : IEnemyState
{
    public void Enter(EnemyBase enemy)
    {
    }

    public void Exit(EnemyBase enemy)
    {
        // keep last velocity
    }

    public void Tick(EnemyBase enemy, float dt)
    {
        var sensor = enemy.sensor;
        if (sensor == null || sensor.target == null)
        {
            enemy.ChangeState(EnemyStateId.Patrol);
            return;
        }

        bool recentlySeen = sensor.HasRecentlySeenTarget(1.0f);
        if (!recentlySeen)
        {
            enemy.ChangeState(EnemyStateId.Patrol);
            return;
        }

        // distance
        Vector3 toTarget = sensor.target.position - enemy.transform.position;
        toTarget.y = 0f;
        float dist = toTarget.magnitude;

        // Use attackEnterRange for entering attack state
        if (dist <= enemy.attackEnterRange)
        {
            enemy.ChangeState(EnemyStateId.Attack);
            return;
        }

        // Otherwise, chase
        enemy.MoveSeek(sensor.target.position, enemy.chaseSpeed);
    }
}
