using UnityEngine;
using static gamedevrobot.poupup.PopupManager;

namespace gamedevrobot.popup
{
    public class WorldSpacePopup : MonoBehaviour
    {
        public TextMesh mainText;
        public SpriteRenderer spriteRenderer;
        public float scaleDuration = 0.5f;
        public float duration = 3f;

        public void SpawnPopUp(PopUpQueueItem popup)
        {
            this.transform.position = popup.target.transform.position;
            this.transform.localScale = Vector3.zero;
            spriteRenderer.sprite = popup.sprite;
            spriteRenderer.color = popup.spriteColor;
            mainText.text = popup.text;

            // Scale and move the object simultaneously over 3 seconds
            LeanTween.scale(gameObject, new Vector3(0.04f, 0.04f, 0.04f), scaleDuration)
                .setEase(LeanTweenType.easeInOutQuad);

            LeanTween.moveLocalY(gameObject, transform.localPosition.y + 1f, duration)
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(() => gameObject.SetActive(false));
        }
    }

}