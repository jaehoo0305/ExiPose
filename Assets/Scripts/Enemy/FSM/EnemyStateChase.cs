using UnityEngine;

public class EnemyStateChase : IEnemyState
{
    public void Enter(EnemyBase enemy)
    {
    }

    public void Exit(EnemyBase enemy)
    {
        enemy.SetVelocity(Vector3.zero);
    }

    public void Tick(EnemyBase enemy, float dt)
    {
        var sensor = enemy.sensor;
        if (sensor == null || sensor.target == null)
        {
            enemy.ChangeState(EnemyStateId.Patrol);
            return;
        }

        if (!sensor.CanSeeTarget)
        {
            enemy.ChangeState(EnemyStateId.Patrol);
            return;
        }

        if (enemy.IsTargetInAttackRange())
        {
            enemy.ChangeState(EnemyStateId.Attack);
            return;
        }

        enemy.MoveSeek(sensor.target.position, enemy.chaseSpeed);
    }
}
