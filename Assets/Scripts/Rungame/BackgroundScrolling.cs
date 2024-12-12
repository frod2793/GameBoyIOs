using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogGuns_Games.Run
{


    public class BackgroundScrolling : MonoBehaviour
    {
        public float speed;
        public SpriteRenderer[] backgrounds; // 배경 스프라이트 배열
        [SerializeField] private RunGameUiManager gameUiManager;

        private float leftPosX;
        private float backgroundWidth; // 각 배경의 너비
        private float xScreenHalfSize;
        private float yScreenHalfSize;
        private float overlapAmount = 0.1f; // 겹칠 양을 설정

        void Start()
        {
            yScreenHalfSize = Camera.main.orthographicSize; // 카메라 높이의 절반
            xScreenHalfSize = yScreenHalfSize * Camera.main.aspect; // 카메라 너비의 절반

            // 배경 스프라이트 중 첫 번째의 너비를 사용하여 계산
            backgroundWidth = backgrounds[0].bounds.size.x;

            // 왼쪽 기준점 설정
            leftPosX = -(xScreenHalfSize * 2); // 화면의 왼쪽 끝
        }

        void Update()
        {
            if (gameUiManager.isPlay)
            {
                // 각 배경 스프라이트에 대해 위치를 갱신
                for (int i = 0; i < backgrounds.Length; i++)
                {
                    // 배경 스프라이트를 왼쪽으로 이동
                    backgrounds[i].transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;

                    // 스프라이트가 화면 왼쪽을 벗어났으면 오른쪽 끝으로 위치를 변경
                    if (backgrounds[i].transform.position.x < leftPosX - backgroundWidth / 2)
                    {
                        // 배경이 이어질 위치 계산 (겹칠 양을 추가로 빼서 약간 겹치게 함)
                        Vector3 nextPos = backgrounds[i].transform.position;

                        // 마지막 배경 스프라이트의 위치를 계산하여 첫 번째와 연결
                        float rightMostX = GetRightmostPositionX();
                        nextPos = new Vector3(rightMostX + backgroundWidth - overlapAmount, nextPos.y, nextPos.z);

                        // 배경의 위치를 새롭게 설정
                        backgrounds[i].transform.position = nextPos;
                    }
                }
            }
        }

        // 가장 오른쪽에 있는 배경 스프라이트의 X 좌표를 반환하는 함수
        private float GetRightmostPositionX()
        {
            float rightMostX = backgrounds[0].transform.position.x;

            // 모든 스프라이트를 순회하며 가장 오른쪽에 있는 스프라이트의 X 좌표를 찾음
            for (int i = 1; i < backgrounds.Length; i++)
            {
                if (backgrounds[i].transform.position.x > rightMostX)
                {
                    rightMostX = backgrounds[i].transform.position.x;
                }
            }

            return rightMostX;
        }
    }
}