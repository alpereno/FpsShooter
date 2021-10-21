using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask collisionMask;
    float bulletSpeed = 10f;
    float damage = 100;
    float lifeTime = 2f;
    float headDamage = 100, bodyDamage = 50, armDamage = 30, legDamage = 10;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void setBulletSpeed(float newSpeed) {
        bulletSpeed = newSpeed;
    }

    void Update()
    {
        float moveDistance = bulletSpeed * Time.deltaTime;
        checkCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    private void checkCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            onHitEnemy(hit);
        }
    }

    void onHitEnemy(RaycastHit hit) {

        if (hit.collider.CompareTag("Head"))
            damage = headDamage;
        else if (hit.collider.CompareTag("Arm"))
            damage = armDamage;
        else if (hit.collider.CompareTag("Leg"))
            damage = legDamage;
        else
            damage = bodyDamage;
        
        IDamageable damageableObject = hit.collider.GetComponentInParent<IDamageable>();
        if (damageableObject != null)
        {
            //print(hit.collider.tag);
            damageableObject.takeHit(damage, hit);
        }
        GameObject.Destroy(gameObject);
    }
}
