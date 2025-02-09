using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float acclerate;
    public float MaxSpeed;

    bool isMoving;
    bool isUsingSkill1=false,isUsingSkill2=false;
    Rigidbody rb;
    Vector2 movingDirection = Vector2.zero;

    EquipSkillBase[] equipSkills =new EquipSkillBase[2];


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //测试用，初始化两个绳在身上
        equipSkills[0]=new Skill_RopeTest();
        equipSkills[0].OnEquip(this);
        equipSkills[1]=new Skill_RopeTest();
        equipSkills[1].OnEquip(this);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovingState();

        if (movingDirection != Vector2.zero)
        {
            Debug.Log("Moving!");
            OnMove();
        }

        if (Input.GetMouseButton(0))
        {
            if (!isUsingSkill1)
            {
                equipSkills[0].OnBeginUse();
            }
            else
            {
                equipSkills[0].OnUse();
            }
            isUsingSkill1 = true;
        }
        else
        {
            if(isUsingSkill1)
                equipSkills[0].OnEndUse();

            isUsingSkill1=false;
        }

    }

    void UpdateMovingState()
    {
        movingDirection = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            movingDirection.y = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movingDirection.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movingDirection.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movingDirection.x = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump();
        }


    }

    void OnMove()
    {
        Vector3 targetVel = new Vector3(movingDirection.x, 0, movingDirection.y) * MaxSpeed;
        rb.velocity = Vector3.MoveTowards(rb.velocity, targetVel, acclerate * Time.deltaTime);
    }
    
     void OnJump()
     {
        rb.AddForce(new Vector3(0,300, 0));
     }


}
