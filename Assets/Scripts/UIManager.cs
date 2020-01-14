using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public TextMeshProUGUI Txt_FeedBack;
	public TextMeshProUGUI Txt_ComboCounter;
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

	public void ShowComboCount(int count)
	{
		Txt_ComboCounter.text = "";

		if (count != 0)
			Txt_ComboCounter.text = "x" + count.ToString();
	}
}
