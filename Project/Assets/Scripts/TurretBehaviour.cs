using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    public float RotationSpeed = 20.0f;
    float angle = 0.0f;

    public GameObject BubblePrefab;

    public GameObject Dir1;
    public GameObject Dir2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Mouse X") != 0.0f)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = pos - transform.position;
            dir.Normalize();

            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, 25.0f, 145.0f);

            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }
        else
        {
            angle += GetInput() * RotationSpeed * Time.deltaTime;

            angle = Mathf.Clamp(angle, 30.0f, 150.0f);

            transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);
        }
    }

    float GetInput()
    {
        if (Input.GetKey(KeyCode.A))
            return 1.0f;
        else if (Input.GetKey(KeyCode.D))
            return -1.0f;

        return 0.0f;
    }

    public void Fire(GameObject obj)
    {
        Vector3 dir = (Dir2.transform.position - Dir1.transform.position).normalized;

        GameObject bubbleToShoot = Instantiate(BubblePrefab, transform.position, transform.rotation);

        bubbleToShoot.AddComponent<BubbleCollision>();
        bubbleToShoot.GetComponent<Renderer>().material.color = obj.GetComponent<Renderer>().material.color;
        bubbleToShoot.GetComponent<Bubble>().Dir = dir;
        bubbleToShoot.GetComponent<Bubble>().bIsFired = true;
        bubbleToShoot.GetComponent<Bubble>().bShouldDie = true;
    }
}
