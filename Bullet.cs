using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    public float Speed;
    public int Damage;
    public GameObject ExplosionPrefab;

    public void Start()
    {
        Rigidbody.AddRelativeForce(new Vector2(0, Speed), ForceMode2D.Impulse);
        Destroy(gameObject, 30f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            var meteoroidController = other.GetComponent<MeteoridControler>();

            meteoroidController.TakeDamage(Damage);

            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
