using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private float hp = 3f;
    private float fall = -4f;
    private float appear;
    private bool hit;
    private bool down;

    private void Start()
    {
        transform.position = new Vector3(Random.Range(-38.5f, 38.5f), 20, Random.Range(5f, 80f));
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        down = true;
    }

    public void Damaged (float damage)
    {
        hp -= damage;
    }

    private void Update()
    {
        if(transform.localScale.x < 2f && Time.time > appear && hp > 0f && down == true)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            appear = Time.time + 0.01f;
        }
        if (transform.position.y > 0.5)
        {
            transform.Translate(0, Time.deltaTime * fall,0);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 0.1f, transform.eulerAngles.z);
        }

        if(transform.position.y <= 0.5)
        {
            down = false;
        }


        if (hp <= 0f || transform.position.y < -10f || hit == true)
        {
            down = false;
            while (transform.localScale.x > 0.1f && Time.time > appear)
            {
                transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                appear = Time.time + 0.005f;
            }
            if(transform.localScale.x <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            hit = true;
        }
    }
}
