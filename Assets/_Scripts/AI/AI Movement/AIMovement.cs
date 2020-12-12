using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [Header("Ai movement settings")]
    public float m_moveSpeed = 2f;
    [HideInInspector] public CharacterController characterController;
    public GameObject target;

    [Header("Raycast Settings")]
    public int raycastAmount = 16;
    public float raycastLength = 5;
    [SerializeField] private float raycastThreshold = 0.98f;

    private bool isMoving = true;

    public void setTarget(GameObject newTarget) => target = newTarget;

    private void LateUpdate()
    {
        move(getDir());
    }

    private bool getRay(Ray ray)
    {
        bool isHit = false;

        var lookPercentage = Vector3.Dot(ray.direction, target.transform.position);

        isHit = lookPercentage > raycastThreshold;

        return isHit;
    }

    private Vector3 getDir()
    {
        Vector3 vec = target.transform.position - gameObject.transform.position;

        Ray ray = new Ray(transform.position, vec);
        RaycastHit hitInfo;

        Physics.Raycast(ray, out hitInfo);

        if (hitInfo.collider.gameObject != target)
        {
            vec.x += 10;
        }


        Debug.DrawLine(transform.position, hitInfo.point, Color.cyan, 0.1f);
        Debug.Log(hitInfo.collider.name);

        vec.y = 0;
        return vec;
    }

    private void move(Vector3 dir)
    {
        if (isMoving) characterController.Move(dir * m_moveSpeed * Time.deltaTime);
    }


    private void Awake()
    {
        
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
}
