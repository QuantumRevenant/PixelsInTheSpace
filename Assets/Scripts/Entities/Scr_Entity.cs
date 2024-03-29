using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityStatus { Entering, PlayArea, Exiting }
public enum Team { Neutral, Player, Enemy, Other }
public class Scr_Entity : MonoBehaviour
{
    [Header("General")]
    private Team team;
    private float generalTimer;
    [Header("Armor")]
    private int armor;
    [Header("Health")]
    private float maxHP;
    private float actualHP;
    [Header("Invulnerability")]
    private bool isInvulnerable = false;
    private float invulnerabilityTime;
    [Header("Movement")]
    private EntityStatus entityStatus;
    private Vector2 speedMovement;
    private float speedMultiplier;
    private Vector2 boundaries;
    private int currentWaypoint = 0;
    private Transform[] waypoints;
    private float waitTime;
    private bool isWaiting = false;
    [Header("Damage")]
    private ScO_ShotAtributtes shotAtributtes;
    private GameObject firePoint;
    private float reloadTime;
    [Header("Gizmos")]
    private GizmosShoot gizmosShoot;
    private struct GizmosShoot
    {
        float angle;
        float offset;
        float radius;
        float internalRadius;
    }
    
    
    #region General
    
    public void GeneralUpdate()
    {
        generalTimer+=Time.deltaTime;
    }
    #endregion
    
    #region Armor
    public void addArmor(int value = 1)
    {
        armor += value;
        armor = Mathf.Clamp(armor, 0, 10);
    }
    public void reduceArmor(int value = 1)
    {
        armor -= value;
        armor = Mathf.Clamp(armor, 0, 10);
    }
    #endregion

    #region Health
    public void heal(float value)
    {
        actualHP += value;
        actualHP = Mathf.Clamp(actualHP, 0, maxHP);
    }
    public void hurt(float value)
    {
        if (isInvulnerable) { return; }
        if (armor > 0)
        {
            reduceArmor();
            return;
        }

        actualHP -= value;
        actualHP = Mathf.Clamp(actualHP, 0, maxHP);
        if (actualHP <= 0)
            death();
    }
    protected virtual void death() { }
    #endregion

    #region Invulnerability
    public void setInvulnerable(bool value) { isInvulnerable = value; }
    public void switchInvulerable() { isInvulnerable = !isInvulnerable; }
    public void ActivateInvulnerability()
    {
        if (!isInvulnerable)
            StartCoroutine(TemporalInvulnerability(invulnerabilityTime));
    }
    public void ActivateInvulnerability(float duration)
    {
        if (!isInvulnerable)
            StartCoroutine(TemporalInvulnerability(duration));
    }
    protected virtual void invulnerableAction() { }
    protected virtual IEnumerator TemporalInvulnerability(float duration)
    {
        setInvulnerable(true);
        invulnerableAction();
        yield return new WaitForSeconds(duration);
        setInvulnerable(false);
    }
    #endregion

