using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
  // SerializeField attribute exposes the variable in the inspector
  // Use it when not exposing to other classes
  public Transform _groundCheckTransform;
  public LayerMask _playerMask;
  private bool _jumpKeyWasPressed;
  private float _horizontalInput;
  private Rigidbody _rigidBodyComponent;
  private int _superJumpsRemaining;

  private const float _jumpPower = 7.0f;
  private const float _superJumpAdd = 2.0f;

  // Start is called before the first frame update
  private void Start() {
    _rigidBodyComponent = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  private void Update() {
    if (Input.GetButton("Jump") && _rigidBodyComponent.velocity.y == 0) {
      _jumpKeyWasPressed = true;
    }

    _horizontalInput = Input.GetAxis("Horizontal");
  }

  // FixedUpdate is called once every physic update
  private void FixedUpdate() {
    _rigidBodyComponent.velocity = new Vector3(_horizontalInput, _rigidBodyComponent.velocity.y, 0);

    if (Physics.OverlapSphere(_groundCheckTransform.position, 0.1f, _playerMask).Length == 0) {
      return;
    }

    if (_jumpKeyWasPressed) {
      _jumpKeyWasPressed = false;
      var jumpPower = _jumpPower; 
      if(_superJumpsRemaining > 0) {
        jumpPower += _superJumpAdd;
        _superJumpsRemaining--; 
      }
      _rigidBodyComponent.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
    }
  }

  private void OnTriggerEnter(Collider other) {
    if (other.gameObject.layer == 6) {
      Destroy(other.gameObject);
      _superJumpsRemaining++;
    }
  }
}
