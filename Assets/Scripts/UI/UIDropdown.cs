using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class UIDropdown : MonoBehaviour {


	public enum DropdownType
	{
		NONE, 
		LOCATION, 
		DATA,
	}

	public DropdownType type; 

	private Dropdown dropdown; 

	void Start () 
	{
		dropdown = GetComponentInChildren<Dropdown>();
		AddOptions(); 
	}

	private void AddOptions()
	{
		List<UnityEngine.UI.Dropdown.OptionData> options = new List<UnityEngine.UI.Dropdown.OptionData>(); 

		if(type == DropdownType.LOCATION)
		{
			foreach (string file in System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/Location"))
			{ 
				if(file.Contains(".meta")) continue; 
				UnityEngine.UI.Dropdown.OptionData data = new UnityEngine.UI.Dropdown.OptionData(); 
				data.text = ExtractName(file); 
				options.Add(data); 
			}

			dropdown.AddOptions(options); 


			DataParser.Instance.SetLocationPath(dropdown.options[0].text);

		}

		if(type == DropdownType.DATA)
		{
			foreach (string file in System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/Data"))
			{ 
				if(file.Contains(".meta")) continue; 
				UnityEngine.UI.Dropdown.OptionData data = new UnityEngine.UI.Dropdown.OptionData(); 
				data.text = ExtractName(file); 
				options.Add(data); 
			}

			dropdown.AddOptions(options); 

			dropdown.value = 3; 

			DataParser.Instance.SetDataPath(dropdown.options[dropdown.value].text);


		}

		dropdown.onValueChanged.AddListener(delegate {
			DropdownValueChanged(dropdown);
			});

	}

	private string ExtractName(string fullName)
	{
		string[] data = fullName.Split('/');
		return data[data.Length - 1]; 
	}

	void DropdownValueChanged(Dropdown change)
	{
//		Debug.Log(change.options[change.value].text);  
		switch(type)
		{
			case DropdownType.LOCATION:{
				DataParser.Instance.SetLocationPath(change.options[change.value].text);
				break;
			}

			case DropdownType.DATA:
			{
				DataParser.Instance.SetDataPath(change.options[change.value].text);
				break; 
			}
		}

	}
}
