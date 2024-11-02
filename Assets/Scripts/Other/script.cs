using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f; // Tinggi lompatan
    public Transform cameraTransform;

    private CharacterController characterController;
    private Vector3 moveDirection;
    private float verticalVelocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // Mengambil input dari tombol WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Membuat arah gerakan berdasarkan kamera
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Menghitung sudut untuk rotasi karakter
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);

            // Rotasi karakter
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            // Menggerakkan karakter maju sesuai arah rotasi
            moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        // Menambahkan gravitasi dan lompatan ke kecepatan vertikal
        if (characterController.isGrounded)
        {
            verticalVelocity = 0f; // Mengatur ke 0 ketika menyentuh tanah
            
            // Mengecek jika tombol lompat ditekan
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity); // Menghitung kecepatan untuk lompatan
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // Menambah kecepatan gravitasi
        }

        moveDirection.y = verticalVelocity;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
