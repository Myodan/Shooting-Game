using UnityEngine;

public enum eEnemyType {
    large,
    medium,
    small
}

public class Enemy : MonoBehaviour {
    public eEnemyType enemyType = eEnemyType.small;

    public float speed = 1f;
    public float startHealth = 3f;
    public float health = 3f;
    public int score = 1;

    public Sprite[] sprites;

    public float curShotDelay = 0f;
    public float maxShotDelay = 0.15f;

    SpriteRenderer spriteRenderer;

    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public ObjectManager objectManager;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        health = startHealth;
    }

    private void Update() {
        Fire();
        Reload();
    }

    private void Fire() {
        if (curShotDelay < maxShotDelay)
            return;

        switch (enemyType) {
            case eEnemyType.small:
                GameObject bullet = objectManager.MakeObj(eEnemyBulletType.a, transform.position + Vector3.forward, transform.rotation);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector3 dirVec = player.transform.position - transform.position;
                rigid.AddForce(dirVec.normalized * 4, ForceMode2D.Impulse);
                break;
            case eEnemyType.large:
                GameObject bulletR = objectManager.MakeObj(eEnemyBulletType.b, transform.position + Vector3.forward * 0.3f + Vector3.right * 0.3f, transform.rotation);
                GameObject bulletL = objectManager.MakeObj(eEnemyBulletType.b, transform.position + Vector3.forward * 0.3f + Vector3.left * 0.3f, transform.rotation);
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
                Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);
                rigidR.AddForce(dirVecR.normalized * 3, ForceMode2D.Impulse);
                rigidL.AddForce(dirVecL.normalized * 3, ForceMode2D.Impulse);
                break;
        }

        curShotDelay = 0.0f;
    }

    private void Reload() {
        curShotDelay += Time.deltaTime;
    }


    public void OnHit(float dmg) {
        if (health <= 0)
            return;

        health -= dmg;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (health <= 0) {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += score;

            int ran = Random.Range(0, 10);

            if (ran < 5) {
                /* #. Not Item! */
            }
            else if (ran < 8) {
                objectManager.MakeObj(eItemType.coin, transform.position, Quaternion.identity);
            }
            else if (ran < 9) {
                objectManager.MakeObj(eItemType.power, transform.position, Quaternion.identity);
            }
            else if (ran < 10) {
                objectManager.MakeObj(eItemType.boom, transform.position, Quaternion.identity);
            }

            gameObject.SetActive(false);
        }
    }

    private void ReturnSprite() {
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "BorderDestroy") {
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "PlayerBullet") {
            Bullet bullet = collision.GetComponent<Bullet>();
            OnHit(bullet.dmg);
        }
    }
}