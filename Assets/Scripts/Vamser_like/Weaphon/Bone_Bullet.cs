
using System.Threading.Tasks;
using Cysharp.Threading.Tasks; 
using DG.Tweening; 
using DogGuns_Games.vamsir; 
using UnityEngine; 

public class Bone_Bullet : MonoBehaviour
{
    // 총알 오브젝트 풀 관리 객체
    public Weaphon_Bone objectPool_Spawner;

    // 공격 방향을 저장하는 벡터
    private Vector3 attackAngle;

    // 총알의 이동 속도
    private float bulletSpeed = 5;
    
    private float rotateSpeed = 1f;

    // 총알의 데미지
    public float bulletDamage = 10f;

    private bool isActive = false;

    void OnEnable()
    {
        isActive = true;
        MoveAndRotateBullet().Forget();
    }

    // 총알 이동과 회전 함수 (UniTask 사용)
    private async UniTaskVoid MoveAndRotateBullet()
    {
        while (isActive)
        {
            // 총알 이동
            transform.Translate(attackAngle * bulletSpeed * Time.deltaTime, Space.World);
            
            // Z 축으로 오브젝트 회전 (이동 방향에 영향을 주지 않음)
            transform.Rotate(0, 0, 360 * Time.deltaTime*rotateSpeed); // 회전 속도는 필요에 따라 조절 가능합니다.

            CheckBounds();

            await UniTask.Yield();
        }
    }

    // 화면 밖으로 나가면 오브젝트 비활성화 함수
    private void CheckBounds()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            if (objectPool_Spawner != null)
            { 
                isActive = false;
                objectPool_Spawner.objectPool.Release(this);
               
            }
      
        }
    }

    // 총알 발사 방향 설정 함수
    public void Thow_Bullet(Vector3 direction)
    {
        attackAngle = direction.normalized;
    }
}