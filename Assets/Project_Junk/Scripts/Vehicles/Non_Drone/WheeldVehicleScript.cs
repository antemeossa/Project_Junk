using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheeldVehicleScript : MonoBehaviour
{
    [SerializeField]
    private GameObject wheels_F, wheels_B;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 waitPos;
    [SerializeField]
    private int lifeTime;

    private int currentLife;

    private bool moving=false, canMove=false;

    private void Update()
    {
        
        move();
    }

    private void Start()
    {
        InvokeRepeating("lifeTimeCounter", 0, 1);
    }
    private void move()
    {
        if (canMove)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
            moving = true;

        }

    }

    

    private void lifeTimeCounter()
    {
        if(moving)
        {
            currentLife++;
            if(currentLife >= lifeTime)
            {
                transform.position = waitPos;
                moving = false;
                currentLife = 0;
                canMove = false;
            }
        }
        
    }

    public bool getMoving { get { return moving; } }
    public void setCanMove(bool boolMove) { canMove = boolMove; }

    public void setMoveRot(int i) { speed *= i; }
}
