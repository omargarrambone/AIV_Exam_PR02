using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorLayerLerper : MonoBehaviour
{
    [SerializeField] float timer, counter, oldValue, nextValue;
    [SerializeField] Animator animator;
    [SerializeField] int layerId;
    [SerializeField] bool isStarted;

    private void Start()
    {
        animator = GetComponent<Animator>();
        timer = 0.25f;
    }

    public void Reset()
    {
        animator.SetLayerWeight(layerId,nextValue);
        isStarted = false;
        counter = 0;
        oldValue = animator.GetLayerWeight(layerId);
    }

    public void StartLerp(int animatorLayerId, float nextValueToLerp)
    {
        Reset();

        if (nextValueToLerp == oldValue) return;

        layerId = animatorLayerId;
        nextValue = nextValueToLerp;
        isStarted = true;
    }

    void Update()
    {
        if (!isStarted) return;

        counter += Time.deltaTime;

        float value = Mathf.Lerp(oldValue, nextValue, counter/timer);
        animator.SetLayerWeight(layerId,value);

        if (counter > timer) { Reset(); }
    }
}
