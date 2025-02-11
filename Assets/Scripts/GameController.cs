using UnityEngine;

public class GameController : MonoBehaviour
{
    void Start() {
        GameEventSystem.AddListener(OnCoinEvent, "Coin");
    }

    private void OnCoinEvent(string type, object payload) {
        Debug.Log($"OnCoinEvent: {type} {payload}");
    }

    private void OnDestroy() {
        GameEventSystem.RemoveListener(OnCoinEvent, "Coin");
    }
}
