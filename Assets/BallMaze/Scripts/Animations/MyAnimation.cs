using UnityEngine;

public delegate void AnimationEventHandler(MonoBehaviour sender);

public abstract class MyAnimation : MonoBehaviour
{

    protected enum State
    {
        IDLE,
        ANIMATING,
        REVERSEANIMATING,
        WAITING_FOR_ANIMATION,
        WAITING_FOR_REVERSEANIMATION,
    }

    protected State state = State.IDLE;
    protected float duration;
    protected float delay;
    private float realDuration;
    private float startingTime;
    private float lastCompletion = 1;

    public event AnimationEventHandler FinishedAnimating;

    public virtual void StartAnimating()
    {
        state = State.WAITING_FOR_ANIMATION;
        realDuration = InitRealDuration();
        startingTime = Time.time;
    }

    public virtual void StartReverseAnimating()
    {
        state = State.WAITING_FOR_REVERSEANIMATION;
        realDuration = InitRealDuration();
        startingTime = Time.time;
    }

    void Update()
    {
        float completion = -1;
        switch (state)
        {
            case State.IDLE:
                completion = -1;
                break;
            case State.WAITING_FOR_ANIMATION:
                CheckTime(State.ANIMATING);
                break;
            case State.WAITING_FOR_REVERSEANIMATION:
                CheckTime(State.REVERSEANIMATING);
                break;
            case State.ANIMATING:
                completion = GetAnimationCompletion();
                break;
            case State.REVERSEANIMATING:
                completion = 1 - GetAnimationCompletion();
                break;
            default:
                completion = -1;
                break;
        }
        if (completion == -1)
            return;
        else if (completion > 1 || completion < 0)
        {
            FinishAnimation();
            if (FinishedAnimating != null)
                FinishedAnimating.Invoke(this);
        }
        else
        {
            Animate(completion);
            lastCompletion = GetAnimationCompletion();
        }

    }

    private void CheckTime(State newState)
    {
        if (Time.time >= (startingTime + delay))
        {
            state = newState;
            Update();
        }
    }

    protected abstract void Animate(float completion);

    protected virtual void FinishAnimation()
    {
        if (state == State.ANIMATING)
            Animate(1);
        else if (state == State.REVERSEANIMATING)
            Animate(0);
        state = State.IDLE;
    }

    protected virtual float GetAnimationCompletion()
    {
        return (Time.time - (startingTime + delay) + (duration - realDuration)) / duration;
    }

    protected virtual float InitRealDuration()
    {
        if (lastCompletion > 1 || lastCompletion < 0)
            return duration;
        else
            return duration * lastCompletion;
    }

}

