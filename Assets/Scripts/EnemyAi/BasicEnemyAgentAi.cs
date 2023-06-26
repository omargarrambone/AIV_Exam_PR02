using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAgentAi : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Rigidbody Rb;
    public FieldOfView Fov;
    private Transform PlayerTarget;
    public GameObject Weapon;
    public StunnManager StunnManager;
    public HealthManager HealthManager;
    public EnemyDamageManager EnemyDamageManager;
    public Animator Anim;
    public float PatrolSpeed;
    public float ChaseSpeed;
    public float AttackDistance;
    public List<Transform> PatrolWaypoints;
    public int CurrentWaypoint;
    public EnemyState CurrentState;
    public ParticleSystem Arancini;
    public bool IsAttacking;
    public PowerUp HeavyHealth;   
    public float TimeParry = 0.3f;
   



    // Start is called before the first frame update
    void Start()
    {
        
        Fov = GetComponent<FieldOfView>();
        Anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody>();
        CurrentState = EnemyState.Patrol;
        Weapon.GetComponent<BoxCollider>().enabled = false;

        //SetNewWaypoint();

        PlayerTarget = PlayerManager.PlayerGameObject.transform;
    }




    // Update is called once per frame
    void Update()
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
              
                Agent.transform.forward = new Vector3(distanceFromTarget.normalized.x, 0,distanceFromTarget.normalized.z);

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

    public void StartAttack()
    {
        Weapon.GetComponent<BoxCollider>().enabled = true;
    }

    public void EndAttack()
    {
        Weapon.GetComponent<BoxCollider>().enabled = false;
    }

}
