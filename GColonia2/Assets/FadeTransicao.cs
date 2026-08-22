using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class FadeTransicao : MonoBehaviour
{
    public GameObject fade;
    public VideoPlayer video;

    public string nomeDaCena;

    void Start()
    {
        fade.SetActive(false);

        video.loopPointReached += VideoTerminou;
    }

    public void IniciarFade()
    {
        fade.SetActive(true);
        video.Play();
    }

    void VideoTerminou(VideoPlayer vp)
    {
        SceneManager.LoadScene(nomeDaCena);
    }

    void OnDestroy()
    {
        video.loopPointReached -= VideoTerminou;
    }
}