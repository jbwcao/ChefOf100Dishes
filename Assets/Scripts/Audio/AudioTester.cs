using UnityEngine;
using UnityEngine.InputSystem;

public class AudioTester : MonoBehaviour
{
    private void Update()
    {
        if (AudioManager.Instance == null) return;
        if (Keyboard.current == null) return;
        if (Keyboard.current.digit1Key.wasPressedThisFrame) AudioManager.Instance.PlayJump();
        if (Keyboard.current.digit2Key.wasPressedThisFrame) AudioManager.Instance.PlayAttack();
        if (Keyboard.current.digit3Key.wasPressedThisFrame) AudioManager.Instance.PlayEnemyDeath();
        if (Keyboard.current.digit4Key.wasPressedThisFrame) AudioManager.Instance.PlayTakeDamage();
        if (Keyboard.current.digit5Key.wasPressedThisFrame) AudioManager.Instance.PlayTomatoSpit();
        if (Keyboard.current.digit6Key.wasPressedThisFrame) AudioManager.Instance.PlayItemPickup();
        if (Keyboard.current.digit7Key.wasPressedThisFrame) AudioManager.Instance.PlayEnterPortal();
        if (Keyboard.current.digit8Key.wasPressedThisFrame) AudioManager.Instance.PlayFoodCreation();
        if (Keyboard.current.digit9Key.wasPressedThisFrame) AudioManager.Instance.PlayDropFoodIntoPot();
        if (Keyboard.current.digit0Key.wasPressedThisFrame) AudioManager.Instance.PlayGiveFoodToCustomer();
        if (Keyboard.current.fKey.wasPressedThisFrame) AudioManager.Instance.StartFootsteps();
        if (Keyboard.current.fKey.wasReleasedThisFrame) AudioManager.Instance.StopFootsteps();
    }
}
