using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void takeHit(float damage, RaycastHit hit);
    void takeDamage(float damage);
}
