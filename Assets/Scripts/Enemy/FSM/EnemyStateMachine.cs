using System.Collections.Generic;

public class EnemyStateMachine
{
    readonly Dictionary<EnemyStateId, IEnemyState> states =
        new Dictionary<EnemyStateId, IEnemyState>();

    public IEnemyState CurrentState { get; private set; }
    public EnemyStateId CurrentId { get; private set; }

    public void Register(EnemyStateId id, IEnemyState state)
    {
        states[id] = state;
    }

    public void Initialize(EnemyBase enemy, EnemyStateId startId)
    {
        CurrentId = startId;
        CurrentState = states[startId];
        CurrentState.Enter(enemy);
    }

    public void ChangeState(EnemyBase enemy, EnemyStateId newId)
    {
        if (newId == CurrentId) return;
        CurrentState?.Exit(enemy);
        CurrentId = newId;
        CurrentState = states[newId];
        CurrentState.Enter(enemy);
    }

    public void Tick(EnemyBase enemy, float deltaTime)
    {
        CurrentState?.Tick(enemy, deltaTime);
    }
}
