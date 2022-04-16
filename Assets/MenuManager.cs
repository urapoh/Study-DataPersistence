using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance { get; private set; }

    public string playerName;
    public string highscoreName;
    public int highscore;

    [SerializeField] Text nameText;
    [SerializeField] Text highscoreText;

    [System.Serializable]
    class HighScoreObject
    {
        public string highscoreName;
        public int highscore;

        public HighScoreObject(string name, int score)
        {
            highscoreName = name;
            highscore = score;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        } else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        ReadHighScore();
        if (highscore > 0)
        {
            highscoreText.text = "Best Score : " + MenuManager.Instance.highscoreName + " : " + MenuManager.Instance.highscore;
        } else
        {
            highscoreText.text = "";
        }
    }

    public void StartGame()
    {
        playerName = nameText.text;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();  
#endif

    }

    public void SetHighScore(int score){
        highscoreName = playerName;
        highscore = score;

        HighScoreObject obj = new HighScoreObject(highscoreName, highscore);

        string json = JsonUtility.ToJson(obj);

        File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
    }

    public void ReadHighScore()
    {
        string path = Application.persistentDataPath + "/highscore.json";

        if (File.Exists(Application.persistentDataPath + "/highscore.json"))
        {
            string json = File.ReadAllText(path);

            HighScoreObject obj = JsonUtility.FromJson<HighScoreObject>(json);

            highscore = obj.highscore;
            highscoreName = obj.highscoreName;
        }
    }
}
