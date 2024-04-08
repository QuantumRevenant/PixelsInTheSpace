using System.Collections;
using System.Collections.Generic;
using QuantumRevenant.GeneralNS;
using QuantumRevenant.Utilities;
using UnityEngine;

public class Scr_BulletPool : MonoBehaviour
{
    [SerializeField]private GameObject bulletPrefab;
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

    public void spawnBullet(Vector3 firePoint, float offset, ScO_Bullet bulletData, float angle,string tag)
    {
        angle = Utility.NormalizeAngle(angle);
        GameObject bullet = getBullet();

        bullet.transform.Rotate(Vector3.forward, angle);

        bullet.transform.position = firePoint;
        bullet.transform.Translate(Vector2.left * offset, Space.Self);

        bullet.GetComponent<Scr_Bullet>().BulletData = bulletData;

        if(Utility.DoesTagExist(tag))
            bullet.tag = gameObject.tag;
        else
            bullet.tag=Tags.NeutralTeam;

        bullet.SetActive(true);
    }
}