using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private PowerUp[] powerUps;
    static private PowerUp[] staticPowerUps;

    private void Start()
    {
        staticPowerUps = powerUps;
    }

    public static void SpawnPowerUpRandom(Vector3 position)
    {
        SpawnPowerUp(staticPowerUps[Random.Range(0, staticPowerUps.Length)], position);
    }

    public static void SpawnPowerUp(PowerUp powerUp, Vector3 position)
    {
        Instantiate(powerUp, position + new Vector3(0, 1f, 0f), powerUp.transform.rotation);
    }
}
