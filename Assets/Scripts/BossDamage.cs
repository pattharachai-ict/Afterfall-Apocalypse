using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamage : MonoBehaviour
{

    public int damage = 50;
    private HealthBar health;

    //Start is called before the first frame update
    void Start()
    {
        
    }

    //Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(health == null)
            {
                health = collision.gameObject.GetComponent<HealthBar>();
            }
            health.TakeDamage(damage);
        }
    }
}