using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

public class CubesManager : MonoBehaviour
{
    public Transform cubesPrefab;
    public float zoneSize = 20;
    public int nbCubesCreate = 30;

    void Start()
    {
        Transform cubePrev = null;
        Color[] palette = new Color[nbCubesCreate];
        VisionUtility.GetColorBlindSafePalette(palette, 0.5f, 1.0f);
        for (int i = 0; i < nbCubesCreate; i++)
        {
            Transform cube = GameObject.Instantiate<Transform>(cubesPrefab);
            cube.parent = transform;
            cube.position = Random.insideUnitSphere * zoneSize;
            cube.position = new Vector3(cube.localPosition.x, 0, cube.localPosition.z);

            cube.GetComponent<SimpleMove>().vitesseMax *= Random.Range(0.5f, 2.0f);
            cube.GetComponent<SimpleMove>().acceleration *= Random.Range(0.5f, 2.0f);
            cube.GetComponent<Renderer>().material.color = palette[i];

            //Une chance sur deux que le cube créé suive celui qu'on a créé a la boucle précédente
            if (Random.Range(0.0f, 1.0f) < 0.5)
            {
                cube.GetComponent<SimpleMove>().target = cubePrev;
            }

            cubePrev = cube;
        }

    }
}