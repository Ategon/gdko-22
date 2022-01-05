using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    [SerializeField] private GameObject camera;
    public float parallaxEffect;

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<TilemapRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = (camera.transform.position.x * (1 - parallaxEffect));
        float dist = (camera.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
    }
}
