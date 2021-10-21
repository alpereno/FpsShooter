using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity
{
    public ParticleSystem damageEffect;
    public enum State { Idle, Chasing, Attacking };
    State currentState;

    Material defaultMaterial;
    Color originalColor;
    Transform target;
    NavMeshAgent pathfinder;
    LivingEntity targetEntity;

    float damage = 10f;
    float attackDistanceThreshold = 3.8f;
    float timeBetweenAttacks = 1.5f;
    float nextAttackTime;
    bool isTargetAlive;

    protected override void Start()
    {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();
        defaultMaterial = GetComponentInChildren<Renderer>().material;
        originalColor = defaultMaterial.color;
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += onTargetDeath;
            isTargetAlive = true;
            currentState = State.Chasing;
            StartCoroutine(updatePath());
        }
    }

    //check for attack
    private void Update()
    {   
        if (isTargetAlive)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDistanceToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDistanceToTarget < Mathf.Pow(attackDistanceThreshold, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(attack());
                }
            }
        }
    }

    void onTargetDeath() {
        isTargetAlive = false;
        currentState = State.Idle;
    }

    //Enemy Attack
    IEnumerator attack() {
        currentState = State.Attacking;
        pathfinder.enabled = false;
        print("attacking");
        defaultMaterial.color = Color.red;

        //attacking anim etc...
        targetEntity.takeDamage(damage);
        yield return new WaitForSeconds(timeBetweenAttacks - .5f);

        defaultMaterial.color = originalColor;
        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    //locating and following player
    IEnumerator updatePath() {
        float refreshRate = .25f;
        float offset = 3.5f;
        while (isTargetAlive)
        {
            if (currentState == State.Chasing)
            {
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                //Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
                Vector3 targetPosition = target.position - directionToTarget * (offset);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    public override void takeHit(float damage, RaycastHit hit)
    {
        //Destroy(Instantiate(damageEffect.gameObject, hit.point, hit.transform.rotation) as GameObject, damageEffect.main.startLifetime.constant);
        Destroy(Instantiate(damageEffect.gameObject, hit.point, Quaternion.LookRotation(hit.normal)) as GameObject, damageEffect.main.startLifetime.constant);
        base.takeHit(damage, hit);
    }
}
