using System.Collections;
using UnityEngine;

public class PreMatchPersistentUI : MonoBehaviour
{
    public UnityEngine.UI.Text Text;

    public BaseAccessor Accessor { get; set; }
    public UnityEngine.UI.Button Quit;

    private void Start()
    {
        StartCoroutine(Refresh());
        Quit.onClick.AddListener(delegate
        {
            PreMatchManager.ResumeStart();
        });
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator Refresh()
    {
        while (isActiveAndEnabled) {
            if (Accessor != null)
            {
                Text.text = Accessor.PlayerListText;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
}