    #region Movement
    protected void moveWaypoint(bool random = false)
    {
        moveWaypoint(random, waitTime);
    }
    protected void moveWaypoint(bool random, float time)
    {
        if (transform.position != waypoints[currentWaypoint].position)
        {
            Vector2 direction = (waypoints[currentWaypoint].position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position, (speedMovement * direction).magnitude * Time.deltaTime);
        }
        else if (!isWaiting)
        {
            if (waypoints.Length <= 1)
            {
                Debug.Log("Error - Insuficientes Waypoints, Tenemos 1 o menos");
                return;
            }
            StartCoroutine(Wait(random, time));
        }
    }
    protected IEnumerator Wait(bool random, float time)
    {
        isWaiting = true;
        yield return new WaitForSeconds(time);
        if (random)
        {
            currentWaypoint = Random.Range(0, waypoints.Length);
        }
        else
        {
            currentWaypoint++;
            if (currentWaypoint == waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }
        isWaiting = false;
    }
    protected virtual void Move(Vector2 direction)
    {
        Move(direction, speedMovement, speedMultiplier);
    }
    protected virtual void Move(Vector2 direction, Vector2 speed, float multiplier)
    {
        Vector2 vector2D = direction.normalized * speed * multiplier * Time.deltaTime;
        Vector3 vector3D = new Vector3(vector2D.x, vector2D.y);
        transform.Translate(vector3D);
    }
    protected virtual void limitPosition()
    {
        limitPosition(boundaries);
    }
    protected virtual void limitPosition(Vector2 limits)
    {
        limitPosition(-limits, limits);
    }
    protected virtual void limitPosition(Vector2 downLeft, Vector2 upRight)
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, downLeft.x, upRight.x), Mathf.Clamp(transform.position.y, downLeft.y, upRight.y));
    }
    private void changeEntityStatus()
    {
        if (isOffLimits())
        {
            if (entityStatus == EntityStatus.PlayArea)
                entityStatus = EntityStatus.Exiting;
        }
        else
        {
            entityStatus = EntityStatus.PlayArea;
        }
    }
    private bool isOffLimits()
    {
        return isOffLimits(boundaries);
    }
    private bool isOffLimits(Vector2 limits)
    {
        return isOffLimits(-limits, limits);
    }
    private bool isOffLimits(Vector2 downLeft, Vector2 upRight)
    {
        return transform.position.x < downLeft.x || transform.position.x > upRight.x || transform.position.y > upRight.y || transform.position.y < downLeft.y;
    }
    #endregion

    #region Damage
    private Vector3 rotateFirePoint(Vector3 firePointPos, float angle, float offset)
    {
        Vector2 posPlayer = new Vector2(transform.position.x, transform.position.y);
        Vector2 posFirePoint = new Vector2(firePointPos.x, firePointPos.y);
        Vector2 output;

        float angleRadians = angle * Mathf.Deg2Rad;

        // Traduce el sistema de coordenadas
        Vector2 translatedPoint = posFirePoint - posPlayer;

        // Aplica la rotación
        float x_prime = translatedPoint.x * Mathf.Cos(angleRadians) - translatedPoint.y * Mathf.Sin(angleRadians);
        float y_prime = translatedPoint.x * Mathf.Sin(angleRadians) + translatedPoint.y * Mathf.Cos(angleRadians);

        // Traduce de nuevo al sistema original
        output = new Vector2(x_prime, y_prime) + posPlayer;
    
        return output;
    }
    private void spawnBullet()
    {
        ScO_Cartridge cartridge = shotAtributtes.Cartridge;

        Vector3 firePointPos;
        float offset;

        for (int i = 0; i < shotAtributtes.RoundsFired; i++)
        {
            float angleArc = 0;
            if (firePoint != null)
                firePointPos = firePoint.transform.position;
            else
                firePointPos = gameObject.transform.position;

            if (shotAtributtes.RoundsFired != 1)
            {
                float limit = shotAtributtes.FiringArc / 2;
                float percentage = (float)i / (shotAtributtes.RoundsFired - 1);
                angleArc = Mathf.Lerp(limit, -limit, percentage);
            }
            float angleOffset=shotAtributtes.Offset+(shotAtributtes.OffsetSpeed*generalTimer);
            angleOffset%=360;
            firePointPos = rotateFirePoint(firePointPos, angleArc,angleOffset);
            for (int j = 0; j < cartridge.bulletsQuantity; j++)
            {
                offset = 0;
                if (cartridge.bulletsQuantity != 1)
                {
                    float limit = cartridge.Separation / 2;
                    float percentage = (float)j / (cartridge.bulletsQuantity - 1);
                    offset = Mathf.Lerp(limit, -limit, percentage);
                }
                spawnBullet(firePointPos, offset, cartridge.Bullets[i],angleArc+angleOffset);
            }
        }
    }

    private void spawnBullet(Vector3 firePoint, float offset, ScO_Bullet bulletData, float angle)
    {
        angle%=360;
        GameObject bullet = Scr_BulletPool.Instance.getBullet();
        bullet.transform.Rotate(Vector3.forward,angle);
        bullet.transform.position = firePoint;
        bullet.transform.Translate(Vector2.left * offset, Space.Self);
        bullet.GetComponent<Scr_Bullet>().BulletData = bulletData;
    }

    #endregion

    private void OnDrawGizmos()
    {
        // // Asegúrate de que el ángulo esté en el rango [0, 360]
        // angulo = Mathf.Clamp(angulo, 0f, 360f);

        // // Calcula los puntos que forman el sector circular
        // Vector3 origen = transform.position;
        // Vector3 puntoA = origen + Quaternion.Euler(0, 0, -angulo / 2) * Vector3.right * radio;
        // Vector3 puntoB = origen + Quaternion.Euler(0, 0, angulo / 2) * Vector3.right * radio;

        // // Dibuja el arco del sector
        // Gizmos.color = Color.green; // Puedes cambiar el color
        // Gizmos.DrawLine(origen, puntoA);
        // Gizmos.DrawLine(origen, puntoB);
        // Gizmos.DrawWireArc(origen, Vector3.forward, puntoA - origen, angulo, radio);

        // // Dibuja los radios
        // Gizmos.color = Color.red; // Puedes cambiar el color
        // Gizmos.DrawLine(origen, puntoA);
        // Gizmos.DrawLine(origen, puntoB);
    }

    private void DrawGizmosFiringArea(float angle )
    {
        Utilities.NormalizeAngle(angle);
    }

}
