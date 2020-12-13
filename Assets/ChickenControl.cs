using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenControl : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;
    bool isJumping;

    public float maxHoldTime = 0.25f;
    public float jumpForce = 5;
    float holdTime;

    public ParticleSystem particleSys;
    bool isLanding;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isJumping = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameController.Instance.isRunning&&!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            GameController.Instance.isRunning = true;
        }
        if (!particleSys.isPlaying && animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            particleSys.Play();
        }
        else if (particleSys.isPlaying && !animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) particleSys.Stop();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !isJumping)
        {
            Jump();
        }

        if (isJumping && (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && holdTime > 0)
        {
            rigid.velocity = Vector2.up * jumpForce;
            holdTime -= Time.deltaTime;
        }

        if (isJumping && (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0)))
        {
            holdTime = 0;
        }

        //if (rigid.velocity.y >= 0) rigid.gravityScale = 1f;
        //else
        //{
        //    rigid.gravityScale = 1.25f;
        //}


        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 5, 1 << LayerMask.NameToLayer("Obstacle"));

        if (hit.collider != null)
        {
            CatScript cs = hit.transform.GetComponent<CatScript>();
            GameController.Instance.AddPoint(cs.point);
            cs.point = 0;
        }

        hit = Physics2D.Raycast(transform.position, Vector2.down, 5, 1 << LayerMask.NameToLayer("Obstacle"));

        if (hit.collider != null)
        {
            CatScript cs = hit.transform.GetComponent<CatScript>();
            GameController.Instance.AddPoint(cs.point);
            cs.point = 0;
        }


        if (isJumping && !isLanding && rigid.velocity.y < 0 && Physics2D.Raycast(transform.position, Vector2.down, 1f, 1 << LayerMask.NameToLayer("Floor")).collider != null)
        {
            isLanding = true;
            animator.SetTrigger("Land");
        }
    }

    void Jump()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            rigid.velocity = Vector2.up * jumpForce;
            isJumping = true;
            isLanding = false;
            holdTime = maxHoldTime;
            animator.SetTrigger("Fly");
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (isJumping && col.gameObject.layer == 9)
        {
            // hit floor
            isJumping = false;
            //animator.SetTrigger("Land");
        }
        if (col.gameObject.layer == 10)
        {
            // hit a cat
            GameController.Instance.GameOver();
        }
    }
}
