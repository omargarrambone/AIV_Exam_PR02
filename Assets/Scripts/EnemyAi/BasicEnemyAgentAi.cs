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

    public StunnManager StunnManager;

    public Animator Anim;

    public float PatrolSpeed;
    public float ChaseSpeed;
    
    public float AttackDistance;

    public List<Transform> PatrolWaypoints;
    public int CurrentWaypoint;

    public EnemyState CurrentState;

    public ParticleSystem Ucelletti;

    public bool IsAttacking;



    // Start is called before the first frame update
    void Start()
    {
        Fov = GetComponent<FieldOfView>();
        Anim = GetComponent<Animator>();       
        CurrentState = EnemyState.Patrol;        
       
        //SetNewWaypoint();

        PlayerTarget = PlayerManager.playerTransform.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromTarget = Vector3.Distance(PlayerTarget.position, Agent.transform.position);
        //if (fov.targetCheck() == true && distanceFromTarget <= activationDistance)        
        Anim.SetFloat("Speed", Agent.velocity.magnitude);
        

        switch (CurrentState)
        {
            case EnemyState.Patrol:
                if (Fov.targetCheck() == true)
                {
                    CurrentState = EnemyState.Chase;
                    break;
                }
                if (StunnManager.IsStunned)
                {
                    CurrentState = EnemyState.Stun;
                    Agent.speed = 0;
                    //IsAttacking = false;
                    //Anim.SetBool("Attack", false);
                    break;
                }
                else if(Agent.remainingDistance < 2f)
                {
                    //currentState = EnemyState.Patrol;
                    Fov.Angle = 150;
                    Agent.speed = PatrolSpeed;
                    SetNewWaypoint();
                    IsAttacking = false;
                    Anim.SetBool("Attack", false);
                    break;
                }                 
                break;


            case EnemyState.Chase:
                Fov.Angle = 360;
                if (Fov.targetCheck() == false)
                {
                    Fov.Angle = 150;
                    CurrentState = EnemyState.Patrol;
                    break;
                }
                else if (Fov.targetCheck() == true && distanceFromTarget <= AttackDistance)
                {
                    Agent.speed = 0;
                    CurrentState = EnemyState.Attack;
                    break;
                }
                Agent.speed = ChaseSpeed;
                Agent.SetDestination(PlayerTarget.position);
                //isAttacking = false;
                //anim.SetBool("Attack", false);
                break;


            case EnemyState.Attack:               
                if (StunnManager.IsStunned)
                {
                    CurrentState = EnemyState.Stun;
                    //SetState((int)EnemyState.Stun);
                    
                    IsAttacking = false;
                    Anim.SetBool("Attack", false);
                }
                else
                {
                    IsAttacking = true;
                    Anim.SetBool("Attack", true);
                }
                if (Fov.targetCheck() == true && distanceFromTarget > AttackDistance)
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
                    //break;
                }
                
                break;


            case EnemyState.Healing:
                break;


            case EnemyState.Stun:
                Anim.SetBool("Stunned", true);
                Ucelletti.gameObject.SetActive(true);
               
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
            default:
                break;
        }

        //if (agent.remainingDistance < 2f)
        //{
        //    currentState = EnemyState.Patrol;
        //    agent.speed = patrolSpeed;
        //    SetNewWaypoint();            
        //}
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

    
}
