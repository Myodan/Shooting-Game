using UnityEngine;

public enum eItemType {
    coin,
    power,
    boom
}

public class Item : MonoBehaviour {
    public eItemType itemType = eItemType.coin;

    Rigidbody2D rigid;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        rigid.velocity = Vector2.down * 1.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "BorderDestroy") {
            gameObject.SetActive(false);
        }
    }
}
