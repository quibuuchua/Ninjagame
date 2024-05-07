using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed=5;
    [SerializeField] private Animator anim;
    private bool isGrounded=true;
    private bool isJumping=false;
    private bool isAttack=false;
    private float horizontal;
    private string currentAnim;
   [SerializeField] private float jumpFocre =350;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = isCheckedGrounDed();
        //-1 -> 0 ->1
        horizontal = Input.GetAxisRaw("Horizontal");
        if (isAttack)
        {
            return;
        }
        if (isGrounded) {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            Jump();
            if (isGrounded && rb.velocity.y < 0)
            {
                ChangeAnim("fall");
                isJumping=false;
            }
            //attack
            if(Input.GetKeyDown (KeyCode.C) && isGrounded)
            Attack();
            //throw
            if(Input.GetKeyDown(KeyCode.V)&& isGrounded)
            Throw(); 

            if(Mathf.Abs(horizontal)>0.1f)
            {

                ChangeAnim("run");
            }
            if (isJumping)
            {
                return;
            }
        }
        

        if(Mathf.Abs(horizontal)> 0.1f)
        {
           
            rb.velocity = new Vector2(horizontal*Time.fixedDeltaTime*speed,rb.velocity.y);
            // transform.localScale = new Vector3(horizontal, 1, 1);
            //
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0:180 , 0));
        }
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }

    }

    private bool isCheckedGrounDed()
    {
        //Debug.DrawLine(transform.position,transform.position+Vector3.down *1.1f, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f,groundLayer);
        // su dung tia ray cast ban xuong ground 
        if(hit.collider !=null) return true;
        else return false;
    }
    private void Attack()
    {
        rb.velocity = Vector2.zero;
        ChangeAnim("attack");
        isAttack=true;
        Invoke(nameof(ResetAttack), 0.5f);
    }
    private void Throw()
    {
        rb.velocity = Vector2.zero;
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }
    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpFocre * Vector2.up);
    }
    private void ChangeAnim(string animName)
    {
        if(currentAnim !=animName)
        {
            anim.ResetTrigger(animName);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);  
        }
    }
    private void ResetAttack()
    {
        isAttack=false;
        ChangeAnim("ilde");
    }
}
