using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
    public RectTransform handle;

    public Vector2 valueRaw = Vector2.zero;
    public bool isTouch = false;

    private RectTransform rect;
    private float radius;

    private void Awake() {
        rect = GetComponent<RectTransform>();
        radius = rect.rect.width * 0.5f;
    }

    private void Update() {
        
    }

    public float GetDistance() {
        return Vector2.Distance(rect.position, handle.position) / radius;
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 value = eventData.position - new Vector2(rect.position.x, rect.position.y);
        value = Vector2.ClampMagnitude(value, radius);
        handle.anchoredPosition = value;
        valueRaw = new Vector2(value.normalized.x > 0 ? 1 : -1, value.normalized.y > 0 ? 1 : -1);
    }

    public void OnPointerDown(PointerEventData eventData) {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        isTouch = false;
        valueRaw = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}
