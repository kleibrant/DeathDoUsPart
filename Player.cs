 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    public float health = 100f;
    [SerializeField] private const float decrease = 1f;

    private bool isHiding;
    [SerializeField] private Collider tableCollider;
    [SerializeField] private Collider ghostReachCollider;
    [SerializeField] private Collider deadlyCollider;

    private bool closeToGhost;
    private bool isDead;

    //color adjustment vars
    private ColorAdjustments col;
    private Volume visualHealth;

    private AudioSource found;

    // Start is called before the first frame update
    void Start()
    {
        //Volume visualHealth = GetComponent<Volume>();
       // visualHealth.profile.TryGet<ColorAdjustments>(out col);
    }
    // Update is called once per frame
    void Update()
    {
        AudioSource found = GetComponent<AudioSource>();

        Volume visualHealth = GetComponent<Volume>();
        visualHealth.profile.TryGet<ColorAdjustments>(out col);

        HealthDecrease(decrease);
        Debug.Log(health);
        
        if (health <= 50)
        {
            Debug.Log("FIFTY PERCENT!!!");
            visualHealth.enabled = true;
            col.saturation.value = 0;
        }
        if (health <= 25)
        {
            Debug.Log("Low health");
            col.saturation.value = 100;
        }
        if (health <= 0)
        {
           isDead = true;
            Debug.Log("You died");
        }

        //searching for the different triggers in unity
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0f);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider == ghostReachCollider)
            {
                closeToGhost = true;
                Debug.Log("GHOST!!!");
            }
            if (hitCollider == deadlyCollider)
            {
                isDead = true;
                found.Play();
                Debug.Log("the ghost caught you");
            }
            if (hitCollider == tableCollider)
            {
                isHiding = true;
                Debug.Log("you're hiding");
                if (isHiding == true)
                {
                    closeToGhost = false;
                    Debug.Log("you can pretend the ghost is not there");
                }
            }

            if (isDead == true)
            {
                SceneManager.LoadScene("Game Over");
            }
        }
    }
    public void HealthDecrease(float decrease)
    {
        health -= decrease * Time.deltaTime;
        if (closeToGhost == true)
        {
            decrease = decrease + 2;
            Debug.Log("More decrease due to ghost..");
        }
    }
}
