using UnityEngine;
[CreateAssetMenu(fileName = "New Bullet Data", menuName = "Scriptable Objects/Bullet Data")]
public class BulletData : ScriptableObject
{
    [SerializeField] private Sprite bulletSprite;
    [SerializeField] private Color bulletColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private float baseSpeed = 1f;
    [SerializeField] private float baseDesviation = 0;
    [SerializeField] private Vector2 baseVector = new Vector2(0, 1f);
    [SerializeField] private bool useAngle = false;
    [SerializeField] private float baseAngle = 0;
    [SerializeField] private float baseScale = 1f;
    [SerializeField] private Vector2 boundaries = new Vector2(1.25f, 1);
    [SerializeField] private float boundariesOffset = 0;
    // [SerializeField] private float spriteSize;
    public Sprite BulletSprite { get { return bulletSprite; } }
    public Color BulletColor { get { return bulletColor; } }
    public float BaseSpeed { get { return baseSpeed; } }
    public float BaseDesviation { get { return baseDesviation; } }
    public Vector2 BaseVector { get { return baseVector; } }
    public bool UseAngle { get { return useAngle; } }
    public float BaseAngle { get { return baseAngle; } }
    public Vector3 BaseScale { get { return new Vector3(baseScale, baseScale, baseScale); } }
    public Vector2 Boundaries { get { return boundaries; } }
    public float BoundariesOffset { get { return boundariesOffset; } }


}
