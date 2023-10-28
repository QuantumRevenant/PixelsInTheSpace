using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_BulletPool : MonoBehaviour
{
    private GameObject bulletPrefab;
    private int initialPoolSize = 10;
    private List<GameObject> bullets;

    private static Scr_BulletPool instance;
    public static Scr_BulletPool Instance { get => instance;}
    
    private void Awake()
    {
        if(instance==null)
        {
            instance=this;
        }else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        bullets = new List<GameObject>();
        for (int i = 0; i < initialPoolSize; i++)
        {
            createNewBullet();
        }

    }

    private GameObject createNewBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform);
        bullet.SetActive(false);
        bullets.Add(bullet);
        bullet.transform.parent = transform;
        return bullet;
    }
    public GameObject getBullet()
    {
        for(int i=0;i<bullets.Count;i++)
        {
            if(!bullets[i].activeInHierarchy)
            {
                return bullets[i];
            }
        }
        
        return createNewBullet();
    }
}