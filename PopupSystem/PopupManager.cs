using gamedevrobot.popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gamedevrobot.poupup
{
    public class PopUpManager : MonoBehaviour
    {
        public static PopUpManager Instance;

        [Header("Settings")]
        [SerializeField] private float processInterval;
        [SerializeField] private GameObject PopupPrefab;
        [SerializeField] private Transform PrefabPool;

        private Queue<PopUpQueueItem> popupQueue;
        private HashSet<GameObject> processed;

        private void Awake()
        {
            Instance = this;
        }

        public struct PopUpQueueItem
        {
            public GameObject target;
            public Sprite sprite;
            public string text;
            public Color spriteColor;

            public PopUpQueueItem(Sprite sprite, Color spriteColor, string text, GameObject target)
            {
                this.target = target;
                this.sprite = sprite;
                this.spriteColor = spriteColor;
                this.text = text;
            }
        }

        void Start()
        {
            processed = new();
            popupQueue = new();
            StartCoroutine(processQueueItems());
        }

        private IEnumerator processQueueItems()
        {
            processed.Clear();

            Queue<PopUpQueueItem> notProcessed = new();
            while (popupQueue.Count > 0)
            {
                PopUpQueueItem item = popupQueue.Dequeue();
                if (!processed.Contains(item.target))
                {
                    processed.Add(item.target);
                    WorldSpacePopup popup = GetFromPool();
                    popup.gameObject.SetActive(true);
                    popup.SpawnPopUp(item);
                }
                else
                {
                    notProcessed.Enqueue(item);
                }
            }
            popupQueue = notProcessed;

            yield return new WaitForSeconds(processInterval);
            StartCoroutine(processQueueItems());
        }

        private WorldSpacePopup GetFromPool()
        {
            GameObject popupObject = null;
            foreach (Transform child in PrefabPool)
            {
                if (!child.gameObject.activeInHierarchy)
                {
                    popupObject = child.gameObject;
                    break;
                }
            }
            if (popupObject == null)
            {
                popupObject = Instantiate(PopupPrefab, PrefabPool);
            }
            return popupObject.GetComponent<WorldSpacePopup>();
        }

        public void SpawnPopUp(Sprite sprite, Color spriteColor, string text, GameObject target)
        {
            popupQueue.Enqueue(new PopUpQueueItem(sprite, spriteColor, text, target));
        }
    }
}