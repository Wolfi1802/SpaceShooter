using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public Bullet BulletPrefab;
    public float FireRate = 1f;
    public AudioSource SoundEffect;

    private float fireRateCounter;

    private void Update()
    {
        this.fireRateCounter += Time.deltaTime;
    }

    public void Fire()
    {
        if (this.fireRateCounter > FireRate)
        {
            this.fireRateCounter = 0;

            this.SoundEffect.Play();

            foreach (var item in SpawnPoints)
            {
                Instantiate(this.BulletPrefab, item);
            }
        }
    }
}
