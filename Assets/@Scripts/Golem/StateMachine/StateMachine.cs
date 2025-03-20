using UnityEngine;
using System.Collections.Generic;
using System;
public abstract class State<T>
{
    protected StateMachine<T> stateMachine;
    protected T context;
    protected int mechanimStateHash;

    public State() { }
    public State(int mechanimStateHash)
    {
        this.mechanimStateHash = mechanimStateHash;
    }
    public State(string mechanimStateName) : this(Animator.StringToHash(mechanimStateName)) { }

    public void SetMachine(StateMachine<T> stateMachine, T context)
    {
        this.stateMachine = stateMachine;
        this.context = context;

        OnInitialized();
    }

    public abstract void OnInitialized();
    public virtual void OnEnter() { }
    public virtual void OnUpdate(float deltaTime) { }
    public virtual void OnFixedUpdate(float deltaTime) { }
    public virtual void OnExit() { }

}

public class StateMachine<T>
{
    private T context;
    public event System.Action OnChangedState;

    private State<T> currentState;
    private State<T> prevState;
    private float elapsedTime; // 현재 상태에서 경과된 시간

    public State<T> CurrentState => currentState;
    public State<T> PrevState => prevState;
    public float ElapsedTime => elapsedTime;

    // 상태들을 관리하는 딕셔너리
    private Dictionary<System.Type, State<T>> states = new Dictionary<Type, State<T>>();
    public StateMachine(T context, State<T> state)
    {
        this.context = context;
        AddState(state);
        currentState = state;
        currentState.OnEnter();
    }

    public void AddState(State<T> state)
    {
        state.SetMachine(this, context);
        states[state.GetType()] = state;
    }

    public void OnUpdate(float deltaTime)
    {
        elapsedTime += deltaTime;
        currentState.OnUpdate(elapsedTime);
    }

    public void OnFixedUpdate(float deltaTime)
    {
        currentState.OnFixedUpdate(deltaTime);
    }

    public R ChangeState<R>() where R : State<T>
    {
        var newType = typeof(R);
        if (currentState.GetType() == newType)
        {
            return currentState as R;
        }
        if (currentState != null)
        {
            currentState.OnExit();
        }
        prevState = currentState;
        currentState = states[newType];
        currentState.OnEnter();
        elapsedTime = 0;

        OnChangedState?.Invoke();
        return currentState as R;
    }
}
