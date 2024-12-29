using System.IO;//외부 파일 가져오기 json
using UnityEngine;
//저장하는 방법
//1.저장할 데이터가 존재
//2.데이터를 제이슨으로 변환
//3. 제이슨을 외부애 저장

//불러오는 방법
//1. 외부에 자장된 제이슨을 가져옴
//2. 제이슨을 데디터 형태로 변환
//3. 불러온 데이터를 사용


//저장방식
//중앙저장 방식이 더 좋으나 개별 저장방식을 사용함(이유: 공부 하면서 하고 있어 이해하면서 하고 싶어서) 
public class PlayerData
{
    
    //이름,플레이어 정보,퀘스트 진행사항  
    #region 플레이어 정보
    public string name;
    public Vector3 plyerPosition;
    public float hpData;
    public float hpMaxData;
    public float staminaData;
    public float staminaMaxData;
    public int hpPositionData;
    public int staminaPositionData;
    public int skillPointData;
    public int useSkillPointData;
    public int skillTree;
    public int[] skillLevelsData = new int[14];
    #endregion
    #region 저장 UI관련
    public int savePoint;
    public string saveTime;
    #endregion
    #region 퀘스트 진행사항
    public int questProgressData;
    #endregion
    #region 가방 아이템
    public int[] itemCountData = new int[91];  // 슬롯 3개, 각각 91개의 아이템을 저장할 수 있음
    public string[] itemnameData = new string[91];
    public int[] skillEnhanceCount = new int[6];
    #endregion
    #region 저주
    public float curseProgressionData;
    #endregion
    #region 소리
    public float backgroundVolume;
    public float playerVolume;
    #endregion
    #region 튜토리얼
    public bool movetip;
    public bool startmenu;
    #endregion
}
public class DataManager : MonoBehaviour
{
    //싱글톤
    public static DataManager instance;
    public PlayerData nowPlayer = new PlayerData();

    public string path;
    public int nowSlot; // 현재 슬롯번호
    private void Awake()
    {
        #region 싱글톤
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(gameObject);
        #endregion
        path = Application.persistentDataPath + "/save";
    }
    public void SaveData()
    {
        //파일 제작  
        string data = JsonUtility.ToJson(nowPlayer); //파일 만들기
        File.WriteAllText(path + nowSlot.ToString(), data);
        Debug.Log("Data saved: " + data); // 저장된 데이터 확인
    }
    public void LoadData()
    {
        // 파일 경로
        string filePath = path + nowSlot.ToString();

        // 파일 존재 여부 확인
        if (File.Exists(filePath))
        {
            // 파일이 존재하면 데이터를 로드
            string data = File.ReadAllText(filePath);
            Debug.Log(nowSlot.ToString() + ": Data 로드 후 확인: " + data); // 저장된 데이터 확인
            nowPlayer = JsonUtility.FromJson<PlayerData>(data);
        }
        else
        {
            // 파일이 없으면 경고 메시지 출력
            Debug.Log($"플레이러가 저장한적이 없음: 저장된 데이터 파일이 존재하지 않습니다. 경로: {filePath}");
            nowPlayer = new PlayerData(); // 데이터 초기화
        }
    }
    public void DataClear()
    {
        //nowSlot = -1;
        nowPlayer = new PlayerData(); //초기값
    }
}
