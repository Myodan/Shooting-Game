using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed = 1f;

    private float viewHeight;

    private int startIndex;
    private int endIndex = 1;

    private Transform[] sprites;

    private void Awake() {
        viewHeight = Camera.main.orthographicSize * 2;
        sprites = GetComponentsInChildren<Transform>();
        startIndex = sprites.Length - 1;
    }

    private void Update() {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;


        if (sprites[endIndex].position.y < viewHeight * (-1)) {
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            Vector3 frontSprite = sprites[endIndex].localPosition;

            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.up * viewHeight;

            int temp = startIndex;
            startIndex = endIndex;
            endIndex = (temp - 1  == 0) ? sprites.Length - 1 : temp - 1;
        }
    }
}
