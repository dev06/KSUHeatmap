using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class DataParser : MonoBehaviour {

	public static DataParser Instance;
	public List<KSUHeatmap.DataPoint> dataPoints = new List<KSUHeatmap.DataPoint>();
	public float width = 0;
	public float height = 0;

	private string fileContents = "";
	private  string saveLocation;


	private string locationFileContents;
	private string beaconFileContents;

	public Location activeLocation;  // represents one active location
	public List<Session> activeSession; // represents current session of the active location



	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Start () {

		Init();
	}

	private void Init()
	{
		activeLocation = ParseLocation(Application.dataPath + "/Datapoint/location.txt");

		activeSession = ParseDatapoints(Application.dataPath + "/Datapoint/datapoint.csv");



		List<string> walls = new List<string>();
		walls.Add("0.432, 0.000");
		walls.Add("0.432, 1.618");
		walls.Add("0.000, 1.618");
		walls.Add("0.000, 9.179");
		walls.Add("0.432, 9.179");
		walls.Add("0.432, 9.462");
		walls.Add("8.989, 9.462");
		walls.Add("8.989, 0.000");
		LocationBuilder.BuildWalls(walls);

		List<string> beacons = new List<string>();
		beacons.Add("beacon_1,  3.137,  0.000,  000");
		beacons.Add("beacon_2,  1.091,  9.462,  180");
		beacons.Add("beacon_3,  8.989,  1.946,  270");
		beacons.Add("beacon_4,  8.989,  7.290,  270");
		beacons.Add("beacon_5,  3.037,  9.462,  180");
		beacons.Add("beacon_6,  0.000,  7.477,  090");
		beacons.Add("beacon_7,  0.000,  5.632,  090");
		beacons.Add("beacon_8,  0.000,  1.838,  090");
		LocationBuilder.BuildBeacons(beacons);

		List<string> datapoints = new List<string>();
		// datapoints.Add("0,0,0,0,clo1");
		// datapoints.Add("5,1,0,90,clo1,clo2,clo3,clo4");
		// datapoints.Add("10,2,5,15,clo1,clo3,clo5");
		// datapoints.Add("20,5,5,90,clo1,clo2,clo4");
		// datapoints.Add("30,6,2,270,clo3,clo5,clo7");
		// datapoints.Add("40,4,3,15,clo5,clo7,clo9");
		// datapoints.Add("50,7,5,15,clo0");
		// datapoints.Add("60,10,10,20,clo0,clo1,clo2");

		datapoints.Add("120, 3.83, 1.654, 87, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("175, 3.83, 1.654, 87, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("238, 3.83, 1.654, 87, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("394, 3.83, 1.654, 86, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("476, 3.83, 1.654, 86, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("598, 3.83, 1.654, 86, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("668, 3.83, 1.654, 85, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("766, 3.83, 1.654, 85, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("878, 3.83, 1.654, 85, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("983, 3.83, 1.654, 84, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("1152, 3.83, 1.654, 85, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("2037, 3.83, 1.654, 84, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("2080, 3.83, 1.654, 83, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("2215, 3.83, 1.654, 82, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("2291, 3.83, 1.654, 83, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("2407, 3.83, 1.654, 83, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("2510, 3.83, 1.654, 83, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("2589, 3.83, 1.654, 84, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("2666, 3.8250443944993706, 1.6325140815557837, 85, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("2748, 3.8164621135605983, 1.5953040598354535, 85, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("2840, 3.805306060151734, 1.5469349849218588, 85, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("2884, 3.7924067722969843, 1.4910078037125365, 87, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("2952, 3.7784152887729654, 1.4303452118511633, 86, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("3108, 3.763837721693218, 1.3671415493823182, 86, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("3270, 3.74906330961732, 1.3030844300487336, 86, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("3330, 3.734387259341533, 1.2394537759805853, 86, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("3445, 3.720029374209422, 1.1772025840847062, 89, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("3554, 3.706149250285014, 1.117022811784922, 85, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("3640, 3.692858664069746, 1.0593990861956288, 88, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("3737, 3.680231656287431, 1.0046524241841266, 86, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("3824, 3.6683127234968604, 0.9529757485795475, 86, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("3924, 3.6571234554617114, 0.9044626656831887, 81, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("4031, 3.646667896528265, 0.8591307104853452, 79, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("4147, 3.636936860517782, 0.8169400546568275, 82, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("4305, 3.6279113885644816, 0.7778084986270877, 77, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("4387, 3.6195655062492196, 0.741623425632886, 77, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("4503, 3.611868409011849, 0.7082512769664249, 67, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("4618, 3.604786182161437, 0.677545009388817, 78, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("4697, 3.598283143029302, 0.6493499142758485, 73, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("4801, 3.5923228772608287, 0.6235081106474112, 77, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("4906, 3.5918139684165404, 0.6213016448739503, 75, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("4998, 3.5954020560625293, 0.6368584438408212, 73, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("5082, 3.6020030881946834, 0.6654784052201304, 73, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("5204, 3.6107538108702593, 0.703418736368663, 69, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("5258, 3.6209709558442635, 0.7477170049891776, 67, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("5337, 3.6321176317380974, 0.7960454220007014, 77, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("5404, 3.6437756779083554, 0.8465909767671671, 75, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("5476, 3.655622951057918, 0.8979569591169553, 66, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("5633, 3.662469753195048, 0.927642500531294, 71, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("5678, 3.665452088872833, 0.9405729529529054, 69, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("5843, 3.665486369197304, 0.9407215814617187, 64, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("5921, 3.6633090367102548, 0.9312813650771139, 59, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("6036, 3.659509283509112, 0.914806852047872, 56, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("6101, 3.6545560440009406, 0.8933311917892565, 61, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("7112, 3.6488202437574104, 0.8684625987930736, 51, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("7219, 3.642593119633749, 0.8414637827965206, 48, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("7299, 3.636101288001721, 0.8133172798148746, 51, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("8275, 3.634464059487408, 0.8062187812906046, 44, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("8360, 3.636495070447693, 0.8150245943938577, 49, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("8461, 3.6412434108285785, 0.8356118776544238, 55, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("8553, 3.6479506092673657, 0.8646921421400958, 46, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("8613, 3.656015164608457, 0.8996574714677718, 43, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("8661, 3.6649633283704524, 0.9384538439375532, 39, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("8830, 3.6744250623854304, 0.9794768925561049, 30, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("8891, 3.6628365007431265, 1.0323862308107217, 28, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("8972, 3.6356533600536234, 1.0933284639875793, 26, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("9104, 3.618505115203245, 1.1483796644691382, 14, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("9203, 3.609168846312557, 1.1980950704216775, 15, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("9276, 3.6058471638728933, 1.2429800403785454, 12, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("9380, 3.607091782645364, 1.2834941005062348, 10, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("9490, 3.611740383680666, 1.3200547439266979, 12, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("9669, 3.618864494729141, 1.353040980759808, 6, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("9727, 3.6277265031033425, 1.3827966420385651, 6, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("9862, 3.6377442343322652, 1.4096334439636076, 6, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("9931, 3.6484617955150798, 1.4338338213412984, 3, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("10039, 3.6595256031436003, 1.4556535406855255, 4, Need to implement (1), Need to implement (2), Need to implement (3)");

		datapoints.Add("10144, 3.6706646988125926, 1.4753241045155918, 358, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("10241, 3.681674608917457, 1.4930549589795408, 358, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("10378, 3.705228565346109, 1.5311432374858127, 350, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("10440, 3.737797990577119, 1.5838647793820468, 357, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("10537, 3.7765861936560454, 1.6466825764558122, 358, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("10599, 3.819392010069593, 1.7160256016281623, 357, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("10706, 3.864497606210547, 1.7891068307882452, 359, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("10795, 3.9105762772113746, 1.8637736924531267, 360, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("10915, 3.9566167765070928, 1.938385330802193, 358, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("10996, 4.001861305369051, 2.0117120243585807, 357, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("11084, 4.045754780924657, 2.082852897761186, 344, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("12139, 4.087903408574572, 2.1511687248250793, 327, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("13257, 4.12804092316772, 2.216227170017206, 332, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("13397, 4.166001144391741, 2.2777582713856397, 334, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("13568, 4.201695725235183, 2.335618346530163, 336, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("13639, 4.196130065447178, 2.397786430446386, 331, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("13739, 4.159711986182054, 2.4621493862094472, 333, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("13811, 4.100761560296843, 2.5270857799282522, 336, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("13922, 4.025894381544655, 2.5913699788852185, 334, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("14006, 3.940337424454769, 2.654093628454856, 325, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("14108, 3.8481890828964422, 2.7146014718245035, 337, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("14191, 3.752633008679423, 2.772438996487997, 316, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("14275, 3.656113736205335, 2.827309823377484, 335, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("14363, 3.56048071957968, 2.8790411131549747, 335, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("14536, 3.4613312784507775, 2.9298597286021764, 344, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("14613, 3.358450342963254, 2.998686028044208, 343, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("14744, 3.2544854689648908, 3.0798709854916617, 346, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("14821, 3.15144038898337, 3.1689718538190577, 349, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("14918, 3.0508025725141734, 3.262524640834287, 337, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("15015, 2.953647500620373, 3.3578571537484705, 336, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("15115, 2.868425960578572, 3.461361696982745, 320, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("15269, 2.7935749284264673, 3.569270187236516, 330, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("15422, 2.729712786457662, 3.6886446708538547, 313, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("15490, 2.6751566811591445, 3.8146290747669136, 325, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("15549, 2.628489371784985, 3.943471178836545, 324, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("15631, 2.5885164478436633, 4.072309189622445, 325, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("15749, 2.554230528375992, 4.198996830265782, 337, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("15814, 2.524781294254686, 4.321960232624807, 335, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("15903, 2.4974869026483506, 4.430119558294773, 315, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("16053, 2.4722635494550156, 4.525373171020113, 325, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("16119, 2.4490138642913197, 4.609359938824894, 335, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("16241, 2.427631924902581, 4.683496793475842, 338, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("16493, 2.4099706525650317, 4.758972106953742, 353, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("16581, 2.395394235054834, 4.834193342660557, 348, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("16680, 2.3833743989569856, 4.9079703327772775, 356, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("17721, 2.373472329520031, 4.979434522199482, 315, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("18286, 2.3653236195693617, 5.047973041277097, 357, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("18342, 2.3604000267802037, 5.089267379897357, 353, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("18420, 2.3579773317419983, 5.109440344197595, 351, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("18530, 2.3574706882676444, 5.113436061686147, 348, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("18687, 2.3497161941907514, 5.1099547487744585, 320, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("18763, 2.3374828315076805, 5.127241740500007, 336, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("18839, 2.3181864289702494, 5.180829621952501, 295, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("18981, 2.297822131028783, 5.239737261564886, 341, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("19092, 2.2769963164999814, 5.301652925803649, 331, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("19191, 2.262262731280761, 5.357659744266946, 330, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("19249, 2.2523438777973808, 5.408301481424255, 326, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("19316, 2.240110117499334, 5.461208315157388, 328, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("19453, 2.2263440730952544, 5.514932190760171, 336, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("19534, 2.2177495195263215, 5.561240513330541, 336, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("19652, 2.2070855568019607, 5.608348302258365, 334, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("19801, 2.195047269923102, 5.655270561138816, 331, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("19954, 2.188270147884287, 5.694138698327793, 332, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("20005, 2.192024470557105, 5.732595822914062, 332, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("20150, 2.1975998047190997, 5.779918735971863, 331, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("20228, 2.204447975261944, 5.833102275165683, 329, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("20321, 2.2121366369147117, 5.88979489164162, 337, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("20393, 2.2203275225390424, 5.94817443913788, 332, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("20490, 2.2287585613816394, 6.00684619244111, 338, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("20604, 2.2372291978238934, 6.064759237544513, 322, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("20686, 2.2455883551918525, 6.1211380340796575, 318, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("20784, 2.2537245839646243, 6.175426497579688, 321, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("20858, 2.2642646791447927, 6.250691292467272, 306, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("20995, 2.276432005877506, 6.340433922647395, 310, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("21049, 2.289616967495749, 6.439539218793919, 306, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("21156, 2.303345405917835, 6.544014759479468, 310, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("22210, 2.317252644635769, 6.650776724557862, 339, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("22737, 2.325108029596957, 6.762531086290693, 326, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("22805, 2.3282936011578985, 6.876167172818804, 335, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("22880, 2.3279237198093066, 6.989320618167908, 318, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("23256, 2.3248934134677075, 7.100226191876938, 300, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("23300, 2.319918293487046, 7.207597435627427, 294, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("23377, 2.3135674819434326, 7.310528411179837, 292, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("23482, 2.3062907485866653, 7.408413669324481, 292, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("23555, 2.3043950194259786, 7.495826273999778, 296, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("23779, 2.306566638898364, 7.573927511828476, 301, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("23826, 2.3117525761876805, 7.643743537225081, 302, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("23882, 2.3191128195515396, 7.706182052874339, 301, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("23987, 2.327981121748575, 7.762046794285599, 308, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("24071, 2.3378326630338755, 7.81205012599484, 326, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("24198, 2.3482574412806194, 7.856824011565745, 319, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("24295, 2.358938400944334, 7.8969295811067175, 326, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("24399, 2.369633480676381, 7.932865487466223, 318, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("24519, 2.3801608991308094, 7.965075214666971, 331, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("24618, 2.390387114652784, 7.993953478711971, 323, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("24695, 2.400216991050052, 8.01985184098941, 329, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("24780, 2.4095857818305024, 8.04308363756989, 332, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("24868, 2.4184526118853316, 8.063928313267972, 338, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("24959, 2.426795190895031, 8.082635237042624, 333, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("25079, 2.434605538637389, 8.09942706481179, 344, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("25176, 2.441886540466867, 8.11450270678069, 347, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("25280, 2.4696491828311946, 8.134467726477688, 349, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("25396, 2.512310344890587, 8.157767029142475, 353, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("25474, 2.565434377353622, 8.183178722322024, 355, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("25587, 2.6255201620783373, 8.209751180167986, 359, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("25722, 2.6898258312539016, 8.236751337216324, 360, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("25782, 2.7562246526146765, 8.263622266048973, 358, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("25924, 2.82308669109168, 8.289948424905976, 352, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("26025, 2.8891817750979043, 8.315427236966961, 334, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("27078, 2.9536000585176074, 8.339845892063252, 292, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("28277, 3.015687103440425, 8.363062451851082, 214, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("28325, 3.0749909354085307, 8.384990497494437, 212, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("28385, 3.1312189604611347, 8.405586690102046, 211, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("28489, 3.1842029965762353, 8.424840723056052, 210, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("28641, 3.2128709737196237, 8.436339457942845, 220, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("28705, 3.222824107015171, 8.44183007369778, 221, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("28811, 3.218575221869912, 8.442723089841241, 221, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("28887, 3.2037456376464695, 8.440153022673705, 228, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("28966, 3.1812276888514464, 8.435028477475713, 224, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("29130, 3.1533187657743214, 8.428073482841512, 221, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("29194, 3.1218317607035724, 8.419861567707391, 217, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("29336, 3.088185977555451, 8.410843827450567, 218, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("29418, 3.053481873901769, 8.40137201402266, 217, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("29489, 3.018562431596478, 8.391717509273576, 218, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("29587, 2.9840634760075684, 8.382086894446092, 220, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("29767, 2.9504548680490936, 8.372634707306933, 218, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("29819, 2.9180741642794192, 8.363473877386337, 219, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("29869, 2.8871540670475064, 8.35468424587834, 219, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("29974, 2.85784475967278, 8.34631950703826, 217, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("30049, 2.8302320331471886, 8.338412850013116, 215, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("30176, 2.798397787700542, 8.336038476410707, 217, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("30254, 2.759071029376104, 8.332795943824276, 228, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("30332, 2.7147803323653954, 8.328956334209606, 240, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("30386, 2.6740076374012745, 8.314619021428442, 248, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("30491, 2.6365529888881625, 8.292647386485429, 256, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("30577, 2.5957083415774145, 8.275430092725633, 266, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("30670, 2.5530019392902856, 8.262065509749249, 271, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("30738, 2.509610994142323, 8.251813202910029, 268, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("30805, 2.4729326020456455, 8.233952086065447, 272, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("30921, 2.441899931272377, 8.210682046997585, 272, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("30998, 2.415618762671219, 8.183743150246729, 270, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("31119, 2.39333924017583, 8.154501591846142, 273, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("32037, 2.3744322776346913, 8.124020396514933, 270, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("32103, 2.3583698511015925, 8.093117493407943, 275, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("32155, 2.3137199924005825, 8.048040114929874, 280, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("32221, 2.2483737238853223, 7.993083169564794, 275, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("33242, 2.1709827937674553, 7.959805741048237, 305, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("33319, 2.0858482162829306, 7.9433425739766825, 301, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("33424, 1.9963248128981552, 7.9397644891067385, 301, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("33515, 1.905001846811271, 7.945909850635835, 294, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("33601, 1.8114627168497606, 7.9310721313476975, 292, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("33721, 1.7178204494816391, 7.900744396768103, 294, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("33824, 1.6256615335146947, 7.85930461296291, 291, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("34000, 1.5361511130314154, 7.810221455301937, 287, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("34073, 1.4501189045022729, 7.756223819876812, 285, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("34208, 1.3705177663637114, 7.727613529643355, 288, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("34310, 1.2970666166537286, 7.718520586474206, 285, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("34376, 1.2294521440936792, 7.724217202656108, 286, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("34456, 1.167342087272081, 7.740911044748266, 284, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("34541, 1.1103955098680143, 7.76557457557158, 286, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("34641, 1.0582706516504974, 7.7958043140962054, 291, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("34760, 1.0106308304598512, 7.829704879421405, 284, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("34847, 0.9671487840592026, 7.865793555424947, 270, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("34921, 0.927509769524396, 7.902921836503855, 266, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("35116, 0.8914136791312178, 7.940211016704638, 236, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("35218, 0.8585763833508139, 7.976999384910149, 226, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("35319, 0.8287304717929973, 8.012799004649112, 214, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("35431, 0.8016255302614183, 8.047260402709183, 204, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("35602, 0.7770280652750436, 8.080143777875751, 193, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("35668, 0.7523326241645193, 8.083122336695052, 193, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("35731, 0.727975291646058, 8.063622640606914, 190, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("35842, 0.7042774961136183, 8.027589248272998, 190, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("35931, 0.6814693165795689, 7.979756140293649, 188, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("36017, 0.6597084853698796, 7.923870464046547, 188, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("36140, 0.641484387206448, 7.891050032460445, 181, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("37160, 0.6262165737971, 7.8760755415631944, 129, Need to implement (1), Need to implement (2), Need to implement (3)");



		datapoints.Add("38155, 0.6134204375101374, 7.874735081030869, 116, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("38230, 0.6026913280030799, 7.883642484281111, 115, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("38353, 0.5936913070727354, 7.900087328647441, 113, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("38447, 0.5861381029237648, 7.921911173111398, 115, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("38562, 0.5797958980966407, 7.9474055369075085, 107, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("38678, 0.5744676461746869, 7.975227884139467, 106, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("38757, 0.5920858853525325, 8.00064640205, 107, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("38861, 0.6266203555355966, 8.02383840600429, 101, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("38951, 0.6732664244695237, 8.044973991092156, 100, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("39008, 0.7330757024288101, 8.069271946723187, 104, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("39143, 0.8017605658577949, 8.095536714334408, 93, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("39221, 0.8759573706652368, 8.122838844803915, 97, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("39341, 0.9530515180930643, 8.150463961909315, 100, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("39415, 1.0310337700290506, 8.17787090037321, 103, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("39493, 1.1083823961472228, 8.204657422548243, 108, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("39655, 1.1839666591333982, 8.230532190470381, 105, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("39722, 1.2569679120541397, 8.255291896570812, 107, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("39860, 1.326815219877127, 8.27880264453662, 108, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("39933, 1.3931329470797487, 8.300984828102033, 108, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("40053, 1.455698193363719, 8.321800885307725, 106, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("40219, 1.514406324853968, 8.341245413456866, 104, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("40326, 1.5692431513907554, 8.359337219344441, 109, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("40481, 1.6235100521035002, 8.347607120104382, 103, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("40605, 1.6764450601030003, 8.313706093672597, 81, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("40682, 1.727498898434242, 8.263741888954215, 92, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("40755, 1.7762909622087695, 8.202563037266192, 102, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("40835, 1.8225734940028522, 8.133992847879071, 94, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("41016, 1.8662025012816998, 8.061021991707413, 86, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("41082, 1.9071142150528073, 7.985966816383546, 87, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("41413, 1.9453060967969245, 7.910599321284253, 98, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("41468, 1.980821573158397, 7.836253711271713, 95, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("41541, 2.0137378208768615, 7.763913608574446, 85, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("41649, 2.044156042984429, 7.694283304741921, 96, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("43262, 2.072193775515364, 7.627845855121829, 83, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("43322, 2.0979788453216424, 7.564910336988063, 114, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("43474, 2.1150957780321016, 7.496690748326826, 108, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("43560, 2.1254263257088555, 7.425645099914341, 114, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("43645, 2.130494908815718, 7.353664000828626, 112, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("43738, 2.131532542110047, 7.282180998722752, 116, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("43868, 2.129529669156738, 7.21226295272019, 132, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("43936, 2.125279797733588, 7.1446839245618206, 133, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("44011, 2.1259643975373326, 7.0889449213670135, 126, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("44095, 2.1270909407645187, 7.0715207536147915, 109, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("44288, 2.125228776949944, 7.0456578374626, 127, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("44366, 2.121156118917625, 7.013896908612969, 110, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("44427, 2.11549346785587, 6.978241819054419, 122, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("44526, 2.1148239693810034, 6.933127423948885, 124, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("44627, 2.1179104936542177, 6.881670456268205, 137, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("44702, 2.1237625929172723, 6.826314175284177, 130, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("44805, 2.1315913384349896, 6.768956014005706, 130, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("44885, 2.140772089551742, 6.7110524118610435, 141, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("44967, 2.1508138323491583, 6.653704789102481, 135, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("45049, 2.1613339565271157, 6.5977299445961375, 122, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("45136, 2.1720375313375127, 6.543717597854063, 156, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("45228, 2.1827003012087327, 6.492077330214225, 145, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("45326, 2.1931547545443832, 6.4430767930283945, 138, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("45459, 2.203278729589429, 6.396872729302834, 144, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("45618, 2.2090316487133896, 6.343888469353617, 143, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("45722, 2.211410393911137, 6.286702133235811, 146, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("45788, 2.2151733271966427, 6.236964964641144, 154, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("45849, 2.219911685993209, 6.193643624498745, 152, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("45921, 2.2253026412764614, 6.155856178031252, 155, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("46053, 2.2345352581620705, 6.112887434657889, 181, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("46119, 2.246495244301354, 6.066748864328013, 181, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("47157, 2.260301424278572, 6.019001899952917, 172, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("47745, 2.27526214663462, 5.9708444227834905, 169, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("47818, 2.2908394304021904, 5.923181685637807, 164, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("48204, 2.296258402721165, 5.848719104684581, 155, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("48310, 2.2890249919150434, 5.747546850558252, 147, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("48389, 2.2724228506118678, 5.628028545787535, 143, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("48458, 2.249070863790955, 5.496742674779704, 138, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("48559, 2.227789025945421, 5.340703393610836, 140, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("48626, 2.217253386017736, 5.173128876174005, 141, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("48773, 2.214952988527232, 4.999695840160148, 144, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("48844, 2.2100284739339, 4.820330299147086, 131, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("48964, 2.2032179467566695, 4.639504805068172, 164, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("49057, 2.1951064189278022, 4.460598104424516, 144, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("49161, 2.1861543327406885, 4.286112277201366, 156, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("49223, 2.1767210292213415, 4.1178502021639805, 153, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("49331, 2.167084034428142, 3.9570603091832086, 157, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("49464, 2.1574548877594406, 3.804554384294576, 159, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("49571, 2.1479921129619237, 3.660803200889859, 150, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("49638, 2.1388118299810874, 3.5260139266548234, 168, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("49692, 2.1299964205791087, 3.4001925722009663, 173, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("49857, 2.121601589851306, 3.283194180157224, 170, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("49939, 2.113662106974599, 3.174762983121797, 161, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("50019, 2.1061964597008256, 3.0745643689598636, 163, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("50096, 2.099210616583815, 2.982210168855749, 160, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("50258, 2.0927010573029934, 2.897278515963401, 150, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("50374, 2.086657203553994, 2.819329301028083, 166, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("50431, 2.081063359849678, 2.747916068142793, 156, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("50511, 2.0759002544072795, 2.6825950423431126, 164, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("50681, 2.071146254418693, 2.622931855620967, 163, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("50734, 2.066778316853608, 2.5685064346523414, 173, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("50812, 2.0716116139544565, 2.523345595015, 171, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("50889, 2.083265282103074, 2.485885872036744, 158, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("50994, 2.0910011118461527, 2.4503971486024394, 167, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("51082, 2.095669632289359, 2.4169781535500037, 160, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("51142, 2.106798750972497, 2.39009760470151, 158, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("52179, 2.1225878330241974, 2.3685688996534484, 189, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("53239, 2.141608736234792, 2.3514128892065114, 217, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("53319, 2.1627364902610466, 2.3378223332178667, 217, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("53404, 2.185092253165276, 2.3271323773394186, 223, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("53529, 2.207996426679582, 2.3187960374749257, 217, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("53647, 2.2220912830373187, 2.307934681951974, 227, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("53730, 2.2293442372549994, 2.295360587277227, 226, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("53944, 2.2401837711862322, 2.28614067314988, 211, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("54049, 2.253532581003775, 2.2795900596685383, 207, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("54124, 2.268540867572425, 2.2751505985630565, 204, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("54239, 2.284543623594927, 2.272368492757894, 205, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("54330, 2.301025519107444, 2.2708757718580572, 200, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("54485, 2.3175920709789297, 2.2703749683188788, 212, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("54542, 2.333946005921784, 2.270626449525847, 225, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("54601, 2.349867912585784, 2.2714379529389754, 229, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("54692, 2.3652004329295764, 2.272655947949839, 232, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("54781, 2.3798353715274834, 2.274158511742156, 251, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("54920, 2.3937032081726777, 2.2758494593906913, 249, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("54998, 2.4067645877425843, 2.277653512470581, 261, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("55085, 2.419003434846527, 2.2795123270726556, 259, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("55197, 2.4304214018159307, 2.2813812325730005, 264, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("55291, 2.44103340923494, 2.2832265578220423, 255, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("55373, 2.442025191311948, 2.280594275795123, 268, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("55543, 2.4357852746811717, 2.2746476596783483, 267, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("55607, 2.42422558263007, 2.2663144032627947, 261, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("55804, 2.408868720547909, 2.256330054230083, 255, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("55873, 2.3909199286438234, 2.2452737915517145, 257, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("56001, 2.371326335905029, 2.233597864683743, 253, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("56075, 2.3508257024191828, 2.2216517884547016, 260, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("57053, 2.3299864655978118, 2.2097022014753422, 254, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("57117, 2.3092405968884866, 2.1979491412162337, 252, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("57240, 2.2889105187416847, 2.186539360334965, 247, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("58291, 2.2692311181525553, 2.1755772020017643, 252, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("58308, 2.2503677157414987, 2.165133463218501, 253, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("58417, 2.2324307020041814, 2.155252601410745, 260, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("58502, 2.2154874300011236, 2.1459585783647492, 260, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("58608, 2.1995718521654783, 2.1372595847741733, 256, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("58686, 2.1846923045859863, 2.1291518465016717, 258, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("58818, 2.1708377721582677, 2.1216226786889343, 260, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("58884, 2.157982909968174, 2.1146529248512325, 264, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("58983, 2.146092048159468, 2.1082188940587687, 262, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("59103, 2.135122367666947, 2.102293889396775, 273, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("59184, 2.125026401169774, 2.0968494044103334, 284, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("59405, 2.1157539862773977, 2.0918560505969968, 306, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("59463, 2.0870541011989836, 2.0759221800549383, 312, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("59537, 2.0442619448990147, 2.0520484864753734, 326, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("59677, 1.991613787787879, 2.0226178205106713, 339, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("59767, 1.932451099353554, 1.9895099363565278, 343, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("59859, 1.8693885572340756, 1.9541959366295392, 353, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("59982, 1.80445216721939, 1.9178159168850328, 356, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("60080, 1.7391926644502107, 1.881242715789797, 7, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("60169, 1.6747784854946473, 1.8451341820331013, 11, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("60284, 1.6120718690485552, 1.8099759576762553, 22, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("60376, 1.551691034758668, 1.7761164357752444, 23, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("60481, 1.4940608843240997, 1.7437952660811264, 27, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("60561, 1.439454249288263, 1.713166546703747, 28, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("60642, 1.3880253613854057, 1.6843176437202108, 29, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("60716, 1.3398369319639973, 1.657284418081783, 30, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("60942, 1.2948819868940324, 1.6320635042128266, 30, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("61031, 1.2531014041721495, 1.6086221727354566, 39, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("61139, 1.2421726103945347, 1.6021724712485048, 41, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("62163, 1.2582286937769704, 1.595926765056927, 61, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("63201, 1.2942558336397365, 1.5899381504400727, 50, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("63306, 1.3446608135317044, 1.584242164747875, 46, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("63491, 1.405009090447374, 1.5788605838440402, 45, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("63559, 1.4718090355155589, 1.573804499546541, 42, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("63658, 1.5423343992803271, 1.5690768057754187, 38, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("63730, 1.61447840433812, 1.5646741997952645, 42, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("63816, 1.686633990281385, 1.5605887864246721, 34, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("63941, 1.7575956687896994, 1.5568093577340945, 29, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("64028, 1.826479222079996, 1.553322408031704, 32, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("64092, 1.8926561222352738, 1.5501129333988273, 36, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("64166, 1.9557000842033667, 1.547165056311967, 39, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("64252, 2.0153436098316027, 1.5444625086694002, 39, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("64407, 2.071442749444475, 1.5419890005707628, 35, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("64463, 2.1239486138856103, 1.5397284972650498, 41, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("64615, 2.1917622019850347, 1.5399376448313475, 40, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("64712, 2.2699263563598526, 1.5419955447588818, 42, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("64831, 2.3332729217571906, 1.5563057106252178, 38, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("64943, 2.3844505193314633, 1.579566573181216, 41, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("65061, 2.4256484310788657, 1.6091527772118612, 40, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("65114, 2.4586749465936584, 1.6429898839479806, 38, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("65291, 2.4850224730087476, 1.6794512159342705, 35, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("65355, 2.5059216321588074, 1.71727302799175, 35, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("65472, 2.513547307652039, 1.751055669901664, 35, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("65674, 2.5199296646999843, 1.7856750568357382, 45, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("65725, 2.525273160296499, 1.8203450127556984, 44, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("65844, 2.5297484515444992, 1.8544750628164068, 34, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("65957, 2.5334980013436463, 1.887631349314998, 33, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("66083, 2.5366407525595016, 1.919504708367025, 27, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("66148, 2.539276025650957, 1.9498846491832613, 28, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("66271, 2.5414867689309215, 1.9786381939757176, 25, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("66496, 2.543342269130935, 2.005692716003798, 33, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("66580, 2.5449004120176517, 2.031022062258128, 30, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("67582, 2.546209567871284, 2.05463537091124, 29, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("68311, 2.5473101641858755, 2.07656809621922, 25, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("68431, 2.548235997574277, 2.0968748385965514, 22, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("68514, 2.549015328211562, 2.1156236480696076, 20, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("68686, 2.5496717929415555, 2.132891527706573, 12, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("68776, 2.550225167162247, 2.148760911972521, 16, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("68842, 2.5458281117083947, 2.182550268305773, 14, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("68952, 2.5377915182561988, 2.2292164477506162, 3, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("69049, 2.5271592145294486, 2.284762474871738, 10, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("69166, 2.553723433824184, 2.3380170076060094, 17, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("69257, 2.6077423083967872, 2.388665344004572, 12, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("69382, 2.681451859851582, 2.4365148942113763, 16, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("69473, 2.7687009264437736, 2.481467862270916, 10, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("69621, 2.864650478279143, 2.523499177585681, 359, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("69682, 2.9655262348496394, 2.5626387314199524, 343, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("69835, 3.068415382361973, 2.5989571382466155, 345, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("69913, 3.171099753955479, 2.632554378037128, 328, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("70001, 3.271919137416639, 2.6635507885540197, 331, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("70113, 3.3696594567208535, 2.6920799702737472, 318, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("70225, 3.4634614725856223, 2.7182832440336058, 324, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("70336, 3.552746393939727, 2.742305365594232, 316, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("70467, 3.6371554123882652, 2.764291254312757, 316, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("70549, 3.7165006866834864, 2.7843835369210637, 320, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("70637, 3.790725731625466, 2.8027207435699015, 300, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("70749, 3.859873520470148, 2.8194360231383238, 333, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("70820, 3.924060904094446, 2.8346562694036277, 324, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("70880, 3.9834581940763836, 2.8485015699205043, 320, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("70996, 4.0382729589935185, 2.8610849061174455, 333, Need to implement (1), Need to implement (2), Need to implement (3)");
		datapoints.Add("71133, 4.0887372506967665, 2.8725120468044865, 334, Need to implement (1), Need to implement (2), Need to implement (3)");



		DataBuilder.BuildData(datapoints);
	}

	public Location ParseLocation(string path)
	{
		/// <summary>
		///	Parses the location to create a room
		/// +path -> path of the file
		/// </summary>

		locationFileContents = System.IO.File.ReadAllText(path);

		string[] data = locationFileContents.Split('\n');

		List<Beacon> beacons = new List<Beacon>();

		List<Wall> walls = new List<Wall>();

		string locationName = data[0].Split(',')[0];

		string locationID = data[0].Split(',')[1];

		float width = float.Parse(data[1].Split(',')[2]);

		float height = float.Parse(data[1].Split(',')[3]);

		int orientation = int.Parse((data[2].Split(',')[0]));

		int endIndex = 0;

		for (int i = 3; i < data.Length; i++)
		{
			if (data[i].Contains("END")) {

				endIndex = i;

				break;
			}

			string[] beaconInfo = data[i].Split(',');

			Beacon beacon = new Beacon(beaconInfo[0], float.Parse(beaconInfo[1]), float.Parse(beaconInfo[2]), int.Parse(beaconInfo[3]));

			beacons.Add(beacon);

		}

		for (int i = endIndex + 1; i < data.Length; i++)
		{
			float x = float.Parse(data[i].Split(',')[0]);

			float y = float.Parse(data[i].Split(',')[1]);

			Wall wall = new Wall(x, y);

			walls.Add(wall);
		}



		Location location = new Location(locationName, locationID, width, height, orientation, beacons, walls);

		return location;

	}


	public List<Session> ParseDatapoints(string path)
	{
		/// <summary>
		/// Reads datapoints and creates session. Each session is extracted between "END" keyword found in the file
		/// x,y,z,0,1,2,3		--> represents one session
		/// x,y,z,0,1,2,3
		///	END
		/// x,y,z,0,1,2,3		--> represents second session
		/// x,y,z,0,1,2,3
		/// </summary>
		beaconFileContents = System.IO.File.ReadAllText(path);

		Session s = new Session();

		string[] sessionData = beaconFileContents.Split(new string[] {"END"}, System.StringSplitOptions.None);

		List<Session> sessions = new List<Session>();

		for (int i = 0; i < sessionData.Length; i++)
		{
			string[] currentSessionData = sessionData[i].Split('\n');

			Session session;

			List<KSUHeatmap.DataPoint> points = new List<KSUHeatmap.DataPoint>();

			for (int j = 0 ; j < currentSessionData.Length; j++)
			{
				string[] contents = currentSessionData[j].Split(',');

				if (contents.Length <= 1) { continue; }

				string localTime = contents[0];

				float x = float.Parse(contents[1]);

				float y = float.Parse(contents[2]);

				int orientation = int.Parse(contents[3]);

				List<string> closestID = new List<string>();

				for (int k = 4; k < contents.Length; k++)
				{
					closestID.Add(contents[k]);
				}

				KSUHeatmap.DataPoint dp = new KSUHeatmap.DataPoint(localTime, new Vector2(x, y), orientation, closestID);

				points.Add(dp);

			}

			session = new Session(points);

			sessions.Add(session);

		}
		return sessions;
	}


	public string GetFileContents(string path)
	{
		string text = "";
		StreamReader reader = new StreamReader(path);

		while (!reader.EndOfStream)
		{
			text += reader.ReadLine() + "\n";
		}

		reader.Close();

		return text;
	}


}
