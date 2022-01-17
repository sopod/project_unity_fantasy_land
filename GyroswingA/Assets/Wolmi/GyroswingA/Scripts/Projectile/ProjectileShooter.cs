using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] ProjectileSpawner spawner;
    [SerializeField] GameObject mouth;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Fire();
        }
    }

    void Fire()
    {
        GameObject p = spawner.SpawnProjectile(ProjectileType.Shoot, mouth.transform.position, mouth.transform.forward);

        p.GetComponent<ProjectileController>().SetStart(spawner, new Options());
    }
}
