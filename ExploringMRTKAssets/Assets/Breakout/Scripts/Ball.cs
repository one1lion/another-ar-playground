using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ball : MonoBehaviour {
  float _speed = 1f;
  Rigidbody _rigidbody;
  Vector3 _velocity;

  private void Start() {
    _rigidbody = GetComponent<Rigidbody>();
      _rigidbody.velocity = Vector3.down * _speed;
  }

  private void FixedUpdate() {
    _rigidbody.velocity = _rigidbody.velocity.normalized * _speed;
    _velocity = _rigidbody.velocity;
  }

  private void Update() {

  }

  private void OnCollisionEnter(Collision collision) {
    _rigidbody.velocity = Vector3.Reflect(_velocity, collision.contacts.First().normal);
  }
}
