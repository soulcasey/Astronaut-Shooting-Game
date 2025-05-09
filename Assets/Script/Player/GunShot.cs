using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShot : MonoBehaviour
{
    public AudioSource shootSound;

    public GameObject muzzle;

    public ParticleSystem[] flash;
    public ParticleSystem splash;
    public TrailRenderer trace;

    private float lastFire = -Mathf.Infinity;
    private const float FIRE_RATE = 0.15f;
    private const float FIRE_DAMAGE = 1f;

    public void Shoot()
    {
        if (Time.time < lastFire + FIRE_RATE) return; 
        lastFire = Time.time;

        shootSound.Play();

        foreach(var particle in flash)
        {
            particle.Emit(1);
        }

        Camera cam = Camera.main;

        Vector3 origin = cam.transform.position;
        Vector3 direction = cam.transform.forward;

        var tracer = Instantiate(trace, muzzle.transform.position, Quaternion.identity);
        tracer.AddPosition(muzzle.transform.position);

        if (Physics.Raycast(origin, direction, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable) == true)
            {
                damageable.OnHit(FIRE_DAMAGE);
            }

            splash.transform.position = hit.point;
            splash.transform.forward = hit.normal;
            splash.Emit(1);

            tracer.transform.position = hit.point;
        }
        else
        {
            tracer.transform.position = origin + direction * 100;
        }

        Debug.DrawLine(muzzle.transform.position, tracer.transform.position, Color.blue, 1.0f);

        Destroy(tracer.gameObject, 1f);
    }
}
