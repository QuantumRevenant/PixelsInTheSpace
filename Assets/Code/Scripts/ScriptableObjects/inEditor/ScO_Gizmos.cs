using UnityEngine;

[CreateAssetMenu(fileName = "New Gizmos Data", menuName = "Scriptable Objects/Utility/ScO_Gizmos", order = 0)]
public class ScO_Gizmos : ScriptableObject {
    [SerializeField] private float gizmosRadius=1;
    [SerializeField] private Color gizmosColor=Color.green;
    [SerializeField] private Color secondGizmosColor=Color.red;
    [SerializeField] private Color thirdGizmosColor=Color.yellow;
    [SerializeField] private float gizmosFixedReductor=100f;

    public float Radius { get => gizmosRadius/gizmosFixedReductor; set => gizmosRadius = value; }
    public Color Color { get => gizmosColor; set => gizmosColor = value; }
    public float FixedReductor { get => gizmosFixedReductor; set => gizmosFixedReductor = value; }
    public Color SecondColor { get => secondGizmosColor; set => secondGizmosColor = value; }
    public Color ThirdColor { get => thirdGizmosColor; set => thirdGizmosColor = value; }
}