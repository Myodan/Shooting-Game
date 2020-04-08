using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
    public GameObject player;

    public ObjectManager objectManager;

    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverUIObj;
    public Transform[] spawnPoints;

    public float curSpawnDelay = 0.0f;
    public float nextSpawnDelay = 0.0f;

    private Player playerLogic;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    private void Awake() {
        spawnList = new List<Spawn>();
        playerLogic = player.GetComponent<Player>();
        ReadSpawnFile();
        UpdateBoomIcon();
    }

    private void ReadSpawnFile() {
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        TextAsset textFile = Resources.Load("Stage 0") as TextAsset;

        if (textFile == null)
            return;

        StringReader stringRender = new StringReader(textFile.text);

        while (stringRender != null) {
            string line = stringRender.ReadLine();

            if (line == null)
                break;

            Spawn spawnData = new Spawn();
            string[] spanwDataSplit = line.Split(',');
            spawnData.delay = float.Parse(spanwDataSplit[0]);
            spawnData.type = (eEnemyType)System.Enum.Parse(typeof(eEnemyType), spanwDataSplit[1]);
            spawnData.point = int.Parse(spanwDataSplit[2]);
            spawnList.Add(spawnData);
        }

        stringRender.Close();
        nextSpawnDelay = spawnList[0].delay;
    }

    private void Update() {
        curSpawnDelay += Time.deltaTime;

        if ((curSpawnDelay > nextSpawnDelay) && (spawnEnd == false)) {
            SpawnEnemy();
            curSpawnDelay = 0f;
        }

        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    private void SpawnEnemy() {
        GameObject enemy = objectManager.MakeObj(
            spawnList[spawnIndex].type, 
            spawnPoints[spawnList[spawnIndex].point].position, 
            spawnPoints[spawnList[spawnIndex].point].rotation);

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

        switch (spawnPoints[spawnList[spawnIndex].point].parent.name) {
            case "Center":
                rigid.velocity = new Vector2(0, -enemyLogic.speed);
                break;
            case "Right":
                enemy.transform.Rotate(Vector3.back * 90);
                rigid.velocity = new Vector2(-enemyLogic.speed, -1);
                break;
            case "Left":
                enemy.transform.Rotate(Vector3.forward * 90);
                rigid.velocity = new Vector2(enemyLogic.speed, -1);
                break;
        }

        spawnIndex++;

        if (spawnIndex == spawnList.Count) {
            spawnEnd = true;
            return;
        }

        nextSpawnDelay = spawnList[spawnIndex].delay;
    }

    public void UpdateLifeIcon() {
        for (int i = 0; i < lifeImage.Length; i++) {
            lifeImage[i].color = new Color(255 / 255, 255 / 255, 255 / 255, 0);
        }
        for (int i = 0; i < playerLogic.life; i++) {
            lifeImage[i].color = new Color(255 / 255, 255 / 255, 255 / 255, 1);
        }
    }

    public void UpdateBoomIcon() {
        for (int i = 0; i < boomImage.Length; i++) {
            boomImage[i].color = new Color(255 / 255, 255 / 255, 255 / 255, 0);
        }
        for (int i = 0; i < playerLogic.boom; i++) {
            boomImage[i].color = new Color(255 / 255, 255 / 255, 255 / 255, 1);
        }
    }

    public void RespawnPlayer() {
        Invoke("RespawnPlayerExe", 2f);
    }

    private void RespawnPlayerExe() {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);
        playerLogic.isHit = false;
    }

    public void GameOver() {
        Time.timeScale = 0f;
        gameOverUIObj.SetActive(true);
    }

    public void GameRetry() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}