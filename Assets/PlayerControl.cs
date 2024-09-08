using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
  private float shipAcceleration = 10f;
  private float shipMaxVelocity = 10f;
  private float shipRotationSpeed = 180f;
  private float bulletSpeed = 8f;

  [SerializeField] private Transform bulletSpawn;
  [SerializeField] private Rigidbody2D bulletPrefab;

  private Rigidbody2D shipRigidbody;
  private bool isAlive = true;
  private bool isAccelerating = false;

  private void Start() {
    shipRigidbody = GetComponent<Rigidbody2D>();
  }

  private void Update() {
    if (isAlive) {
      ShipAcceleration();
      ShipRotation();
      Shooting();
    }
  }

  private void FixedUpdate() {
    if (isAlive && isAccelerating) {
      shipRigidbody.AddForce(shipAcceleration * transform.up);
      shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
    }
  }

  private void ShipAcceleration() {
    isAccelerating = Input.GetKey(KeyCode.UpArrow);
  }

  private void ShipRotation() {
    if (Input.GetKey(KeyCode.LeftArrow)) {
      transform.Rotate(shipRotationSpeed * Time.deltaTime * transform.forward);
    } else if (Input.GetKey(KeyCode.RightArrow)) {
      transform.Rotate(-shipRotationSpeed * Time.deltaTime * transform.forward);
    }
  }

  private void Shooting() {
    if (Input.GetKeyDown(KeyCode.Space)) {
      Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
      Vector2 shipVelocity = shipRigidbody.velocity;
      Vector2 shipDirection = transform.up;
      float shipForwardSpeed = Vector2.Dot(shipVelocity, shipDirection);
      if (shipForwardSpeed < 0) { 
        shipForwardSpeed = 0; 
      }
      bullet.velocity = shipDirection * shipForwardSpeed;
      bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
    }
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("Asteroid")) {
      isAlive = false;
      GameManager gameManager = FindAnyObjectByType<GameManager>();
      gameManager.GameOver();
      Destroy(collision.gameObject);
    }
  }
}