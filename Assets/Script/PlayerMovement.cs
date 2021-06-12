using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public GameObject gameover;

    public Rigidbody rb;
    private Animator anim;
    public GameObject enemy;

    private float spawntime = 0.5f;
    private float nextspawn;

    public float speed;
    public float jump;
    public bool ground = true;
    public bool right = false;

    public HealthBar healthBar;
    public float maxHealth = 100;
    public float currentHealth = 100;
    private float nextDamage;

    public GameObject heart;
    public ParticleSystem heartGain;
    public ParticleSystem heartCircle;
    public int heartScore;
    public int killScore;

    public GameOverScreen lose;

    public GameObject Robot;
    public GameObject HitSoundData;
    private AudioSource heartSound;
    private AudioSource jumpSound;
    private AudioSource damageSound;
    private AudioSource hitSound;

    public bool dead;

    void Start()
    {
        dead = false;
        Time.timeScale = 1;
        gameover.transform.localScale = new Vector3(0, 0, 0);
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();
        heartScore = 0;
        killScore = 0;
        heartSound = heart.GetComponent<AudioSource>();
        jumpSound =GetComponent<AudioSource>();
        damageSound = Robot.GetComponent<AudioSource>();
        hitSound = HitSoundData.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("d"))
        {
            anim.SetInteger("AnimationPar", 1);
        }


        else if (Input.GetKey("s"))
        {
            anim.SetInteger("AnimationPar", 2);
        }
        else
        {
            anim.SetInteger("AnimationPar", 0);
        }

        SpawnEnemy();

        PassiveDamage();

    }

    void FixedUpdate()
    {
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * speed, 0, Input.GetAxis("Vertical") * Time.deltaTime * speed);

        if (Input.GetButton("Jump") && ground)
        {
            jumpSound.Play();
            rb.AddForce(new Vector3(0, jump, 0), ForceMode.Impulse);
            ground = false;
            anim.SetBool("Air", true);
            anim.SetFloat("AnimSpeed", 0.4f);
        }

        if (rb.transform.position.y < -30 || currentHealth <= 0)
        {
            Death();
        }

        if(rb.transform.position.y < -5f)
        {
            anim.SetBool("Air", true);
            anim.SetFloat("AnimSpeed", 0.4f);
            ground = false;
        }

    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Enemy")
        {
            ground = true;
            anim.SetBool("Air", false);
            anim.SetFloat("AnimSpeed", 1f);
            if (right == true)
            {
                anim.SetBool("Right", true);
                right = false;
            }
            else
            {
                anim.SetBool("Right", false);
                right = true;
            }
        }

        if (collision.gameObject.tag == "Enemy")
        {
            hitSound.Play();
            currentHealth -= 10f;
            float dirx = collision.contacts[0].point.x - transform.position.x;
            float dirz = collision.contacts[0].point.z - transform.position.z;
            rb.velocity = Vector3.zero;
            rb.AddForce(-dirx*900, 0, -dirz*700);
            killScore += 1;
        }

        if (collision.gameObject.tag == "Robot")
        {
            damageSound.Play();
            currentHealth -= 30f;
            float dirx = collision.contacts[0].point.x - transform.position.x;
            float dirz = collision.contacts[0].point.z - transform.position.z;
            rb.velocity = Vector3.zero;
            rb.AddForce(-dirx * 2000, 0, -dirz * 2000);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Heart")
        {
            heartSound.Play();
            heartScore += 1;

            if (currentHealth < 50f)
            {
                currentHealth += 50f;
            }
            else
            {
                currentHealth = 100f;
            }
            heart.transform.position = new Vector3(Random.Range(-32.5f, 32.5f), 7f, Random.Range(10f, 75f));
            heartGain.Emit(10);
            heartCircle.Emit(1);
        }
    }

    void SpawnEnemy()
    {
        if (Time.time > nextspawn)
        {
            nextspawn = Time.time + spawntime;
            Instantiate(enemy);
        }
    }

    void PassiveDamage()
    {
        if (Time.time > nextDamage)
        {
            nextDamage = Time.time + 0.005f;
            currentHealth -= 0.01f;
            healthBar.SetHealth(currentHealth);
        }
    }

    private void Death()
    {
        dead = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameover.transform.localScale = new Vector3(1, 1, 1);
        Time.timeScale = 0;
    }

}
