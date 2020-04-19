using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void OnEnable() {
        Invoke("Disable", 2f);
    }

    private void Disable() {
        gameObject.SetActive(false);
    }

    public void StartExplosion(string target) {
        switch (target) {
            case "Player":
                transform.localScale = Vector3.one * 1f;
                anim.SetTrigger("OnExplosion");
                break;
        }
    }

    public void StartExplosion(eEnemyType target) {
        switch (target) {
            case eEnemyType.small:
                transform.localScale = Vector3.one * 0.7f;
                break;
            case eEnemyType.medium:
                transform.localScale = Vector3.one * 1f;
                break;
            case eEnemyType.large:
                transform.localScale = Vector3.one * 2f;
                break;
            case eEnemyType.boss:
                transform.localScale = Vector3.one * 3f;
                break;
        }

        anim.SetTrigger("OnExplosion");
    }
}
