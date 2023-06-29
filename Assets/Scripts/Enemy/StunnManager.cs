using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StunnManager : MonoBehaviour
{
    float MinStunnValue = 0;
    public float CurrentStunn { get; private set; }
    public bool IsStunned;

    public float StunnDecreaseVelocity = 20.0f;
    public float Timer = 3.0f;
    [SerializeField] float counter;

    public BarScript StunnBar;
    public EnemyDamageManager EnemyDamageManager;

    public UnityEvent OnStun;

    // Start is called before the first frame update
    void Start()
    {
        CurrentStunn = MinStunnValue;
        StunnBar.SetMinStunnValue(MinStunnValue);
    }

    public void TakeStunn(float stunnDamage)
    {
        if (IsStunned) return;

        CurrentStunn += stunnDamage;

        StunnBar.SetStunn(CurrentStunn);

        if (CurrentStunn >= 95)
        {
            OnStun.Invoke();
            IsStunned = true;
        }
    }

    private void Update()
    {
        if (CurrentStunn > 0)
        {
            if (counter <= Timer)
                counter += Time.deltaTime;

            if (counter >= Timer)
            {
                CurrentStunn -= Time.deltaTime * StunnDecreaseVelocity;
                StunnBar.SetStunn(CurrentStunn);
            }
        }

        if (EnemyDamageManager.PlayerIsAttacking)
        {
            counter = 0;
        }
    }
}

