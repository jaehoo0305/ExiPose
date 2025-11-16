using UnityEngine;

public class EnemyStatePatrol : IEnemyState
{
    public void Enter(EnemyBase enemy)
    {
        enemy.SetVelocity(Vector3.zero);
    }

    public void Exit(EnemyBase enemy)
    {
    }

    public void Tick(EnemyBase enemy, float dt)
    {
        if (enemy.sensor != null && enemy.sensor.CanSeeTarget)
        {
            enemy.ChangeState(EnemyStateId.Chase);
            return;
        }

        Transform point = enemy.GetCurrentPatrolPoint();
        if (point == null)
        {
            enemy.SetVelocity(Vector3.zero);
            return;
        }

        enemy.MoveArrive(point.position, enemy.patrolSpeed, 0.8f);

        float dist = Vector3.Distance(enemy.transform.position, point.position);
        if (dist <= 0.5f)
        {
            enemy.NextPatrolPoint();
        }
    }
}
