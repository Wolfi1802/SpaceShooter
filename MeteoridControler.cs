using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeteoridControler : MonoBehaviour
{
    public int Damage = 1;
    public int MaximumHealth = 3;
    public int CurrentHealt;
    public int Points = 1;

    public GameObject ExplosionPrefeb;

    private GameManager gameManager;

    private void Start()
    {
        this.gameManager = FindObjectOfType<GameManager>();
        this.CurrentHealt = this.MaximumHealth;
        Destroy(gameObject, 30f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerController = collision.GetComponent<PlayerController>();
            playerController.TakeDamage(this.Damage);

            Instantiate(ExplosionPrefeb, transform.position, Quaternion.identity);

            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        this.CurrentHealt -= damage;

        if (this.CurrentHealt <= 0)
        {
            this.gameManager.AddPoints(this.Points);
            Destroy(this.gameObject);
        }
    }
}
