using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    public Transform Player;
    public Transform Robot;
    private int MoveSpeed = 9;

    private float hp;
    private float spawn;
    private float begin;
    private bool open;
    private float walk;

    Animator anim;

    private AudioSource walkSound;
    public GameObject ground;

    public GameObject start;
    public GameObject stop;
    private AudioSource startSound;
    private AudioSource stopSound;
    private bool stopping;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        hp = 40f;
        begin = Time.time;
        open = false;
        walkSound = ground.GetComponent<AudioSource>();
        startSound = start.GetComponent<AudioSource>();
        stopSound = stop.GetComponent<AudioSource>();
        startSound.Play();
        stopping = true;
    }

    void Update()
    {
        if (hp > 0f)
        {
            stopping = true;
            anim.SetBool("Open_Anim", true);
            if (Time.time > begin + 4f)
            {
                open = true;
                walk = Time.time;
            }
            if (open == true)
            {
                if (Time.time > walk)
                {
                    walk = Time.time + 1f;
                    walkSound.Play();
                }
                Vector3 target = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);
                transform.LookAt(target);
                transform.position += transform.forward * MoveSpeed * Time.deltaTime;
                anim.SetBool("Walk_Anim", true);
                spawn = Time.time;
            }
        }


        if (hp <= 0f)
        {
            if(stopping == true)
            {
                stopSound.Play();
                stopping = false;
            }
            
            open = false;
            anim.SetBool("Walk_Anim", false);
            anim.SetBool("Open_Anim", false);
            if (Time.time > spawn + 10)
            {
                begin = Time.time;
                hp = 30f;
                startSound.Play();
            }
        }

    }

    public void Damaged(float damage)
    {
        hp -= damage;
    }

}
