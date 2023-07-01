using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiEnemy : BasicEnemyAgentAi
{
    [SerializeField] protected float kunaiSpeed;
    [SerializeField] protected bool isMoving;
    [SerializeField] protected Transform kunaiTransform;

    private float yOffsetKunaiTransform = 1.2f;

    // Start is called before the first frame update

    protected override void Start()
    {
        isMoving = false;       
        weapon.GetComponent<BoxCollider>().enabled = true;
        playerTarget = PlayerManager.PlayerGameObject.transform;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isMoving)
        {
            MovingKunaiEnemy();
        }
        else
        {
            StandingKunaiEnemy();
        }
    }

    public void ThrowKunai()
    {
        GameObject kunai = Instantiate(weapon, kunaiTransform.position, kunaiTransform.rotation);
        Vector3 distanceFromTarget = playerTarget.position - kunaiTransform.transform.position;       
        kunai.GetComponent<Rigidbody>().velocity = new Vector3(distanceFromTarget.x, distanceFromTarget.y + yOffsetKunaiTransform, distanceFromTarget.z).normalized * kunaiSpeed;
    }



    void MovingKunaiEnemy()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);

        Vector3 distanceFromTarget = playerTarget.position - agent.transform.position;


        switch (currentState)
        {
            case EnemyState.Patrol:

                if (fov.targetCheck() == true)
                {
                    currentState = EnemyState.Chase;
                    break;
                }
                else if (agent.remainingDistance < 2f)
                {
                    fov.Angle = 150;
                    SetNewWaypoint();
                    agent.speed = patrolSpeed;
                    anim.SetBool("Attack", false);
                    break;
                }
                if (fov.targetCheck() == true && distanceFromTarget.magnitude <= attackDistance)
                {
                    currentState = EnemyState.Chase;
                    break;
                }
                if (enemyDamageManager.PlayerIsAttacking || enemyDamageManager.IsHitting)
                {
                    currentState = EnemyState.Chase;
                    //GetComponent<Animator>().Play("Knockback");
                    break;
                }
                break;


            case EnemyState.Chase:

                fov.Angle = 360;

                if (fov.targetCheck() == true && distanceFromTarget.magnitude <= attackDistance)
                {
                    agent.speed = 0;
                    currentState = EnemyState.Attack;
                    break;
                }
                else if (fov.targetCheck() == false)
                {
                    //Fov.Angle = 150;
                    currentState = EnemyState.Patrol;
                    agent.speed = patrolSpeed;
                    break;
                }

                agent.speed = chaseSpeed;
                agent.SetDestination(playerTarget.position);
                break;



            case EnemyState.Attack:

                anim.SetBool("Attack", true);
                agent.transform.forward = new Vector3(distanceFromTarget.normalized.x, 0, distanceFromTarget.normalized.z);
               
                if (enemyDamageManager.IsHitting && stunnManager.IsStunned == false)
                {
                    enemyDamageManager.IsHitting = false;
                    break;
                }

                if (fov.targetCheck() == true && distanceFromTarget.magnitude > attackDistance)
                {
                    anim.SetBool("Attack", false);
                    agent.speed = chaseSpeed;
                    currentState = EnemyState.Patrol;
                    break;
                }
                else if (fov.targetCheck() == false)
                {
                    currentState = EnemyState.Patrol;
                    agent.speed = patrolSpeed;
                    break;
                }

                break;


            case EnemyState.Healing:
                break;

            case EnemyState.Dead:
                agent.GetComponent<BasicEnemyAgentAi>().enabled = false;
                agent.GetComponent<CapsuleCollider>().enabled = false;
                agent.GetComponent<Animator>().enabled = false;             
                arancini.gameObject.SetActive(false);
                PowerUpManager.SpawnPowerUpRandom(transform.position);
                Destroy(this.gameObject, 5f);
                break;

            case EnemyState.Stun:
                if (healthManager.IsDead)
                {
                    currentState = EnemyState.Dead;
                    break;
                }
                anim.SetBool("Stunned", true);
                arancini.gameObject.SetActive(true);
                weapon.GetComponent<BoxCollider>().enabled = false;
                anim.SetBool("Attack", false);
                agent.isStopped = true;

                if (stunnManager.CurrentStunn < 1)
                {
                    anim.SetBool("Stunned", false);
                    currentState = EnemyState.Patrol;
                    arancini.gameObject.SetActive(false);
                    stunnManager.IsStunned = false;
                }

                break;

            default:
                break;
        }
    }




    void StandingKunaiEnemy()
    {
        Vector3 distanceFromTarget = playerTarget.position - agent.transform.position;

        switch (currentState)
        {
            case EnemyState.Patrol:

                fov.Angle = 360;
                if (fov.targetCheck() == true)
                {
                    agent.speed = 0;
                    currentState = EnemyState.Attack;
                    break;
                }
                if (enemyDamageManager.PlayerIsAttacking || enemyDamageManager.IsHitting)
                {
                    currentState = EnemyState.Attack;
                    //GetComponent<Animator>().Play("Knockback");
                    break;
                }
                break;



            case EnemyState.Attack:

                anim.SetBool("Attack", true);
                agent.transform.forward = new Vector3(distanceFromTarget.normalized.x, 0, distanceFromTarget.normalized.z);
              
                if (enemyDamageManager.IsHitting && stunnManager.IsStunned == false)
                {
                    enemyDamageManager.IsHitting = false;
                    break;
                }
                else if (fov.targetCheck() == false)
                {
                    anim.SetBool("Attack", false);
                    currentState = EnemyState.Patrol;
                    break;
                }

                break;


            case EnemyState.Healing:
                break;

            case EnemyState.Dead:
                agent.GetComponent<BasicEnemyAgentAi>().enabled = false;
                agent.GetComponent<CapsuleCollider>().enabled = false;
                agent.GetComponent<Animator>().enabled = false;              
                arancini.gameObject.SetActive(false);
                PowerUpManager.SpawnPowerUpRandom(transform.position);
                Destroy(this.gameObject, 5f);
                break;

            case EnemyState.Stun:
                if (healthManager.IsDead)
                {
                    currentState = EnemyState.Dead;
                    break;
                }
                anim.SetBool("Stunned", true);
                arancini.gameObject.SetActive(true);
                weapon.GetComponent<BoxCollider>().enabled = false;
                anim.SetBool("Attack", false);
                //agent.isStopped = true;

                if (stunnManager.CurrentStunn < 1)
                {
                    anim.SetBool("Stunned", false);
                    currentState = EnemyState.Patrol;
                    arancini.gameObject.SetActive(false);
                    stunnManager.IsStunned = false;
                }

                break;

            default:
                break;
        }
    }
}
