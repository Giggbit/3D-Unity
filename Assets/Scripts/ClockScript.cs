using UnityEngine;

public class ClockScript : MonoBehaviour
{
    private TMPro.TextMeshProUGUI clock;

    void Start() {
        clock = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update() {
        int hour = Mathf.FloorToInt(GameState.gameTimeHour);
        int minute = Mathf.FloorToInt(60 * (GameState.gameTimeHour - hour));
        clock.text = $"{hour:D2}:{minute:D2}";
    }
}
