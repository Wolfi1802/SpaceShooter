using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public Camera Camera;
    public GameObject Player;
    public GameObject[] Meteorids;

    public float SpawnRateMinimum = 0.5f;
    public float SpawnRateMaximum = 1.5f;

    public float MeteoridRotationMinimum = 0.5f;
    public float MeteoridRotationMaximum = 1.5f;

    public float MeteoridSpeedMinimum = 1;
    public float MeteoridSpeedMaximum = 3;

    private float nextSpawnTime;

    private void DetermineNextSpawnTime()
    {
        this.nextSpawnTime = Time.time + Random.Range(SpawnRateMinimum, SpawnRateMaximum);
    }

    private void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            this.SpawnMeteroid();
            this.DetermineNextSpawnTime();
        }
    }

    private void Start()
    {
        this.DetermineNextSpawnTime();
    }

    private void SpawnMeteroid()
    {
        var prefableIndexToSpawn = Random.Range(0, this.Meteorids.Length);
        var prefableToSpawn = this.Meteorids[prefableIndexToSpawn];

        var meteorid = Instantiate(prefableToSpawn, transform);

        var placeVertical = Random.Range(0, 2) == 0;
        float y;
        float x;
        int sign;

        if (placeVertical)
        {
            var halfWidth = Camera.orthographicSize * Camera.aspect;

            x = Random.Range(-halfWidth, halfWidth);

            sign = (Random.Range(0, 2) == 0) ? -1 : 1;

            y = sign * (Camera.orthographicSize + 1);
        }
        else
        {
            var halfHeighth = Camera.orthographicSize;

            y = Random.Range(-halfHeighth, halfHeighth);

            sign = (Random.Range(0, 2) == 0) ? -1 : 1;

            x = sign * (Camera.orthographicSize * Camera.aspect + 1);
        }

        var position = new Vector3(x, y);

        meteorid.transform.position = position;

        var direction = position - Player.transform.position;
        var speed = Random.Range(MeteoridSpeedMinimum, MeteoridSpeedMaximum);

        var rigitBody = meteorid.GetComponent<Rigidbody2D>();

        rigitBody.AddForce(-direction.normalized * speed, ForceMode2D.Impulse);

        var rotation = Random.Range(MeteoridRotationMinimum, MeteoridRotationMaximum);

        rigitBody.AddTorque(rotation);
    }
}
