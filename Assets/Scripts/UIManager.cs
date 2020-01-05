using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public TextMeshProUGUI Txt_FeedBack;
	public static UIManager Instance;

	void Awake()
    {
		if (Instance == null)
			Instance = this;
		else
			Destroy(this.gameObject);

		DontDestroyOnLoad(this);
    }

	public void ShowFeedback(string feedback)
	{
		Txt_FeedBack.gameObject.SetActive(true);
		Txt_FeedBack.text = feedback;
		Invoke("DisableText", 1.5f);
	}

	private void DisableText()
	{
		Txt_FeedBack.gameObject.SetActive(false);
	}
}
