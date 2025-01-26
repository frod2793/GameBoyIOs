using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DogGuns_Games.vamsir
{
    public class VAM_PlayerControll : MonoBehaviour
    {
        [Header("<color=green>Player Object")] [SerializeField]
        private Player_Base player;

        [Header("<color=green>Player HP_IU")] [SerializeField]
        private Slider playerHpSliderPrefab;

        private Animator _playerAnimator;

        [Header("<color=green>Joystick")] [SerializeField]
        private VariableJoystick variableJoystick;

        [Header("<color=green>camera")] [SerializeField]
        private Transform cameraTransform;

        [Header("<color=green>Move Duration")] [SerializeField]
        private float moveDuration = 0.1f;


        bool _isGameStart = false;

        bool _isAttack = false;

        private void Start()
        {
            Play_State.OnGameStart += PlayerInit;
            Play_State.OnGamePause += Pause;
            Play_State.OnGameResume += Resume;
        }

        private void OnDestroy()
        {
            Play_State.OnGameStart -= PlayerInit;
            Play_State.OnGamePause -= Pause;
            Play_State.OnGameResume -= Resume;
        }

        private void PlayerInit()
        {
            if (player == null)
            {
                player = FindFirstObjectByType<Player_Base>();
                _playerAnimator = player.GetComponent<Animator>();
                cameraTransform = Camera.main.transform;
                Set_playerHpSlider();
            }
            _isGameStart = true;
        }

        private void Set_playerHpSlider()
        {
            Slider playerHpSlider = Instantiate(playerHpSliderPrefab, player.transform);
            playerHpSlider.transform.localPosition = new Vector3(0, -0.4f, 0);
            playerHpSlider.maxValue = player.Health;
            playerHpSlider.value = player.Health;
        }

        private void Pause()
        {
            _isGameStart = false;
        }

        private void Resume()
        {
            _isGameStart = true;
        }

        private void FixedUpdate()
        {
            if (_isGameStart)
            {
                PlayerMovement();
                FallowCamera();
            }
        }

        private void PlayerMovement()
        {
            if (player == null)
            {
                PlayerInit();
            }

            Vector3 moveVector = (Vector3.right * variableJoystick.Horizontal + Vector3.up * variableJoystick.Vertical);
            float deltaSpeed = player.MoveSpeed * Time.deltaTime;
            player.transform.DOMove(player.transform.position + moveVector * deltaSpeed, moveDuration);
            _playerAnimator.SetFloat("Walk", moveVector.magnitude);

            // 조이스틱 조작 시 공격 
            if (moveVector.magnitude > 0 && _isAttack == false)
            {
                PlayerAttack(moveVector).Forget();
            }

            if (moveVector != Vector3.zero)
            {
                float angle = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;
                float yRotation = (angle > 90 || angle < -90) ? 0f : 180f;
                player.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            }
        }

        /// <summary>
        /// 플레이어 공격 호출 
        /// </summary>
        /// <param name="attackAngle">공격 방향</param>
        private async UniTask PlayerAttack(Vector3 attackAngle)
        {
            _isAttack = true;
            player.AttackAngle = attackAngle;
            player.PlayState = Player_Base.playerState.Attack;
            
            await UniTask.Delay(100);
            _isAttack = false;
        }

        /// <summary>
        /// 카메라 추적 
        /// </summary>
        private void FallowCamera()
        {
            Vector3 cameraPosition = new Vector3(player.transform.position.x, player.transform.position.y,
                cameraTransform.position.z);
            cameraTransform.DOMove(cameraPosition, moveDuration);
        }
    }
}