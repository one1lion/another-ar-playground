using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ball : MonoBehaviour {
  public GameObject Player { get; set; }

  float _speed = .8f;
  Rigidbody _rigidbody;
  Vector3 _velocity;

  private void Start() {
    _rigidbody = GetComponent<Rigidbody>();
    Invoke("Launch", 0.25f);
  }

  void Launch() {
    _rigidbody.velocity = Vector3.up * _speed;
  }

  private void FixedUpdate() {
    _rigidbody.velocity = _rigidbody.velocity.normalized * _speed;
    _velocity = _rigidbody.velocity;
    if(_rigidbody.transform.position.y <= Player.transform.position.y - .25) {
      GameManager.Instance.Balls--;
      Destroy(gameObject);
    }
  }

  private void Update() {

  }

  private void OnCollisionEnter(Collision collision) {
    _rigidbody.velocity = Vector3.Reflect(_velocity, collision.contacts.First().normal);
  }
}
