using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public ParticleSystem muzzleFlash;

    [SerializeField] private Transform muzzle;
    [SerializeField] private Projectile projectile;
    [SerializeField] private float msBetweenShots = 100f;
    [SerializeField] private float muzzleVelocity = 35;
    [SerializeField] private int bulletsPerMagazine;
    [SerializeField] private float reloadTime = 1.5f;

    Material gunMaterial;
    Color originalColor;
    float nextShotTime;
    Vector3 recoilDampVelocity;
    float recoilTime = .1f;
    int bulletsRemainingInMagazine;
    bool isReloading;

    private void Start()
    {
        bulletsRemainingInMagazine = bulletsPerMagazine;
        gunMaterial = GetComponentInChildren<Renderer>().material;
        originalColor = gunMaterial.color;
    }

    private void Update()
    {
        //recoil anim
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilDampVelocity, recoilTime);

        if (!isReloading && bulletsRemainingInMagazine == 0)
        {
            reload();
        }
    }

    public void shoot() {
        if (!isReloading && bulletsRemainingInMagazine > 0 && Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000f;
            Projectile tempProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            tempProjectile.setBulletSpeed(muzzleVelocity);
            bulletsRemainingInMagazine--;
            muzzleFlash.Play();
            //Destroy(Instantiate(muzzleFlash.gameObject, muzzle.transform.position, muzzle.rotation) as GameObject, muzzleFlash.main.startLifetime.constantMax);
            //recoil
            transform.localPosition -= Vector3.forward * .1f;
        }
    }

    public void reload() {
        if (!isReloading && bulletsRemainingInMagazine != bulletsPerMagazine)
        {
            StartCoroutine(reloadAnimation());
        }
    }

    IEnumerator reloadAnimation() {
        isReloading = true;
        gunMaterial.color = Color.white;
        print("reloading");
        yield return new WaitForSeconds(reloadTime);
        //reload anim

        gunMaterial.color = originalColor;
        
        isReloading = false;
        bulletsRemainingInMagazine = bulletsPerMagazine;
    }
}
