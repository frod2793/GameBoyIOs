using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogGuns_Games.Run
{
    public class Obect_base : MonoBehaviour
    {
        protected float Speed;
        protected Rigidbody2D objRigidbody;
        [SerializeField] protected RunGameUiManager uimanager;


        private void Awake()
        {
            BaseInit();
        }


        protected virtual void BaseInit()
        {
            uimanager = FindAnyObjectByType<RunGameUiManager>();
            objRigidbody = GetComponent<Rigidbody2D>();
            Speed = uimanager.CoinSpeed;
        }
    }
}