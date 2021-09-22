using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject patientPrefab;
    public float startTime = 2;
    public float delay = 4;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnPatients", startTime);
    }


    private void SpawnPatients()
    {
        Instantiate(patientPrefab, this.transform.position, Quaternion.identity);
        Invoke("SpawnPatients", Random.Range(1, 4));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
