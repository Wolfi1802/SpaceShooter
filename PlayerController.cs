using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float acceleration;//Beschleunigung
    private float steering;//Drehung

    public Camera Camera;
    public TrailRenderer[] TrailRenderer;
    public SpriteRenderer SpriteRenderer;

    public float AccelerationSpeed = 3;
    public float SteeringSpeed = 3;

    public int MaximumHealth = 6;
    public int CurrentHealth;

    public WeaponSystem[] WeaponSystems;
    public GameManager GameManager;

    public GameObject Flame;

    public SpriteRenderer DamageOverRenderer;
    public Sprite[] DamageOverlays;
    private int currentDamageOverlay = -1;

    private int currentWeaponSystemIndex = 0;

    private Rect visGameArea;
    private bool resetTrails;

    public Rigidbody2D Rigidbody;

    private void DetermineBoundary()
    {
        var halfWidth = Camera.orthographicSize * Camera.aspect;
        var halfHeight = Camera.orthographicSize;
        var shipSize = SpriteRenderer.size;

        visGameArea = new Rect(-halfWidth - shipSize.x,
            -halfHeight - shipSize.y,
            2 * halfWidth + 2 * shipSize.x,
            2 * halfHeight + 2 * shipSize.y);
    }

    private void Update()
    {
        this.acceleration = Math.Max(0, Input.GetAxis("Vertical"));
        this.steering = Input.GetAxis("Horizontal");
        Flame.SetActive(acceleration > 0);

        if (Input.GetKey(KeyCode.Space) &&
            this.currentWeaponSystemIndex >= 0 &&
            this.currentWeaponSystemIndex <= WeaponSystems.Length - 1)
        {
            this.WeaponSystems[this.currentWeaponSystemIndex].Fire();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            this.currentWeaponSystemIndex--;

            if (this.currentWeaponSystemIndex < 0)
            {
                this.currentWeaponSystemIndex = WeaponSystems.Length - 1;
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            this.currentWeaponSystemIndex++;

            if (this.currentWeaponSystemIndex > WeaponSystems.Length)
            {
                this.currentWeaponSystemIndex = 0;
            }
        }

        if (resetTrails)
        {
            foreach (var item in TrailRenderer)
            {
                item.Clear();
            }
            resetTrails = false;
        }
    }

    private void FixedUpdate()
    {
        this.Rigidbody.AddRelativeForce(new Vector2(0, this.acceleration * this.AccelerationSpeed));
        this.Rigidbody.AddTorque(-this.steering * this.SteeringSpeed);

        ContainInBoundary();
    }

    private void ContainInBoundary()
    {
        var pos = Rigidbody.position;
        var newPosition = Vector2.zero;

        if (pos.x < visGameArea.xMin)
        {
            newPosition = new Vector2(visGameArea.xMax, pos.y);
        }

        if (pos.x > visGameArea.xMax)
        {
            newPosition = new Vector2(visGameArea.xMin, pos.y);
        }

        if (pos.y < visGameArea.yMin)
        {
            newPosition = new Vector2(pos.x, visGameArea.yMax);
        }

        if (pos.y > visGameArea.yMax)
        {
            newPosition = new Vector2(pos.x, visGameArea.yMin);
        }

        if (newPosition != Vector2.zero)
        {
            Rigidbody.position = newPosition;
            resetTrails = true;
        }
    }

    private void Start()
    {
        this.CurrentHealth = this.MaximumHealth;
        this.GameManager.SetHealth(this.CurrentHealth);
        this.GameManager.AddPoints(0);//makes your Count visible after start

        Flame.SetActive(false);

        DetermineBoundary();
    }

    public void TakeDamage(int damage)
    {
        this.CurrentHealth -= damage;
        this.GameManager.SetHealth(this.CurrentHealth);

        this.SetDamageShip();

        if (this.CurrentHealth < 0)
        {
            gameObject.SetActive(false);

            GameManager.GameOver();
        }
    }

    private void SetDamageShip()
    {//TODO ahand des max lebens den dmg setzen
        var damagePacksAviable = this.DamageOverlays.Length;

        this.currentDamageOverlay++;

        this.currentDamageOverlay = Math.Min(this.currentDamageOverlay, this.DamageOverlays.Length - 1);
        this.DamageOverRenderer.sprite = this.DamageOverlays[this.currentDamageOverlay];

    }
}
