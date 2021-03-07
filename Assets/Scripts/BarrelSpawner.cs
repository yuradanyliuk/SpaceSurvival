﻿using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    #region Fields
    [Header("Objects")]
    [SerializeField] Transform player;
    [SerializeField] GameObject barrelPrefab;
    [Header("Spawn circle")]
    [SerializeField] float spawnCircleRadius;
    [SerializeField] float noSpawnCircleRadius;
    [SerializeField] float spawnDelayDistance;
    [Header("Spawn count")]
    [SerializeField] int startSpawnCount;
    [SerializeField] int spawnCountWhenPassingDistance;

    Vector3 offset;
    Vector3 previousPlayerPosition;
    #endregion

    #region Methods
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3();
        Spawn(startSpawnCount);
        previousPlayerPosition = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, previousPlayerPosition) > spawnDelayDistance)
        {
            Spawn(spawnCountWhenPassingDistance);
            previousPlayerPosition = player.position;
        }
    }
    void Spawn(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            float spawnRadius = spawnCircleRadius * Random.value + noSpawnCircleRadius;
            float ang = Random.value * 360;
            offset.x = Mathf.Sin(ang * Mathf.Deg2Rad) * spawnRadius;
            offset.z = Mathf.Cos(ang * Mathf.Deg2Rad) * spawnRadius;
            Instantiate(barrelPrefab, player.position + offset, Random.rotation);
        }
    }
    #endregion
}
