using UnityEngine;

public enum eEnemyType {
    large,
    medium,
    small,
    boss
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

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    private SpriteRenderer spriteRenderer;
    private Animator anim;

    [HideInInspector] public GameObject player;
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public ObjectManager objectManager;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (enemyType == eEnemyType.boss) {
            anim = GetComponent<Animator>();
        }
    }

    private void OnEnable() {
        health = startHealth;
        if (enemyType == eEnemyType.boss) {
            Invoke("Stop", 2f);
        }
        else {
            ReturnSprite();
        }
    }

    private void Update() {
        if (enemyType == eEnemyType.boss)
            return;

        Fire();
        Reload();
    }

    private void Stop() {
        if (gameObject.activeSelf == false)
            return;

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2f);
    }

    private void Think() {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch (patternIndex) {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    private void FireFoward() {
        GameObject bulletR = objectManager.MakeObj(eEnemyBulletType.d, transform.position + Vector3.forward * 0.3f + Vector3.right * 0.3f, transform.rotation);
        GameObject bulletRR = objectManager.MakeObj(eEnemyBulletType.d, transform.position + Vector3.forward * 0.3f + Vector3.right * 0.45f, transform.rotation);
        GameObject bulletL = objectManager.MakeObj(eEnemyBulletType.d, transform.position + Vector3.forward * 0.3f + Vector3.left * 0.3f, transform.rotation);
        GameObject bulletLL = objectManager.MakeObj(eEnemyBulletType.d, transform.position + Vector3.forward * 0.3f + Vector3.left * 0.45f, transform.rotation);

        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

        rigidR.AddForce(Vector2.down * 4, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 4, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 4, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 4, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) {
            Invoke("FireFoward", 2f);
        }
        else {
            Invoke("Think", 3f);
        }
    }

    private void FireShot() {
        if (health <= 0) return;

        for (int i = 0; i < 5; i++) {
            GameObject bullet = objectManager.MakeObj(eEnemyBulletType.b, transform.position + Vector3.forward, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 3f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 4, ForceMode2D.Impulse);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) {
            Invoke("FireShot", 3.5f);
        }
        else {
            Invoke("Think", 3f);
        }
    }

    private void FireArc() {
        if (health <= 0) return;

        GameObject bullet = objectManager.MakeObj(eEnemyBulletType.a, transform.position, Quaternion.identity);

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 10 * (float)curPatternCount / maxPatternCount[patternIndex]), -1);
        rigid.AddForce(dirVec.normalized * 4, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) {
            Invoke("FireArc", 0.15f);
        }
        else {
            Invoke("Think", 3f);
        }
    }

    private void FireAround() {
        if (health <= 0) return;

        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        for (int i = 0; i < roundNum; i++) {
            GameObject bullet = objectManager.MakeObj(eEnemyBulletType.c, transform.position, Quaternion.identity);

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));
            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * i / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) {
            Invoke("FireAround", 0.7f);
        }
        else {
            Invoke("Think", 3f);
        }
    }

    private void Fire() {
        if (curShotDelay < maxShotDelay)
            return;

        switch (enemyType) {
            case eEnemyType.small:
                GameObject bullet = objectManager.MakeObj(eEnemyBulletType.a, transform.position + Vector3.forward, transform.rotation);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dirVec = player.transform.position - transform.position;
                rigid.AddForce(dirVec.normalized * 4, ForceMode2D.Impulse);
                break;
            case eEnemyType.large:
                GameObject bulletR = objectManager.MakeObj(eEnemyBulletType.b, transform.position + Vector3.forward * 0.3f + Vector3.right * 0.3f, transform.rotation);
                GameObject bulletL = objectManager.MakeObj(eEnemyBulletType.b, transform.position + Vector3.forward * 0.3f + Vector3.left * 0.3f, transform.rotation);
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                Vector2 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
                Vector2 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);
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

        if (enemyType == eEnemyType.boss) {
            anim.SetTrigger("OnHit");
        }
        else {
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }

        if (health <= 0) {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += score;
            int ran;

            if (enemyType == eEnemyType.boss) {
                ran = 0;
            }
            else {
                ran = Random.Range(0, 10);
            }

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

            CancelInvoke();
            gameObject.SetActive(false);
            gameManager.CallExplosion(transform.position, enemyType);
        }
    }

    private void ReturnSprite() {
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "BorderDestroy" && enemyType != eEnemyType.boss) {
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "PlayerBullet") {
            Bullet bullet = collision.GetComponent<Bullet>();
            OnHit(bullet.dmg);
        }
    }
}