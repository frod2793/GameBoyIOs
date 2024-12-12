using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization; 

namespace DogGuns_Games.vamsir
{
    public class BAM_PlayerControll : MonoBehaviour
    {
        [Header("<color=green>Player Object")] [SerializeField]
        private Player_Base player;

        private Animator playerAnimator;

        [Header("<color=green>Joystick")] [SerializeField]
        private VariableJoystick variableJoystick;

        [Header("<color=green>camera")] [SerializeField]
        private Transform cameraTransform;

        [Header("<color=green>Move Duration")] [SerializeField]
        private float moveDuration = 0.1f;


        bool isGameStart = false;

        private void Start()
        {
            playStart();
        }


        private void playStart()
        {
            player = FindFirstObjectByType<Player_Base>();
            playerAnimator = player.GetComponent<Animator>();
            if (player != null)
            {
                isGameStart = true;
            }
        }

        private void FixedUpdate()
        {
            if (isGameStart)
            {
                PlayerMovement();
                FallowCamera();
            }
        }

        private void PlayerMovement()
        {
            Vector3 moveVector = (Vector3.right * variableJoystick.Horizontal + Vector3.up * variableJoystick.Vertical);
            float deltaSpeed = player.moveSpeed * Time.deltaTime;
            player.transform.DOMove(player.transform.position + moveVector * deltaSpeed, moveDuration);
            playerAnimator.SetFloat("Walk", moveVector.magnitude);

            if (moveVector != Vector3.zero)
            {
                float angle = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;
                float yRotation = (angle > 90 || angle < -90) ? 180f : 0f;
                player.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            }
        }


        private void FallowCamera()
        {
            Vector3 cameraPosition = new Vector3(player.transform.position.x, player.transform.position.y,
                cameraTransform.position.z);
            cameraTransform.DOMove(cameraPosition, moveDuration);
        }
    }
}