using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Scr_Bullet : MonoBehaviour
{
    [SerializeField] private ScO_Bullet bulletData;
    [SerializeField] private float timerActive = float.PositiveInfinity;
    public ScO_Bullet BulletData
    {
        get { return bulletData; }
        set
        {
            bulletData = value;
            UpdateProperties();
        }
    }
    void Start()
    {
        if (BulletData != null)
        {
            UpdateProperties();
        }
    }
    void Update()
    {
        timerDeactivate();
        Movement();
    }
    private void UpdateProperties()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = bulletData.Sprite;
        gameObject.GetComponent<SpriteRenderer>().color = bulletData.Color;
        transform.rotation = Quaternion.Euler(0, 0, bulletData.Angle);
        transform.localScale = new Vector3(bulletData.Scale, bulletData.Scale, bulletData.Scale);
        timerActive = bulletData.LifeTime;
    }

    private void timerDeactivate()
    {
        timerActive -= Time.deltaTime;
        if (timerActive <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void onDrawGizmos()
    {
        float radius = 0;
        if (bulletData != null)
        {
            radius = bulletData.AoeRadius;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Movement()
    {
        Vector3 translateMovement = Vector3.up * bulletData.Speed * Time.deltaTime;
        transform.Translate(translateMovement, Space.Self);
        RotateObject(bulletData.AngularSpeed);
    }

    private void RotateObject(float angularSpeed)
    {
        float angle = transform.localRotation.eulerAngles.z;
        angle += angularSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
