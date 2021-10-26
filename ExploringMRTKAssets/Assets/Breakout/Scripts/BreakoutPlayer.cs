using UnityEngine;

public class BreakoutPlayer : MonoBehaviour {
  Rigidbody _rigidbody;
  private void Start() {
    _rigidbody = GetComponent<Rigidbody>();
  }

  private void FixedUpdate() {
  }
}
