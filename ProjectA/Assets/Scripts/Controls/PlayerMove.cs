using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CooldownTimer;

//TODO: merge this with UsePrimary
public partial class PlayerMove : MonoBehaviour
{
    private const float deadZone = .1f;
    private static Player player;
    public Rigidbody2D rb;

    public float defaultSpeed = 5;
    public float playerSpeed;
    public float diveDur;
    public float diveCoef;
    public Cooldown jumpCooldown;

    public Cooldown walkSoundCooldown;
    public AudioClip[] walkSounds;

    private Animator anim;

    private void Start()
    {
        playerSpeed = defaultSpeed;

        anim = GetComponent<Animator>();
        player = Player.Instance;
    }

    // Update is called once per frame
    float xAxis;
    float yAxis;
    float xAxisRaw;
    float yAxisRaw;
    bool dodge;
    Vector2 facingDir;

    void Update()
    {
        //pause guard
        if (PauseController.Paused) return;

        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        dodge = Input.GetButtonDown("Jump");

        //Check Raw inputs, keyboard only
        xAxisRaw = Input.GetAxisRaw("Horizontal");
        yAxisRaw = Input.GetAxisRaw("Vertical");

        if (dodge && jumpCooldown.IsReady)
        {
            //Store last control input for anim facing dir
            if (xAxisRaw != 0 || yAxisRaw != 0)
            {
                anim.SetFloat("xInput", xAxisRaw);
                anim.SetFloat("yInput", yAxisRaw);
                facingDir.Set(xAxisRaw, yAxisRaw);
            }

            SetPath(Boomerang.Mult(defaultSpeed * diveCoef, RollPath(facingDir.normalized)), diveDur);
            player.damageInvuln.Use();
            jumpCooldown.Use();

            //Anim parameter updates
            anim.SetTrigger("Dodge");
           
        }
        //Set looking vals
        Vector2 lookDir = (CameraReference.Instance.camera.ScreenToWorldPoint(Input.mousePosition) - Player.Instance.gameObject.transform.position).normalized;
        anim.SetFloat("aimX", lookDir.x);
        anim.SetFloat("aimY", lookDir.y);
    }

    private void FixedUpdate()
    {
        if (PathEnd)
        {
            //Store last control input for anim facing dir
            if (xAxisRaw != 0 || yAxisRaw != 0)
            {
                anim.SetFloat("xInput", xAxisRaw);
                anim.SetFloat("yInput", yAxisRaw);
                facingDir.Set(xAxisRaw, yAxisRaw);
            }

            //Set moving state for animator
            if (Mathf.Abs(xAxis) >= deadZone || Mathf.Abs(yAxis) >= deadZone)
            {
                anim.SetBool("isMoving", true);
            } 
            else
            {
                anim.SetBool("isMoving", false);
            }

            rb.velocity = Vector2.ClampMagnitude(new Vector2(xAxis, yAxis), 1) * defaultSpeed;
            if (rb.velocity.sqrMagnitude > .1f && walkSoundCooldown.IsReady)
            {
                player.PlaySound(walkSounds[UnityEngine.Random.Range(0, walkSounds.Length)]);
                walkSoundCooldown.Use();
            }
        }
        else
        {
            //velocity is determined purely by the path if I am in a path
            Vector2 v = GetPathVel();
            rb.velocity = v;
        }
    }
}
