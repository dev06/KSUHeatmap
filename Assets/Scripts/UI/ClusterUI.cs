using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ClusterUI : MonoBehaviour {

	private bool running;
	public GameObject ClusterSize;
	public GameObject PointScale;
	public GameObject DistanceThreshold;
	public GameObject CentroidDelay;

	public InputField input_location;
	public InputField input_data;

	private Slider clusterSlider;
	private Slider pointSlider;
	private Slider distanceSlider;
	private Slider centroidSlider;

	private Text clusterSliderText;
	private Text pointSliderText;
	private Text distanceSliderText;
	private Text cellDensityText;




	Cluster cluster;
	DataParser dataParser;
	Navigation.Grid grid; 


	void Start ()
	{
		clusterSlider = ClusterSize.transform.GetChild(1).GetComponent<Slider>();
		clusterSliderText = ClusterSize.transform.GetChild(2).GetComponent<Text>();


		pointSlider = PointScale.transform.GetChild(1).GetComponent<Slider>();
		pointSliderText = PointScale.transform.GetChild(2).GetComponent<Text>();


		distanceSlider = DistanceThreshold.transform.GetChild(1).GetComponent<Slider>();
		distanceSliderText = DistanceThreshold.transform.GetChild(2).GetComponent<Text>();

		centroidSlider = CentroidDelay.transform.GetChild(1).GetComponent<Slider>();
		cellDensityText = CentroidDelay.transform.GetChild(2).GetComponent<Text>();


		cluster = Cluster.Instance;
		dataParser = DataParser.Instance;

		input_location.text = "/Atrium/3rd floor atrium_Info.csv";
		input_data.text = "/Atrium/3rd floor atrium_No_Filter.csv";

		grid = FindObjectOfType<Navigation.Grid>(); 

	}

	void Update ()
	{
		cluster.clusterSize = (int)clusterSlider.value;
		cluster.pointScale = pointSlider.value;
		dataParser.distanceThreshold = distanceSlider.value;
		grid.celldensity = (int)centroidSlider.value;

		clusterSliderText.text = cluster.clusterSize.ToString();
		pointSliderText.text = cluster.pointScale.ToString("F3");
		cellDensityText.text = grid.celldensity.ToString("F2");
		distanceSliderText.text = dataParser.distanceThreshold.ToString("F3");
		dataParser.locationfile = input_location.text;
		dataParser.datafile = input_data.text;

	}


	public void StartButtonPress()
	{
		if (running) { return; }
		dataParser.Init();
		//dataParser.BuildAll();
		running = true;
	}
	public void StopButtonPress()
	{
		Cluster.Instance.ClearArea(false);
		running = false;
	}

	public void SetPresets(PresetButtons preset)
	{
		input_location.text = preset.text_location;
		input_data.text = preset.text_data;
		clusterSlider.value = (int)preset.num_clusters;
		pointSlider.value = preset.scale_point;
		distanceSlider.value = preset.threshold_distance;
		centroidSlider.value = preset.delay_calculatecentroid;
	}
}
