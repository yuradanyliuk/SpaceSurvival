﻿using UnityEngine;

namespace Player
{
    public class PlayerOnBaseMovement : MonoBehaviour
    {
        #region Properties
        public bool IsPlayerOnTheBasePosition => Mathf.Abs(transform.position.x) < basePositionRadius && Mathf.Abs(transform.position.z) < basePositionRadius;
        #endregion

        #region Fields
        [Header("Keys")]
        [SerializeField] KeyCode keyToTakeOffFromBase;
        [SerializeField] KeyCode keyToLandsOnTheBase;
        [Header("Base position")]
        [SerializeField] Vector3 playerOnBasePosition;
        [SerializeField] float basePositionRadius;

        Player playerScript;
        Vector3 targetPosition = new Vector3();

        bool isLandsOnTheBase;
        bool isTakeOffFromBase;
        bool isPlayerOnTheBase = true;

        float distanse;
        float playerSpeedController;
        float playerDefaultYPosition;
        #endregion

        #region Methods
        // Start is called before the first frame update
        void Start()
        {
            playerScript = GetComponent<Player>();
            playerDefaultYPosition = playerScript.DefaultYPosition;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isPlayerOnTheBase && Input.GetKeyDown(keyToLandsOnTheBase) && IsPlayerOnTheBasePosition && !isTakeOffFromBase)
                StartLandsOnTheBase();
            else if (!playerScript.AreResourcesBeingUnloaded && isPlayerOnTheBase)
                playerScript.UnloadResources();
            else if (playerScript.AreResourcesBeingUnloaded && Input.GetKeyDown(keyToTakeOffFromBase) && isPlayerOnTheBase)
                StartTakeOffFromBase();
        }
        void FixedUpdate()
        {
            if (isLandsOnTheBase)
                LandsOnTheBaseUpdate();
            else if (isTakeOffFromBase)
                TakeOffFromBaseUpdate();
        }

        public void StartLandsOnTheBase()
        {
            isLandsOnTheBase = true;
            playerScript.IsActive = false;
        }
        void LandsOnTheBaseUpdate()
        {
            distanse = Vector3.Distance(transform.position, playerOnBasePosition);
            playerSpeedController = distanse < 1f ? 1f : distanse;

            transform.position = Vector3.MoveTowards(transform.position, playerOnBasePosition, playerSpeedController * 2f * Time.deltaTime);
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation,
                Quaternion.Euler(0f, 90f, 0f), (playerSpeedController + 2f) / playerSpeedController * Time.deltaTime);

            if (transform.position.y <= playerOnBasePosition.y)
                FinishLandsOnTheBase();
        }
        void FinishLandsOnTheBase()
        {
            isPlayerOnTheBase = true;
            isLandsOnTheBase = false;
        }

        public void StartTakeOffFromBase()
        {
            isPlayerOnTheBase = false;
            isTakeOffFromBase = true;
            targetPosition.Set(Random.Range(-2f, 2f), playerDefaultYPosition, Random.Range(-2f, 2f));
        }
        void TakeOffFromBaseUpdate()
        {
            distanse = Vector3.Distance(transform.position, targetPosition);
            playerSpeedController = distanse < 1f ? 1f : distanse;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 4f / playerSpeedController * Time.deltaTime);
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation,
                Quaternion.Euler(0f, Quaternion.LookRotation(transform.position, targetPosition).eulerAngles.y, 0f),
                playerSpeedController / (playerSpeedController - 0.5f) * Time.deltaTime);

            if (transform.position.y >= playerDefaultYPosition)
                FinishTakeOffFromBase();
        }
        void FinishTakeOffFromBase()
        {
            isTakeOffFromBase = false;
            playerScript.IsActive = true;
        }
        #endregion
    }
}