using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Zombie
{
    public class ZombieTest : MonoBehaviour
    {
        private Rigidbody m_Rigidbody;
        private Transform m_Transform;

        private float CacheRotationY;
        
        // Start is called before the first frame update
        void Start()
        {
            m_Rigidbody = gameObject.GetComponent<Rigidbody>();
            m_Transform = gameObject.GetComponent<Transform>();
            CacheRotationY = m_Transform.localRotation.y;
        }

        // Update is called once per frame
        void Update()
        {
            /*if (Input.GetKey(KeyCode.W))
            {
                m_Rigidbody.MovePosition(m_Transform.position + Vector3.forward * 0.2f);
            }

            if (Input.GetKey(KeyCode.S))
            {
                m_Rigidbody.MovePosition(m_Transform.position + Vector3.back * 0.2f);
            }

            if (Input.GetKey(KeyCode.A))
            {
                m_Rigidbody.MovePosition(m_Transform.position + Vector3.left * 0.2f);
            }

            if (Input.GetKey(KeyCode.D))
            {
                m_Rigidbody.MovePosition(m_Transform.position + Vector3.right * 0.2f);
            }*/
            CacheRotationY = m_Transform.localRotation.y;
            m_Transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("丧尸撞击了");
        }
    }
}
