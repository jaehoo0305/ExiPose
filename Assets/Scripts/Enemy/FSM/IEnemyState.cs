using UnityEngine;

public interface IEnemyState
{
    void Enter(EnemyBase enemy);
    void Exit(EnemyBase enemy);
    void Tick(EnemyBase enemy, float deltaTime);
}
