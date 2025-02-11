using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        GameEventSystem.EmitEvent("Coin", "Collected");
        animator.SetTrigger("OnCollected");
    }

    private void OnDisappearEnd() {
        GameEventSystem.EmitEvent("Coin", "Destroy");
        Destroy(gameObject);
    }
}
