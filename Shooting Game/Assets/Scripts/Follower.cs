using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {
    public float curShotDelay = 0f;
    public float maxShotDelay = 0.15f;

    public ObjectManager objectManager;

    public Vector3 followPos;
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;

    private void Awake() {
        parentPos = new Queue<Vector3>();
    }

    private void Update() {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    private void Watch() {
        if (parentPos.Contains(parent.position) == false) {
            parentPos.Enqueue(parent.position);
        }

        if (parentPos.Count > followDelay) {
            followPos = parentPos.Dequeue();
        }
        else if (parentPos.Count < followDelay) {
            followPos = parent.position;
        }
    }

    private void Follow() {
        transform.position = followPos;
    }

    private void Fire() {
        if (curShotDelay < maxShotDelay)
            return;

        GameObject bullet = objectManager.MakeObj(eFollowerBulletType.a, transform.position + Vector3.forward, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        curShotDelay = 0.0f;
    }

    private void Reload() {
        curShotDelay += Time.deltaTime;
    }

}
