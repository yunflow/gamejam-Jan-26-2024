using UnityEngine;

[CreateAssetMenu(fileName = "new Action Set", menuName = "Action Set", order = 0)]
public class ActionSetSO : ScriptableObject
{
    public Vector2[] actions;
}

public enum Action
{
    Idle, // 0
    Speak, // 1
    Up, // 2
    Drink, // 3
    Down, // 4
    Angry // 5
}