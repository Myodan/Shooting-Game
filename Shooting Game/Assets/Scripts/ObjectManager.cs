using UnityEngine;

public class ObjectManager : MonoBehaviour {
    [Header("- Enemy")]
    public GameObject enemyLPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemySPrefab;
    public GameObject enemyBPrefab;

    [Header("- Item")]
    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;

    [Header("- Bullet")]
    public GameObject playerBulletAPrefab;
    public GameObject playerBulletBPrefab;
    public GameObject followerBulletPrefab;
    public GameObject enemyBulletAPrefab;
    public GameObject enemyBulletBPrefab;
    public GameObject enemyBulletCPrefab;
    public GameObject enemyBulletDPrefab;

    [Header("- Other")]
    public GameObject explosionPrefab;

    private GameObject[] enemyL;
    private GameObject[] enemyM;
    private GameObject[] enemyS;
    private GameObject[] enemyB;

    private GameObject[] itemCoin;
    private GameObject[] itemPower;
    private GameObject[] itemBoom;

    private GameObject[] playerBulletA;
    private GameObject[] playerBulletB;
    private GameObject[] followerBullet;
    private GameObject[] enemyBulletA;
    private GameObject[] enemyBulletB;
    private GameObject[] enemyBulletC;
    private GameObject[] enemyBulletD;

    private GameObject[] explosion;

    private GameObject[] targetPool;

    private void Awake() {
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[10];
        enemyB = new GameObject[1];

        itemCoin = new GameObject[20];
        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];

        playerBulletA = new GameObject[100];
        playerBulletB = new GameObject[100];
        enemyBulletA = new GameObject[100];
        enemyBulletB = new GameObject[100];
        followerBullet = new GameObject[100];
        enemyBulletC = new GameObject[1000];
        enemyBulletD = new GameObject[50];

        explosion = new GameObject[20];

        Generate();
    }

    private void GenerateExe(GameObject[] obj, GameObject prefab) {
        for (int i = 0; i < obj.Length; i++) {
            obj[i] = Instantiate(prefab);
            obj[i].SetActive(false);
        }
    }

    private void Generate() {
        /* #1. Enemy */
        GenerateExe(enemyL, enemyLPrefab);
        GenerateExe(enemyM, enemyMPrefab);
        GenerateExe(enemyS, enemySPrefab);
        GenerateExe(enemyB, enemyBPrefab);

        /* #2. Item */
        GenerateExe(itemCoin, itemCoinPrefab);
        GenerateExe(itemPower, itemPowerPrefab);
        GenerateExe(itemBoom, itemBoomPrefab);

        /* #3. Bullet */
        GenerateExe(playerBulletA, playerBulletAPrefab);
        GenerateExe(playerBulletB, playerBulletBPrefab);
        GenerateExe(followerBullet, followerBulletPrefab);
        GenerateExe(enemyBulletA, enemyBulletAPrefab);
        GenerateExe(enemyBulletB, enemyBulletBPrefab);
        GenerateExe(enemyBulletC, enemyBulletCPrefab);
        GenerateExe(enemyBulletD, enemyBulletDPrefab);

        /* Other */
        GenerateExe(explosion, explosionPrefab);
    }

    private void SelectTarget(eEnemyType type) {
        switch (type) {
            case eEnemyType.large:
                targetPool = enemyL;
                break;
            case eEnemyType.medium:
                targetPool = enemyM;
                break;
            case eEnemyType.small:
                targetPool = enemyS;
                break;
            case eEnemyType.boss:
                targetPool = enemyB;
                break;
        }
    }
    private void SelectTarget(eItemType type) {
        switch (type) {
            case eItemType.coin:
                targetPool = itemCoin;
                break;
            case eItemType.boom:
                targetPool = itemBoom;
                break;
            case eItemType.power:
                targetPool = itemPower;
                break;
        }
    }
    private void SelectTarget(ePlayerBulletType type) {
        switch (type) {
            case ePlayerBulletType.a:
                targetPool = playerBulletA;
                break;
            case ePlayerBulletType.b:
                targetPool = playerBulletB;
                break;
        }
    }
    private void SelectTarget(eEnemyBulletType type) {
        switch (type) {
            case eEnemyBulletType.a:
                targetPool = enemyBulletA;
                break;
            case eEnemyBulletType.b:
                targetPool = enemyBulletB;
                break;
            case eEnemyBulletType.c:
                targetPool = enemyBulletC;
                break;
            case eEnemyBulletType.d:
                targetPool = enemyBulletD;
                break;
        }
    }
    private void SelectTarget(eFollowerBulletType type) {
        switch (type) {
            case eFollowerBulletType.a:
                targetPool = followerBullet;
                break;
        }
    }
    private void SelectTarget(string type) {
        switch (type) {
            case "Explosion":
                targetPool = explosion;
                break;
        }
    }

    private GameObject MakeObjExe(Vector3 pos, Quaternion rot) {
        for (int i = 0; i < targetPool.Length; i++) {
            if (targetPool[i].activeSelf == false) {
                targetPool[i].SetActive(true);
                targetPool[i].transform.position = pos;
                targetPool[i].transform.rotation = rot;
                return targetPool[i];
            }
        }

        return null;
    }

    public GameObject MakeObj(eEnemyType type, Vector3 pos, Quaternion rot) {
        SelectTarget(type);
        return MakeObjExe(pos, rot);
    }
    public GameObject MakeObj(eItemType type, Vector3 pos, Quaternion rot) {
        SelectTarget(type);
        return MakeObjExe(pos, rot);
    }
    public GameObject MakeObj(ePlayerBulletType type, Vector3 pos, Quaternion rot) {
        SelectTarget(type);
        return MakeObjExe(pos, rot);
    }
    public GameObject MakeObj(eEnemyBulletType type, Vector3 pos, Quaternion rot) {
        SelectTarget(type);
        return MakeObjExe(pos, rot);
    }
    public GameObject MakeObj(eFollowerBulletType type, Vector3 pos, Quaternion rot) {
        SelectTarget(type);
        return MakeObjExe(pos, rot);
    }
    public GameObject MakeObj(string type, Vector3 pos, Quaternion rot) {
        SelectTarget(type);
        return MakeObjExe(pos, rot);
    }

    public GameObject[] GetPool(eEnemyType type) {
        SelectTarget(type);
        return targetPool;
    }
    public GameObject[] GetPool(eItemType type) {
        SelectTarget(type);
        return targetPool;
    }
    public GameObject[] GetPool(ePlayerBulletType type) {
        SelectTarget(type);
        return targetPool;
    }
    public GameObject[] GetPool(eEnemyBulletType type) {
        SelectTarget(type);
        return targetPool;
    }
    public GameObject[] GetPool(eFollowerBulletType type) {
        SelectTarget(type);
        return targetPool;
    }
    public GameObject[] GetPool(string type) {
        SelectTarget(type);
        return targetPool;
    }
}