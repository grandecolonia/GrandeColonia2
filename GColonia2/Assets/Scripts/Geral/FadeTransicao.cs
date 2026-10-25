
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class FadeTransicao : MonoBehaviour
{
    [Header("Transição")]

    // Objeto que aparece na tela quando a transição começa.
    public GameObject fade;

    // Vídeo que será reproduzido antes de trocar de cena.
    public VideoPlayer video;

    // Nome da cena que será carregada depois que o vídeo terminar.
    public string nomeDaCena;

    void Start()
    {
        // Começa com o fade escondido para não atrapalhar a cena.
        fade.SetActive(false);

        // Fica de olho no vídeo e chama VideoTerminou quando ele acabar.
        video.loopPointReached += VideoTerminou;
    }

    public void IniciarFade()
    {
        // Mostra o objeto de transição na tela.
        fade.SetActive(true);

        // Começa a reproduzir o vídeo da transição.
        video.Play();
    }

    void VideoTerminou(VideoPlayer vp)
    {
        // Terminou o vídeo, então carrega a cena configurada no Inspector.
        SceneManager.LoadScene(nomeDaCena);
    }

    void OnDestroy()
    {
        // Se o VideoPlayer ainda existe, remove o evento para não deixar ligações desnecessárias.
        if (video != null)
        {
            video.loopPointReached -= VideoTerminou;
        }
    }
}
