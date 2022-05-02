using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    Color[] m_colors = { Color.blue, Color.red, Color.yellow, Color.green };

    public string name;

    public bool bIsFired = false;
    public bool bShouldDie = false;
    public Vector3 Dir;
    public float Speed = 5.0f;
    public float LifeTime = 5.0f;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Renderer>().material.color = m_colors[Random.Range(0, 3)];
    }

    // Update is called once per frame
    void Update()
    {
        if (bIsFired)
            Travel();

        if (bShouldDie)
        {
            LifeTime -= Time.deltaTime;

            if (LifeTime <= 0.0f)
                Destroy(gameObject);
        }
    }

    public Color GetColor()
    {
        return GetComponent<Renderer>().material.color;
    }

    void Travel()
    {
        transform.position += Dir * Speed * Time.deltaTime;
    }
}