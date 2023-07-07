using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKickScript : MonoBehaviour
{
    [SerializeField] private Collider kickCollider;

    [SerializeField] float resetCounter, resetTimer;
    bool isTimerOn;

    private void Update()
    {
        if (!isTimerOn) return;

        resetCounter -= Time.deltaTime;

        if (resetCounter < 0)
        {
            PlayerManager.EnablePlayerMovement();
            resetCounter = resetTimer;
            isTimerOn = false;
        }
    }

    public void CallOnStartKick()
    {
        kickCollider.enabled = true;

        isTimerOn = true;
    }

    public void StartedAnimation()
    {
        PlayerManager.DisablePlayerMovement();
    }

    public void CallOnEndKick()
    {
        kickCollider.enabled = false;
    }
}
