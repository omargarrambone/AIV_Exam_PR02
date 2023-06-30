using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiEnemy : BasicEnemyAgentAi
{
    [SerializeField] protected float kunaiSpeed;
    [SerializeField] protected bool isMoving;
    private float yOffsetKunaiTransform = 1.2f;
    [SerializeField] protected Transform kunaiTransform;

    //protected float kunaiTimer = 0;
    // Start is called before the first frame update

    protected override void Start()
    {
        fov = GetComponent<FieldOfView>();
        anim = GetComponent<Animator>();
        currentState = EnemyState.Patrol;
        weapon.GetComponent<BoxCollider>().enabled = true;
        playerTarget = PlayerManager.PlayerGameObject.transform;
    }

    // Update is called once per frame
    protected override void Update()
    {
        //base.Update();

        Vector3 distanceFromTarget = playerTarget.position - agent.transform.position;


        if (isMoving)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude);

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
                        //agent.speed = 0;
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
                    //if (enemyDamageManager.PlayerIsAttacking)
                    //{
                    //    currentState = EnemyState.Chase;
                    //    anim.Play("Knockback");
                    //    break;
                    //}
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
                    //weapon.GetComponent<BoxCollider>().enabled = false;
                    agent.GetComponent<Animator>().enabled = false;
                    //gameObject.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                    //gameObject.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
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
        else
        {
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
                    //if (enemyDamageManager.PlayerIsAttacking)
                    //{
                    //    currentState = EnemyState.Chase;
                    //    anim.Play("Knockback");
                    //    break;
                    //}
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
                    //weapon.GetComponent<BoxCollider>().enabled = false;
                    agent.GetComponent<Animator>().enabled = false;
                    //gameObject.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                    //gameObject.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
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

    public void ThrowKunai()
    {
        GameObject kunai = Instantiate(weapon, kunaiTransform.position, kunaiTransform.rotation);
        Vector3 distanceFromTarget = playerTarget.position - kunaiTransform.transform.position;       
        kunai.GetComponent<Rigidbody>().velocity = new Vector3(distanceFromTarget.x, distanceFromTarget.y + yOffsetKunaiTransform, distanceFromTarget.z).normalized * kunaiSpeed;
    }

    
}
