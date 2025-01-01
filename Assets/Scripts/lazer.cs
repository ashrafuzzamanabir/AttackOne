using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazer : MonoBehaviour
{
    [SerializeField]  private float timeTilSpawn;
    [SerializeField]  public float startTimeTilSpawn;

    [SerializeField]  public GameObject laser;
    [SerializeField]  public Transform whereToSpawn;

    


    private void Update()
    {
        if (timeTilSpawn <= 0)
        {
            Instantiate(laser, whereToSpawn.position, whereToSpawn.rotation);
            timeTilSpawn = startTimeTilSpawn;
        }
        else
        {
            timeTilSpawn -= Time.deltaTime;
        }
    }

    


}
