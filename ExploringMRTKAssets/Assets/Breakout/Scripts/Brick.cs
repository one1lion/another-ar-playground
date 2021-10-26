using System;
using UnityEngine;

public class Brick : MonoBehaviour {
  public int Hits = 1;
  public int Points = 100;
  public Vector3 Rotator;
  public Material HitMaterial;

  private Renderer _renderer;
  private Material _originalMateral;
  private void Start() {
    transform.Rotate(Rotator * (transform.position.x + transform.position.y) * 2.0f);
    _renderer = GetComponent<Renderer>();
    _originalMateral = _renderer.sharedMaterial;
  }

  private void Update() {
    transform.Rotate(Rotator * Time.deltaTime);
  }

  private void OnCollisionEnter(Collision collision) {
    GameManager.Instance.Score += (int)Math.Floor(Points * .25f);
    if (--Hits <= 0) {
    GameManager.Instance.Score += Points;
      Destroy(gameObject);
    }
    _renderer.sharedMaterial = HitMaterial;
    Invoke("RestoreMaterial", 0.05f);
  }

  private void RestoreMaterial() {
    _renderer.sharedMaterial = _originalMateral;
  }

}
