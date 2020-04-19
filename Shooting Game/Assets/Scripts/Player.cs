using UnityEngine;

public class Player : MonoBehaviour {
    public int life = 3;
    public int score = 0;

    public int power = 1;
    public int maxPower = 3;
    public int boom = 0;
    public int maxBoom = 3;
    public float speed = 3f;
    public float curShotDelay = 0f;
    public float maxShotDelay = 0.15f;

    [HideInInspector]
    public bool isHit = false;
    [HideInInspector]
    public bool isBoomTime = false;

    public GameManager gameManager;
    public ObjectManager objectManager;

    public GameObject boomEffect;
    public GameObject[] followers;

    public Joystick joystick;

    public bool isRespawnTime;

    private bool isTouchTop;
    private bool isTouchBottom;
    private bool isTouchRight;
    private bool isTouchLeft;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        Unbeatable();
        Invoke("Unbeatable", 3f);
    }

    private void Update() {
        Move();
        Fire();
        Boom();
        Reload();
    }

    private void Unbeatable() {
        isRespawnTime = !isRespawnTime;

        if (isRespawnTime) {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            for (int i = 0; i < followers.Length; i++) {
                followers[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
        else {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            for (int i = 0; i < followers.Length; i++) {
                followers[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    private void Move() {
        float h = joystick.valueRaw.x;
        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1)) {
            h = 0;
        }
        float v = joystick.valueRaw.y;
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1)) {
            v = 0;
        }

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * joystick.GetDistance() * Time.deltaTime;

        transform.position = curPos + nextPos;

        anim.SetInteger("Input", (int)h);
    }

    private void Fire() {
        if (curShotDelay < maxShotDelay)
            return;

        switch (power) {
            case 1:
                GameObject bullet = objectManager.MakeObj(ePlayerBulletType.a, transform.position + Vector3.forward, transform.rotation);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletR = objectManager.MakeObj(ePlayerBulletType.a, transform.position + Vector3.forward + Vector3.right * 0.1f, transform.rotation);
                GameObject bulletL = objectManager.MakeObj(ePlayerBulletType.a, transform.position + Vector3.forward + Vector3.left * 0.1f, transform.rotation);
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 3:
            case 4:
            case 5:
            case 6:
                GameObject bulletRR = objectManager.MakeObj(ePlayerBulletType.a, transform.position + Vector3.forward + Vector3.right * 0.275f, transform.rotation);
                GameObject bulletCC = objectManager.MakeObj(ePlayerBulletType.b, transform.position + Vector3.forward, transform.rotation);
                GameObject bulletLL = objectManager.MakeObj(ePlayerBulletType.a, transform.position + Vector3.forward + Vector3.left * 0.275f, transform.rotation);
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

        }

        curShotDelay = 0.0f;
    }

    private void Reload() {
        curShotDelay += Time.deltaTime;
    }

    private void Boom() {
        if (!Input.GetButton("Fire2"))
            return;

        if (isBoomTime)
            return;

        if (boom <= 0)
            return;

        boom--;
        isBoomTime = true;

        gameManager.UpdateBoomIcon();

        boomEffect.SetActive(true);
        Invoke("returnBoomEffect", 1f);

        GameObject[] enemiesL = objectManager.GetPool(eEnemyType.large);
        GameObject[] enemiesM = objectManager.GetPool(eEnemyType.medium);
        GameObject[] enemiesS = objectManager.GetPool(eEnemyType.small);
        foreach (GameObject enemy in enemiesL) {
            if (enemy.activeSelf == true) {
                Enemy enemyLogic = enemy.gameObject.GetComponent<Enemy>();
                enemyLogic.OnHit(1000f);
            }
        }
        foreach (GameObject enemy in enemiesM) {
            if (enemy.activeSelf == true) {
                Enemy enemyLogic = enemy.gameObject.GetComponent<Enemy>();
                enemyLogic.OnHit(1000f);
            }
        }
        foreach (GameObject enemy in enemiesS) {
            if (enemy.activeSelf == true) {
                Enemy enemyLogic = enemy.gameObject.GetComponent<Enemy>();
                enemyLogic.OnHit(1000f);
            }
        }

        GameObject[] bulletsA = objectManager.GetPool(eEnemyBulletType.a);
        GameObject[] bulletsB = objectManager.GetPool(eEnemyBulletType.b);
        foreach (GameObject bullet in bulletsA) {
            if (bullet.activeSelf == true) {
                bullet.SetActive(false);
            }
        }
        foreach (GameObject bullet in bulletsB) {
            if (bullet.activeSelf == true) {
                bullet.SetActive(false);
            }
        }
    }

    private void returnBoomEffect() {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Border") {
            switch (collision.gameObject.name) {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        else if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet") {
            if (isRespawnTime) {
                return;
            }

            if (isHit)
                return;

            isHit = true;
            life--;
            gameManager.UpdateLifeIcon();
            gameManager.CallExplosion(transform.position, "Player");

            if (life <= 0) {
                gameManager.GameOver();
            }
            else {
                gameManager.RespawnPlayer();
                gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.tag == "Item") {
            Item item = collision.gameObject.GetComponent<Item>();

            switch (item.itemType) {
                case eItemType.coin:
                    score += 1000;

                    break;
                case eItemType.power:
                    if (power >= maxPower) {
                        score += 500;
                    }
                    else {
                        power++;
                        AddFollower();
                    }

                    break;
                case eItemType.boom:
                    if (boom >= maxBoom) {
                        score += 500;
                    }
                    else {
                        boom++;
                        gameManager.UpdateBoomIcon();
                    }

                    break;
            }

            collision.gameObject.SetActive(false);
        }
    }

    private void AddFollower() {
        if (power == 4) {
            followers[0].SetActive(true);
        }
        else if (power == 5) {
            followers[1].SetActive(true);
        }
        else if (power == 6) {
            followers[2].SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Border") {
            switch (collision.gameObject.name) {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}