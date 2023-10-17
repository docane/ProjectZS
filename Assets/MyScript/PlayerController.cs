using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : LivingEntity
{
    private float walkSpeed = 1.0f;
    private float runSpeed = 3.0f;
    private float backwardSpeed = 1.0f;
    private float crouchSpeed = 1.0f;
    private float jumpForce = 6f;
    private float applySpeed;

    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    private float crouchPosY = 0.5f;
    private float originPosY;
    private float applyCrouchPosY = 0.5f;

    private Vector3 velocity;

    private CapsuleCollider col;
    private Rigidbody rb;

    float sensivity = 3.0f;
    private float currentCameraRotationX = 0;
    private float currentCameraRotationY = 0;

    public Projectile projectile;
    public Transform muzzle;
    float msBetweenShots = 200;
    float muzzleVelocity = 35;
    float nextShotTime;

    public GameObject mainCamera;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        dead = false;
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;

        originPosY = mainCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        IsGround();
        PlayerMove();
        TryJump();
        TryRun();
        TryCrouch();
        CameraRotation();
        CharacterRotation();

        if (Input.GetMouseButton(0))
        {
            FireGun();
        }
        Debug.Log(isGround);
    }

    void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        velocity = new Vector3(h, 0, v);
        velocity = transform.TransformDirection(velocity);
        if (v > 0.1)
        {
            velocity *= applySpeed;
        }
        else if (v < -0.1)
        {
            velocity *= backwardSpeed;
        }
        transform.localPosition += velocity * Time.fixedDeltaTime;
        // mainCamera.transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y, transform.position.z);
    }

    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()
    {

        float posY = mainCamera.transform.localPosition.y;
        int count = 0;

        while (posY != applyCrouchPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.3f);
            mainCamera.transform.localPosition = new Vector3(0, posY, 0);
            // if (count > 15)
            //    break;
            yield return null;
        }
        // mainCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.1f);
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (isCrouch)
            Crouch();
        rb.velocity = transform.up * jumpForce;
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            Running();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            RunningCancel();
    }

    private void Running()
    {
        if (isCrouch)
            Crouch();

        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    void FireGun()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            bool fire = Input.GetMouseButton(0);
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.SetSpeed(muzzleVelocity);
        }
    }

    private void CameraRotation()
    {
        float yRotateSize = Input.GetAxis("Mouse X");
        float yRotate = yRotateSize * sensivity;
        currentCameraRotationY += yRotate;

        float xRotateSize = Input.GetAxis("Mouse Y");
        float xRotate = -xRotateSize * sensivity;
        currentCameraRotationX += xRotate;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -45, 60);

        mainCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0f);

        transform.Rotate(new Vector3(0, yRotate, 0));

        float rad = Mathf.Deg2Rad * currentCameraRotationY;
        float x = 0.7f * Mathf.Sin(rad);
        float z = 0.7f * Mathf.Cos(rad);

        Vector3 position = new Vector3(x, 1.2f, z);
        // mainCamera.transform.localPosition = position;
    }

    private void CharacterRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, yRotation, 0f) * sensivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(_characterRotationY));
    }
}