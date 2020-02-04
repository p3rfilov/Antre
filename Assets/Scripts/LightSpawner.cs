using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpawnTempLight (Vector3 position, Color color, float intensity, float lifetime)
    {
        GameObject lightGameObject = new GameObject("Temp Light");
        Light lightComp = lightGameObject.AddComponent<Light>();
        lightComp.color = color;
        lightGameObject.transform.position = position;
    }
}
