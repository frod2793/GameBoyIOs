using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization; // DoTween 사용

public class BAM_PlayerControll : MonoBehaviour
{
    [FormerlySerializedAs("PlayerSprite")]
    [Header("<color=green>Player Object")]
    [SerializeField] private Player_Base player;
    [Header("<color=green>Joystick")]
    [SerializeField] private VariableJoystick variableJoystick;
    [Header("<color=green>camera")]
    [SerializeField] private Transform cameraTransform;
  
    [Header("<color=green>Move Duration")]
    [SerializeField] private float moveDuration = 0.1f;
 
private void Start()
{
    player = FindFirstObjectByType<Player_Base>();
}

    private void FixedUpdate()
    {
        PlayerMovement();
        FallowCamera();
    }
    
    private void PlayerMovement()
    {
        Vector3 moveVector = (Vector3.right * variableJoystick.Horizontal + Vector3.up * variableJoystick.Vertical);
        float deltaSpeed = player.moveSpeed * Time.deltaTime;
        player.transform.DOMove( player.transform.position + moveVector * deltaSpeed, moveDuration);

        if (moveVector != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;
            player.transform.DORotateQuaternion(Quaternion.AngleAxis(angle, Vector3.forward), moveDuration);
        }
    }
    
    
    private void FallowCamera()
    {
        Vector3 cameraPosition = new Vector3(player.transform.position.x, player.transform.position.y, cameraTransform.position.z);
        cameraTransform.DOMove(cameraPosition, moveDuration);
    }
    
}