using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] Animator transitonAnim;

    public Text txtCountCherrysText;
    private int Cherry;

    public Text txtCountGemsText;
    private int Gem;
    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        StartCoroutine(LoadLevel());
        Cherry = 0;
        Gem = 0;
    }
    IEnumerator LoadLevel()
    {
        transitonAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        transitonAnim.SetTrigger("Start");
    }
    public void AddCherry()
    {
        Cherry++;
        txtCountCherrysText.text = Cherry.ToString();
    }
    public void AddGem()
    {
        Gem++;
        txtCountGemsText.text = Gem.ToString();
    }
}
