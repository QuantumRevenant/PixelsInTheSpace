using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuantumRevenant.Utilities;
using QuantumRevenant.PixelsinTheSpace;
using QuantumRevenant.PixelsinTheSpace.Multiplier;
using UnityEditor;
using QuantumRevenant.CustomEditors;
public class Scr_Entity : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private DamageTypes typeResistance;
    private DamageTypes previousResistance;
    private float generalTimer;

    [Header("Armor")]
    [SerializeField] private int armor;

    [Header("Health")]
    [SerializeField] private float health;

    [Header("Invulnerability")]
    private bool isInvulnerable = false;

    [Header("Movement")]
    [SerializeField] private Transform[] waypoints;
    private EntityStatus entityStatus;
    private int currentWaypoint = 0;
    private bool isWaiting = false;

    [Header("Damage")]
    [SerializeField] private GameObject firePoint;
    [ExposedScriptableObject]
    [SerializeField] private ScO_ShotAtributtes shotAtributte;
    [SerializeField] private List<ScO_ShotAtributtes> availableShotAtributtes;

    [Header("Multipliers")]
    [SerializeField] private StandardMultiplier multipliers = StandardMultiplier.OneMultiplier();

    [Header("Data")]
    [ExposedScriptableObject]
    [SerializeField] private ScO_Entity entityData;
    private readonly StandardMultiplier neutralChangeStats = StandardMultiplier.CreateMultiplier(1, 1, 1, 1, 2);
    [Header("Gizmos")]
    [ExposedScriptableObject]
    [SerializeField] private ScO_Gizmos gizmosData;
    public DamageTypes TypeResistance { get { return typeResistance; } set { typeResistance = value; VerifyResistance(); } }


    #region General
    public virtual void Think()
    {
        generalTimer += Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        Think();
    }

    private void Awake()
    {
        if (shotAtributte != null && !availableShotAtributtes.Find(shotAtt => shotAtt == shotAtributte))
            availableShotAtributtes.Add(shotAtributte);

        if (shotAtributte == null && availableShotAtributtes.Count != 0)
            shotAtributte = availableShotAtributtes[0];
    }
    #endregion

    #region Armor
    private void AddArmor(int value = 1)
    {
        armor += value;
        armor = Mathf.Clamp(armor, 0, entityData.MaxArmor);
    }
    #endregion

    #region Health
    public void VerifyResistance()
    {
        if (previousResistance == typeResistance)
            return;

        if (previousResistance == DamageTypes.Neutral)
            multipliers /= neutralChangeStats;
        else if (typeResistance == DamageTypes.Neutral)
            multipliers *= neutralChangeStats;

        previousResistance = typeResistance;
    }
    public void Heal(float value)
    {
        health += value;
        health = Mathf.Clamp(health, 0, entityData.MaxHealth);
    }
    private void Hurt(float value)
    {
        if (isInvulnerable) { return; }
        if (armor > 0) { AddArmor(-1); return; }

        health -= value;
        health = Mathf.Clamp(health, 0, entityData.MaxHealth);
        if (health <= 0)
            Death();
    }
    public void Hurt(Damage damage)
    {
        if (IsResisted(damage.type)) { return; }

        VerifyResistance();

        Hurt(damage.value / multipliers.resistance);
    }
    protected virtual void Death() { }
    private bool IsResisted(DamageTypes type)
    {
        return typeResistance.HasFlag(type) && type != DamageTypes.Neutral;
    }
    #endregion

    #region Invulnerability
    private void SetInvulnerable(bool value) { isInvulnerable = value; }
    private void ActivateInvulnerability(float duration)
    {
        if (duration == 0)
            return;
        if (!isInvulnerable)
            StartCoroutine(TemporalInvulnerability(duration));
    }
    private void ActivateInvulnerability()
    {
        ActivateInvulnerability(entityData.InvulnerabilityTime);
    }
    protected virtual void InvulnerableAction() { }
    protected virtual IEnumerator TemporalInvulnerability(float duration)
    {
        SetInvulnerable(true);
        InvulnerableAction();
        yield return new WaitForSeconds(duration);
        SetInvulnerable(false);
    }
    #endregion

    #region Movement
    protected void MoveWaypoint(bool random = false)
    {
        MoveWaypoint(random, entityData.WaitTimeWaypoints);
    }
    protected void MoveWaypoint(bool random, float time)
    {
        if (transform.position != waypoints[currentWaypoint].position)
        {
            Vector2 direction = (waypoints[currentWaypoint].position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position, (entityData.SpeedMovement * direction).magnitude * Time.fixedDeltaTime);
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
        Move(direction, entityData.SpeedMovement, multipliers.speed);
    }
    protected virtual void Move(Vector2 direction, Vector2 speed, float multiplier)
    {
        Vector2 vector2D = direction.normalized * speed * multiplier * Time.fixedDeltaTime;
        Vector3 vector3D = new Vector3(vector2D.x, vector2D.y);
        transform.Translate(vector3D);
    }
    protected virtual void LimitPosition()
    {
        LimitPosition(entityData.Boundaries);
    }
    protected virtual void LimitPosition(Vector2 limits)
    {
        LimitPosition(-limits, limits);
    }
    protected virtual void LimitPosition(Vector2 downLeft, Vector2 upRight)
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, downLeft.x, upRight.x), Mathf.Clamp(transform.position.y, downLeft.y, upRight.y));
    }
    private void ChangeEntityStatus()
    {
        if (!IsOffLimits())
        {
            entityStatus = EntityStatus.PlayArea;
            return;
        }
        if (entityStatus == EntityStatus.PlayArea)
            entityStatus = EntityStatus.Exiting;
    }
    private bool IsOffLimits()
    {
        return IsOffLimits(entityData.Boundaries);
    }
    private bool IsOffLimits(Vector2 limits)
    {
        return IsOffLimits(-limits, limits);
    }
    private bool IsOffLimits(Vector2 downLeft, Vector2 upRight)
    {
        return transform.position.x < downLeft.x || transform.position.x > upRight.x || transform.position.y > upRight.y || transform.position.y < downLeft.y;
    }
    #endregion

    #region Damage
    [ContextMenu("Shoot")]
    [MyBox.ButtonMethod]
    private void SpawnBullet()
    {
        ScO_Bullet bullet = shotAtributte.Bullet;
        Vector3 gameObjectPos = gameObject.transform.position;

        for (int i = 0; i < shotAtributte.ProjectileQuantity; i++)
        {
            Vector3 firePointPos = firePoint == null ? gameObject.transform.position : firePoint.transform.position;
            float angleArc = 0;
            float lateralOffset = 0;

            if (shotAtributte.ProjectileQuantity != 1)
            {
                float limit;
                float percentage;

                if (Utility.NormalizeAngle(shotAtributte.FiringArc) == 360)
                    limit = 180f * (shotAtributte.ProjectileQuantity - 1) / shotAtributte.ProjectileQuantity;
                else
                    limit = shotAtributte.FiringArc / 2;

                percentage = (float)i / (shotAtributte.ProjectileQuantity - 1);
                angleArc = Mathf.Lerp(limit, -limit, percentage);


                limit = shotAtributte.Spacing / 2;
                percentage = (float)i / (shotAtributte.ProjectileQuantity - 1);
                lateralOffset = Mathf.Lerp(limit, -limit, percentage);
            }

            float angleOffset = shotAtributte.AngularOffset + (shotAtributte.AngularOffsetSpeed * generalTimer);
            angleOffset = Utility.NormalizeAngle(angleOffset);

            if (firePointPos != gameObjectPos)
                firePointPos = Utility.RotatePointRelativeToPivot(firePointPos, gameObjectPos, angleOffset);

            Scr_BulletPool.Instance.spawnBullet(firePointPos, lateralOffset, bullet, angleArc + angleOffset + transform.eulerAngles.z, gameObject.tag, shotAtributte.Damage, shotAtributte.Type);
        }
    }
    #endregion
    #region Gizmos
    private void OnDrawGizmos()
    {
        if (shotAtributte != null && gizmosData != null)
            DrawGizmosFiringArea();
    }

    private void DrawGizmosFiringArea()
    {
        float angleOffset = shotAtributte.AngularOffset + (shotAtributte.AngularOffsetSpeed * generalTimer);
        angleOffset = Utility.NormalizeAngle(angleOffset);
        angleOffset += transform.eulerAngles.z;

        Vector3 origen = firePoint == null ? transform.position : firePoint.transform.position;

        if (origen != transform.position)
            origen = Utility.RotatePointRelativeToPivot(origen, transform.position, angleOffset);

        Vector3 puntoCentral = origen + Quaternion.Euler(0, 0, angleOffset) * Vector3.up * gizmosData.Radius;

        Gizmos.color = gizmosData.Color; // Puedes cambiar el color
        Handles.color = gizmosData.Color;

        Gizmos.DrawLine(origen, puntoCentral);

        // // Asegúrate de que el ángulo esté en el rango [0, 360]
        if (Utility.NormalizeAngle(shotAtributte.FiringArc) == 360)
        {
            Handles.DrawWireDisc(origen, Vector3.forward, gizmosData.Radius);
            return;
        }

        Vector3 puntoA = origen + Quaternion.Euler(0, 0, -shotAtributte.FiringArc / 2 + angleOffset) * Vector3.up * gizmosData.Radius;
        Vector3 puntoB = origen + Quaternion.Euler(0, 0, shotAtributte.FiringArc / 2 + angleOffset) * Vector3.up * gizmosData.Radius;

        Gizmos.DrawLine(origen, puntoA);
        Gizmos.DrawLine(origen, puntoB);
        Handles.DrawWireArc(origen, Vector3.forward, puntoA - origen, shotAtributte.FiringArc, gizmosData.Radius);
    }
    #endregion
}
