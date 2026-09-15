using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class FadeTransicao : MonoBehaviour
{
    // Elementos usados na transição entre as cenas.
    public GameObject fade;
    public VideoPlayer video;
    public string nomeDaCena;

    void Start()
    {
        // O fade começa escondido.
        fade.SetActive(false);

        // Quando o vídeo terminar, chama a função que troca de cena.
        video.loopPointReached += VideoTerminou;
    }

    public void IniciarFade()
    {
        // Mostra o fade e inicia o vídeo da transição.
        fade.SetActive(true);
        video.Play();
    }

    void VideoTerminou(VideoPlayer vp)
    {
        // Carrega a cena configurada no Inspector.
        SceneManager.LoadScene(nomeDaCena);
    }

    void OnDestroy()
    {
        // Remove o evento quando este objeto for destruído.
        if (video != null)
        {
            video.loopPointReached -= VideoTerminou;
        }
    }
}