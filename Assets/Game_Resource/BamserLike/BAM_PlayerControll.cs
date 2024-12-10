using UnityEngine;
using DG.Tweening; // DoTween 사용

public class BAM_PlayerControll : MonoBehaviour
{
    [Header("<color=green>Player Object")]
    [SerializeField] private SpriteRenderer PlayerSprite;
    [Header("<color=green>Joystick")]
    [SerializeField] private VariableJoystick variableJoystick;
    [Header("<color=green>camera")]
    [SerializeField] private Transform cameraTransform;

    [Header("<color=green>Player Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 200f;
    [SerializeField] private float moveDuration = 0.25f; // 이동 애니메이션 시간


    private void FixedUpdate()
    {
        PlayerMovement();
        FallowCamera();
    }
    
    private void PlayerMovement()
    {
        Vector3 moveVector = (Vector3.right * variableJoystick.Horizontal + Vector3.up * variableJoystick.Vertical);
        float deltaSpeed = moveSpeed * Time.deltaTime;
        PlayerSprite.transform.DOMove( PlayerSprite.transform.position + moveVector * deltaSpeed, moveDuration);

        if (moveVector != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;
            PlayerSprite.transform.DORotateQuaternion(Quaternion.AngleAxis(angle, Vector3.forward), moveDuration);
        }
    }
    
    
    private void FallowCamera()
    {
        Vector3 cameraPosition = new Vector3(PlayerSprite.transform.position.x, PlayerSprite.transform.position.y, cameraTransform.position.z);
        cameraTransform.DOMove(cameraPosition, moveDuration);
    }
    
}