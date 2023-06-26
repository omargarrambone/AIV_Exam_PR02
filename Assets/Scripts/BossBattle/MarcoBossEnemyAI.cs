using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MarcoBossEnemyAI : BasicEnemyAgentAi
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        Vector3 distanceFromTarget = PlayerTarget.position - Agent.transform.position;

        Anim.SetFloat("Speed", Agent.velocity.magnitude);

        switch (CurrentState)
        {
            case EnemyState.Patrol:

                if (Fov.targetCheck() == true)
                {
                    CurrentState = EnemyState.Chase;
                    break;
                }
                else if (Agent.remainingDistance < 2f)
                {

                    Fov.Angle = 150;
                    SetNewWaypoint();
                    Agent.speed = PatrolSpeed;
                    //IsAttacking = false;
                    Anim.SetBool("Attack", false);

                    break;
                }
                if (EnemyDamageManager.PlayerIsAttacking)
                {
                    CurrentState = EnemyState.Chase;
                    break;
                }
                break;


            case EnemyState.Chase:


                Fov.Angle = 360;


                if (Fov.targetCheck() == true && distanceFromTarget.magnitude <= AttackDistance)
                {
                    Agent.speed = 0;
                    CurrentState = EnemyState.Attack;
                    IsAttacking = true;
                    break;
                }
                else if (Fov.targetCheck() == false)
                {
                    //Fov.Angle = 150;
                    CurrentState = EnemyState.Patrol;
                    Agent.speed = PatrolSpeed;

                    break;
                }

                Agent.speed = ChaseSpeed;
                Agent.SetDestination(PlayerTarget.position);
                break;



            case EnemyState.Attack:

                Anim.SetBool("Attack", true);

                Agent.transform.forward = new Vector3(distanceFromTarget.normalized.x, 0, distanceFromTarget.normalized.z);

                if (EnemyDamageManager.IsParrying)
                {
                    Anim.SetBool("IsParrying", true);
                    TimeParry -= Time.deltaTime;

                    if (TimeParry <= 0)
                    {
                        EnemyDamageManager.IsParrying = false;
                        Anim.SetBool("IsParrying", false);
                        TimeParry = 0.3f;
                    }
                    break;
                }

                if (EnemyDamageManager.IsHitting && StunnManager.IsStunned == false)
                {
                    Anim.SetTrigger("IsHitting");

                    EnemyDamageManager.IsHitting = false;

                    break;
                }

                if (Fov.targetCheck() == true && distanceFromTarget.magnitude > AttackDistance)
                {
                    Anim.SetBool("Attack", false);
                    Agent.speed = ChaseSpeed;
                    CurrentState = EnemyState.Chase;
                    IsAttacking = false;
                    break;
                }
                else if (Fov.targetCheck() == false)
                {
                    CurrentState = EnemyState.Patrol;
                    Agent.speed = PatrolSpeed;
                    break;
                }

                break;


            case EnemyState.Healing:
                break;

            case EnemyState.Dead:
                Agent.GetComponent<BasicEnemyAgentAi>().enabled = false;
                Agent.GetComponent<CapsuleCollider>().enabled = false;
                Weapon.GetComponent<BoxCollider>().enabled = false;
                Agent.GetComponent<Animator>().enabled = false;
                gameObject.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                gameObject.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                Arancini.gameObject.SetActive(false);
                SpawnPowerUp(HeavyHealth);
                Destroy(this.gameObject, 5f);
                break;

            case EnemyState.Stun:
                if (HealthManager.IsDead)
                {
                    CurrentState = EnemyState.Dead;
                    break;
                }
                Anim.SetBool("Stunned", true);
                Arancini.gameObject.SetActive(true);
                Weapon.GetComponent<BoxCollider>().enabled = false;
                IsAttacking = false;
                Anim.SetBool("Attack", false);
                Agent.speed = 0;

                //Enemy ready to be purified by the sound of the Magic Flute

                if (StunnManager.CurrentStunn < 1)
                {
                    Anim.SetBool("Stunned", false);
                    CurrentState = EnemyState.Patrol;
                    //Agent.speed = PatrolSpeed;
                    Arancini.gameObject.SetActive(false);
                    StunnManager.IsStunned = false;
                }

                break;
        }
    }
}

