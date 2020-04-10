using UnityEngine;

public class ObjectManager : MonoBehaviour {
    [Space]
    public GameObject enemyLPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemySPrefab;
    [Space]
    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;
    [Space]
    public GameObject playerBulletAPrefab;
    public GameObject playerBulletBPrefab;
    public GameObject enemyBulletAPrefab;
    public GameObject enemyBulletBPrefab;
    public GameObject followerBulletPrefab;

    private GameObject[] enemyL;
    private GameObject[] enemyM;
    private GameObject[] enemyS;

    private GameObject[] itemCoin;
    private GameObject[] itemPower;
    private GameObject[] itemBoom;

    private GameObject[] playerBulletA;
    private GameObject[] playerBulletB;
    private GameObject[] enemyBulletA;
    private GameObject[] enemyBulletB;
    private GameObject[] followerBullet;

    private void Awake() {
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[10];

        itemCoin = new GameObject[20];
        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];

        playerBulletA = new GameObject[100];
        playerBulletB = new GameObject[100];
        enemyBulletA = new GameObject[100];
        enemyBulletB = new GameObject[100];
        followerBullet = new GameObject[100];

        Generate();
    }

    private void Generate() {
        /* #1. Enemy */
        for (int i = 0; i < enemyL.Length; i++) {
            enemyL[i] = Instantiate(enemyLPrefab);
            enemyL[i].SetActive(false);
        }

        for (int i = 0; i < enemyM.Length; i++) {
            enemyM[i] = Instantiate(enemyMPrefab);
            enemyM[i].SetActive(false);
        }

        for (int i = 0; i < enemyS.Length; i++) {
            enemyS[i] = Instantiate(enemySPrefab);
            enemyS[i].SetActive(false);
        }

        /* #2. Item */
        for (int i = 0; i < itemCoin.Length; i++) {
            itemCoin[i] = Instantiate(itemCoinPrefab);
            itemCoin[i].SetActive(false);
        }

        for (int i = 0; i < itemPower.Length; i++) {
            itemPower[i] = Instantiate(itemPowerPrefab);
            itemPower[i].SetActive(false);
        }

        for (int i = 0; i < itemBoom.Length; i++) {
            itemBoom[i] = Instantiate(itemBoomPrefab);
            itemBoom[i].SetActive(false);
        }

        /* #3. Bullet */
        for (int i = 0; i < playerBulletA.Length; i++) {
            playerBulletA[i] = Instantiate(playerBulletAPrefab);
            playerBulletA[i].SetActive(false);
        }

        for (int i = 0; i < playerBulletB.Length; i++) {
            playerBulletB[i] = Instantiate(playerBulletBPrefab);
            playerBulletB[i].SetActive(false);
        }

        for (int i = 0; i < enemyBulletA.Length; i++) {
            enemyBulletA[i] = Instantiate(enemyBulletAPrefab);
            enemyBulletA[i].SetActive(false);
        }

        for (int i = 0; i < enemyBulletB.Length; i++) {
            enemyBulletB[i] = Instantiate(enemyBulletBPrefab);
            enemyBulletB[i].SetActive(false);
        }

        for (int i = 0; i < followerBullet.Length; i++) {
            followerBullet[i] = Instantiate(followerBulletPrefab);
            followerBullet[i].SetActive(false);
        }
    }

    public GameObject MakeObj(eEnemyType type, Vector3 pos, Quaternion rot) {
        GameObject[] targetPool = null;

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
        }

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

    public GameObject MakeObj(eItemType type, Vector3 pos, Quaternion rot) {
        GameObject[] targetPool = null;

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

    public GameObject MakeObj(ePlayerBulletType type, Vector3 pos, Quaternion rot) {
        GameObject[] targetPool = null;

        switch (type) {
            case ePlayerBulletType.a:
                targetPool = playerBulletA;
                break;
            case ePlayerBulletType.b:
                targetPool = playerBulletB;
                break;
        }

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

    public GameObject MakeObj(eEnemyBulletType type, Vector3 pos, Quaternion rot) {
        GameObject[] targetPool = null;

        switch (type) {
            case eEnemyBulletType.a:
                targetPool = enemyBulletA;
                break;
            case eEnemyBulletType.b:
                targetPool = enemyBulletB;
                break;
        }

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

    public GameObject MakeObj(eFollowerBulletType type, Vector3 pos, Quaternion rot) {
        GameObject[] targetPool = null;

        switch (type) {
            case eFollowerBulletType.a:
                targetPool = followerBullet;
                break;
        }

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

    public GameObject[] GetPool(eEnemyType type) {
        GameObject[] targetPool = null;

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
        }

        return targetPool;
    }

    public GameObject[] GetPool(eItemType type) {
        GameObject[] targetPool = null;

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

        return targetPool;
    }

    public GameObject[] GetPool(ePlayerBulletType type) {
        GameObject[] targetPool = null;

        switch (type) {
            case ePlayerBulletType.a:
                targetPool = playerBulletA;
                break;
            case ePlayerBulletType.b:
                targetPool = playerBulletB;
                break;
        }

        return targetPool;
    }

    public GameObject[] GetPool(eEnemyBulletType type) {
        GameObject[] targetPool = null;

        switch (type) {
            case eEnemyBulletType.a:
                targetPool = enemyBulletA;
                break;
            case eEnemyBulletType.b:
                targetPool = enemyBulletB;
                break;
        }

        return targetPool;
    }

    public GameObject[] GetPool(eFollowerBulletType type) {
        GameObject[] targetPool = null;

        switch (type) {
            case eFollowerBulletType.a:
                targetPool = followerBullet;
                break;
        }

        return targetPool;
    }
}