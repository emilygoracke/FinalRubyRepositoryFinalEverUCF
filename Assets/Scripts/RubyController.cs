using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;

    public int score;

    public int Cog;

    public TextMeshProUGUI CogText;

    public TextMeshProUGUI scoreText;

    public GameObject projectilePrefab;

    public GameObject WinTextObject;

    public GameObject LostTextObject;

    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip collectedClip;
    public AudioClip cogClip;

    public AudioSource Background;

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;

    float horizontal;
    float vertical;

    Animator animator;

    Vector2 lookDirection = new Vector2(1, 0);


    public Transform hitEffect;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        hitEffect.GetComponent<ParticleSystem>().enableEmission = false;

        audioSource = GetComponent<AudioSource>();

        WinTextObject.SetActive(false);
        LostTextObject.SetActive(false);

        Background.Play();

        score = 0;
        SetScoreText();

        Cog = 4;
        SetCogText();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Cog > 0)
            {
                Launch();
            }

        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                if (score == 4)
                {
                    SceneManager.LoadScene("Level 2");
                }
                else
                {
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        character.DisplayDialog();

                    }
                }
            }
        }
    }


    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            PlaySound(hitSound);
        }
        if (currentHealth == 0)
        {
            LostTextObject.SetActive(true);
            Background.Stop();
            Destroy(this);
            GetComponent<Rigidbody>().Sleep();
        }

        hitEffect.GetComponent<ParticleSystem>().enableEmission = true;
        StartCoroutine(stopHitEffect());
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
    public void ChangeScore(int scoreAmount)
    {
        score += scoreAmount;
        SetScoreText();
    }
    void SetScoreText()
    {
        scoreText.text = "Robots Fixed: " + score.ToString();

        if (score >= 4)
        {
            WinTextObject.SetActive(true);
            Background.Stop();

        }
    }
    void SetCogText()
    {
        CogText.text = "Cogs: " + Cog.ToString();

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cogs"))
        {
            other.gameObject.SetActive(false);
            Cog = Cog + 4;
            SetCogText();
            PlaySound(cogClip);
        }
    }
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Cog = Cog + -1;
        SetCogText();

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);



        animator.SetTrigger("Launch");

        PlaySound(throwSound);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void OnTriggerEnter2D()
    {
        hitEffect.GetComponent<ParticleSystem>().enableEmission = true;
        StartCoroutine(stopHitEffect());
    }

    IEnumerator stopHitEffect()
    {
        yield return new WaitForSeconds(0.4f);
        hitEffect.GetComponent<ParticleSystem>().enableEmission = false;
    }

}