using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace Unicorn
{
    public class BackgroundRender : MonoBehaviour
    {

        private Vector3 startPos;
        [SerializeField] public float speed;
        private float singleTextureWidth;
        //public bool isMoving;
        private void Awake()
        {
            startPos = transform.position;
            Sprite sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            singleTextureWidth = sprite.texture.width / sprite.pixelsPerUnit;
        }


        private void Move()
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }

        private void FixedUpdate()
        {
            if (PlayingManager.Instance.isBackGoundMoving)
            {
                Move();
                CheckReset();
            }
            
        }

        private void CheckReset()
        {
            if(Mathf.Abs(transform.position.x) - singleTextureWidth > 0)
            {
                transform.position = startPos;
            }
        }

       
    }
}