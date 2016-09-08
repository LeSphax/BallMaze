using UnityEngine;
using UnityEngine.UI;

public class InitLevelName : MonoBehaviour {

	void Start()
    {
        GetComponent<InputField>().text = Settings.GetFirstLevelName();
    }
}
