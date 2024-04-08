using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuantumRevenant.Utilities;
using QuantumRevenant.PixelsinTheSpace;
using UnityEditor;

public class Scr_Entity : MonoBehaviour
{
    [Header("General")]
    private DamageTypes resistances;
    private float generalTimer;
    [Header("Armor")]
    private int maxArmor;
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
    [SerializeField]private ScO_ShotAtributtes shotAtributtes;
    [SerializeField]private GameObject firePoint;
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
    public virtual void Think()
    {
        generalTimer += Time.deltaTime;
    }
    #endregion

    #region Armor
    private void addArmor(int value = 1)
    {
        armor += value;
        armor = Mathf.Clamp(armor, 0, maxArmor);
    }
    private void reduceArmor(int value = 1)
    {
        armor -= value;
        armor = Mathf.Clamp(armor, 0, maxArmor);
    }
    #endregion

    #region Health
    public void heal(float value)
    {
        actualHP += value;
        actualHP = Mathf.Clamp(actualHP, 0, maxHP);
    }
    private void hurt(float value)
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
    public void hurt(Damage damage)
    {
        if (isResisted(damage.type)) { return; }

        if (resistances == DamageTypes.Standard)
            hurt(damage.value);
        else
            hurt(damage.value * 2);

    }
    protected virtual void death() { }
    private bool isResisted(DamageTypes type)
    {
        return resistances.HasFlag(type) && type != DamageTypes.Standard;
    }
    #endregion

    #region Invulnerability
    private void setInvulnerable(bool value) { isInvulnerable = value; }
    private void switchInvulerable() { isInvulnerable = !isInvulnerable; }
    private void ActivateInvulnerability(float duration)
    {
        if (!isInvulnerable)
            StartCoroutine(TemporalInvulnerability(duration));
    }
    private void ActivateInvulnerability()
    {
        ActivateInvulnerability(invulnerabilityTime);
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
        if (!isOffLimits())
        {
            entityStatus = EntityStatus.PlayArea;
            return;
        }
        if (entityStatus == EntityStatus.PlayArea)
            entityStatus = EntityStatus.Exiting;
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
    [ContextMenu("Shoot")]
    private void spawnBullet()
    {
        ScO_Bullet bullet = shotAtributtes.Bullet;
        Vector3 firePointPos = firePoint == null ? gameObject.transform.position : firePoint.transform.position;
        Vector3 gameObjectPos = gameObject.transform.position;

        for (int i = 0; i < shotAtributtes.ProjectileQuantity; i++)
        {
            float angleArc = 0;

            if (shotAtributtes.ProjectileQuantity != 1)
            {
                float limit = shotAtributtes.FiringArc / 2;
                float percentage = (float)i / (shotAtributtes.ProjectileQuantity - 1);
                angleArc = Mathf.Lerp(limit, -limit, percentage);
            }

            float angleOffset = shotAtributtes.AngularOffset + (shotAtributtes.OffsetSpeed * generalTimer);
            angleOffset = Utility.NormalizeAngle(angleOffset);

            if (firePointPos != gameObjectPos)
                firePointPos = Utility.rotatePoint3DRelativeToPivotZ(firePointPos, gameObjectPos, angleArc + angleOffset);

            Scr_BulletPool.Instance.spawnBullet(firePointPos, 0, bullet, angleArc + angleOffset,gameObject.tag);
        }
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

    private void DrawGizmosFiringArea(float angle)
    {
        Utility.NormalizeAngle(angle);
    }

}
