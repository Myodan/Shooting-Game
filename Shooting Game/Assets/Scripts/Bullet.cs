using UnityEngine;

public enum eBulletType {
    player,
    enemy
}

public enum ePlayerBulletType {
    a,
    b
}

public enum eEnemyBulletType {
    a,
    b
}

public enum eFollowerBulletType {
    a
}

public class Bullet : MonoBehaviour {

    public eBulletType bulletType = eBulletType.player;

    public float dmg = 1f;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "BorderDestroy") {
            gameObject.SetActive(false);
            return;
        }

        switch (bulletType) {
            case eBulletType.player:
                if (collision.tag == "Enemy") {
                    gameObject.SetActive(false);
                }
                break;
            case eBulletType.enemy:
                if (collision.tag == "Player") {
                    gameObject.SetActive(false);
                }
                break;
        }
    }
}