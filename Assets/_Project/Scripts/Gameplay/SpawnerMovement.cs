using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Gameplay
{
    public class SpawnerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [Tooltip("Spawner speed in world units per seconds")]
        [Min(0f)]
        [SerializeField] private float moveSpeed = 4f;
        
        [Header("Input")]
        [Tooltip("Gameplay Move Action from GameControls")]
        [SerializeField] private InputActionReference moveAction = null;
        
        [Header("Bounds")]
        [Tooltip("Left limit of the spawn point.")]
        [SerializeField] private float minX = -3.32f;
        
        [Tooltip("Right limit of the spawn point.")]
        [SerializeField] private float maxX = 3.32f;
        
        [Tooltip("Lowest spawn point. Spawn zone bottom + ball radius.")]
        [SerializeField] private float minY = 3.43f;
        
        [Tooltip("Highest spawn point. Ceiling bottom face - margin - ship top offset.")]
        [SerializeField] private float maxY = 3.94f;
        
        [Header("Debug")]
        [Tooltip("Radius of the Scene view ball gizmo. Match the ball’s world radius.")]
        [Min(0f)]
        [SerializeField] private float gizmoBallRadius = 0.15f;

        private void OnEnable()
        {
            if (moveAction == null)
            {
                Debug.LogError($"SpawnerMovement on '{gameObject.name}': moveAction is not assigned. Assign Gameplay/Move from GameControls. Disabling component.", this);
                enabled = false;
                return;
            }
            moveAction.action.Enable();
        }

        private void OnDisable()
        {
            if(moveAction != null) moveAction.action.Disable();
        }

        private void Start()
        {
            transform.position = ClampToBounds(transform.position);
        }

        private void Update()
        {
            Vector2 currentInput = moveAction.action.ReadValue<Vector2>();
            Vector2 delta = currentInput * (moveSpeed * Time.deltaTime);
            Vector3 position = transform.position;

            position.x += delta.x;
            position.y += delta.y;
            transform.position = ClampToBounds(position);
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            
            Vector3 rectangleCenter;
            rectangleCenter.x = (minX + maxX) / 2;
            rectangleCenter.y = (minY + maxY) / 2;
            rectangleCenter.z = 0;

            Vector3 rectangleSize;
            rectangleSize.x = maxX - minX;
            rectangleSize.y = maxY - minY;
            rectangleSize.z = 0;
            
            Gizmos.DrawWireCube(rectangleCenter, rectangleSize);
            
            Gizmos.color = Color.darkRed;
            Gizmos.DrawWireSphere(transform.position, gizmoBallRadius);

        }

        private void OnValidate()
        {
            if (minX > maxX)
            {
                Debug.LogWarning($"SpawnerMovement on '{gameObject.name}': minX ({minX}) is greater than maxX ({maxX}). Spawner X clamp will misbehave.", this);
            }

            if (minY > maxY)
            {
                Debug.LogWarning($"SpawnerMovement on '{gameObject.name}': minY ({minY}) is greater than maxY ({maxY}). Spawner Y clamp will misbehave.", this);
            }
        }

        private Vector3 ClampToBounds(Vector3 inVec)
        {
            inVec.x = Mathf.Clamp(inVec.x, minX, maxX);
            inVec.y = Mathf.Clamp(inVec.y, minY, maxY);
            return inVec;
        }
        
    }
}
