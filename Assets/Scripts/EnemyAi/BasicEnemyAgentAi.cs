using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAgentAi : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Rigidbody Rb;
    public FieldOfView Fov;

    public Transform PlayerTarget;

    public GameObject Weapon;

    public StunnManager StunnManager;
    public HealthManager HealthManager;

    public Animator Anim;

    public float PatrolSpeed;
    public float ChaseSpeed;
    
    public float AttackDistance;

    public List<Transform> PatrolWaypoints;
    public int CurrentWaypoint;

    public EnemyState CurrentState;

    public ParticleSystem Ucelletti;

    public bool IsAttacking;

    public PowerUp HeavyHealth;
    





    // Start is called before the first frame update
    void Start()
    {
        Fov = GetComponent<FieldOfView>();
        Anim = GetComponent<Animator>();       
        CurrentState = EnemyState.Patrol;        
       
        //SetNewWaypoint();

        PlayerTarget = PlayerManager.PlayerGameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distanceFromTarget = PlayerTarget.position - Agent.transform.position;
        //float distanceFromTarget = Vector3.Distance(PlayerTarget.position, Agent.transform.position);
               
        Anim.SetFloat("Speed", Agent.velocity.magnitude);
        

        switch (CurrentState)
        {
            case EnemyState.Patrol:
                if (Fov.targetCheck() == true)
                {
                    CurrentState = EnemyState.Chase;
                    break;
                }             
                else if(Agent.remainingDistance < 2f)
                {
                    Fov.Angle = 150;
                    Agent.speed = PatrolSpeed;
                    SetNewWaypoint();
                    //IsAttacking = false;
                    Anim.SetBool("Attack", false);
                    break;
                }                 
                break;


            case EnemyState.Chase:
                Fov.Angle = 360;
                if (Fov.targetCheck() == false)
                {
                    //Fov.Angle = 150;
                    CurrentState = EnemyState.Patrol;
                    break;
                }
                else if (Fov.targetCheck() == true && distanceFromTarget.magnitude <= AttackDistance)
                {
                    Agent.speed = 0;
                    CurrentState = EnemyState.Attack;
                    IsAttacking = true;
                    break;
                }
                Agent.speed = ChaseSpeed;
                Agent.SetDestination(PlayerTarget.position);
             
                break;


            case EnemyState.Attack:
               
                //IsAttacking = true;
                Anim.SetBool("Attack", true);
                Weapon.GetComponent<BoxCollider>().enabled = true;
                if (IsAttacking)
                {
                    Agent.transform.forward = distanceFromTarget.normalized;
                }

                if (Fov.targetCheck() == true && distanceFromTarget.magnitude > AttackDistance)
                {
                    CurrentState = EnemyState.Chase;
                    Agent.speed = ChaseSpeed;
                    IsAttacking = false;
                    Anim.SetBool("Attack", false);
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


            case EnemyState.Stun:
                if (HealthManager.IsDead)
                {
                    CurrentState = EnemyState.Dead;
                    break;
                }
                Anim.SetBool("Stunned", true);
                Ucelletti.gameObject.SetActive(true);
                Weapon.GetComponent<BoxCollider>().enabled = false;
                IsAttacking = false;
                Anim.SetBool("Attack", false);
                Agent.speed = 0;

                //Enemy ready to be purified by the sound of the Magic Flute

                if (StunnManager.CurrentStunn < 1)
                {
                    Anim.SetBool("Stunned", false);
                    CurrentState = EnemyState.Patrol;
                    Agent.speed = PatrolSpeed;
                    Ucelletti.gameObject.SetActive(false);                   
                    StunnManager.IsStunned = false;
                }
              
                break;

            case EnemyState.Dead:
                Agent.GetComponent<BasicEnemyAgentAi>().enabled = false;
                Agent.GetComponent<CapsuleCollider>().enabled = false;
                Weapon.GetComponent<BoxCollider>().enabled = false;
                Agent.GetComponent<Animator>().enabled = false;
                gameObject.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                gameObject.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                Ucelletti.gameObject.SetActive(false);
                SpawnPowerUp(HeavyHealth);
                Destroy(this.gameObject, 5f);
                break;
            default:
                break;
        }

    }

    public void SetState(int state)
    {
        CurrentState = (EnemyState)state;
    }


    public void SetNewWaypoint()
    {
        CurrentWaypoint = Random.Range(0, PatrolWaypoints.Count);        
        Agent.SetDestination(PatrolWaypoints[CurrentWaypoint].position);
    }


    public void SpawnPowerUp(PowerUp lightHealth)
    {
        Instantiate(lightHealth, transform.position + new Vector3(0, 1f, 1f), lightHealth.transform.rotation);
    }
    
}
