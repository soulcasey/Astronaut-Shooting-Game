using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShot : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    private AudioSource shootSound;

    public Camera cam;
    public float fire;
    private float nextfire;
    private float damage = 1f;


    public GameObject muzzle;

    public ParticleSystem[] flash;
    public ParticleSystem splash;
    public TrailRenderer trace;

    public PlayerMovement player;

    void Start()
    {
        shootSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextfire && player.dead == false)
        {
            nextfire = Time.time + fire;
            Shoot();
        }
    }

    void Shoot()
    {
        shootSound.Play();
        foreach(var particle in flash)
        {
            particle.Emit(1);
        }

        ray.origin = cam.transform.position;

        var tracer = Instantiate(trace, muzzle.transform.position, Quaternion.identity);
        tracer.AddPosition(muzzle.transform.position);

        if (Physics.Raycast(ray.origin, cam.transform.forward, out hit))
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if(enemy != null && enemy.transform.position.y > 0.5)
            {
                enemy.Damaged(damage);
            }

            RobotMovement robot = hit.transform.GetComponent<RobotMovement>();
            if (robot != null)
            {
                robot.Damaged(damage);
            }

            Debug.DrawLine(muzzle.transform.position, hit.point, Color.red, 1.0f);

            splash.transform.position = hit.point;
            splash.transform.forward = hit.normal;
            splash.Emit(1);

            tracer.transform.position = hit.point;
        }

        else
        {
            Debug.DrawLine(muzzle.transform.position, ray.origin + cam.transform.forward * 100, Color.blue, 1.0f);
            tracer.transform.position = ray.origin + cam.transform.forward * 100;
        }

    }
}
