using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private float bulletLifetime = 1f;
  private void Awake() {
    Destroy(gameObject, bulletLifetime);
  }
}