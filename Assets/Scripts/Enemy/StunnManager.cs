using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StunnManager : MonoBehaviour
{

    float MinStunnValue = 0;
    float CurrentStunn;
    public bool IsStunned;
    float StunnDecreaseVelocity = 20.0f;

    float timer = 3.0f;
    float counter;

    public HealthBarScript StunnBar;
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

        if (CurrentStunn >= 100)
        {
            OnStun.Invoke();
            IsStunned = true;
        }
    }

    private void Update()
    {
        if (CurrentStunn > 0)
        {
            if (counter <= timer)
                counter += Time.deltaTime;

            if (counter >= timer)
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
