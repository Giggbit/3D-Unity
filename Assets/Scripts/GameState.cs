using UnityEngine;

public class GameState : MonoBehaviour
{
    public static bool isFpv { get; set; }

    public static float minFpvDistance { get; set; } = 2f;
    public static float maxFpvDistance { get; set; } = 4f;

    public static float lookSensitivityX { get; set; } = 10.0f;
    public static float lookSensitivityY { get; set; } = -6.0f;


}
