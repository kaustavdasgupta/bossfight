using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private CharacterController controller;
    private Animator playerAnim;
    private Vector3 playerVelocity;

    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 10f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0f)
        {
            playerVelocity.y = -2f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        move = Vector3.ClampMagnitude(move, 1f);
        PlayerAnimationController();

        if (move.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;


        Vector3 finalMove = move * playerSpeed;
        finalMove.y = playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime);
    }

    private void PlayerAnimationController()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            playerAnim.SetBool("isWalking", true);
        else
            playerAnim.SetBool("isWalking", false);
    }
}