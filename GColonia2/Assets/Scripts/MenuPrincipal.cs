using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private GameObject menuinicial; // variaveis para armazenar o grupo inteiro do menu, opções e o componente de som.
    [SerializeField] private GameObject opcoes;
    [SerializeField] private AudioSource som; 

public void AbrirOpcoes() // método para abrir opções de volume e cursor
{
   menuinicial.SetActive(false);
   opcoes.SetActive(true);
}

public void FecharOpcoes() // metodo para fechar as opções e retornar ao menu padrão
{
   menuinicial.SetActive(true);
   opcoes.SetActive(false);
}

public void ControlarSom(float value) // metodo que passa o valor que o usuário deslizar no slider
// de volume
{
    som.volume = value;
}

public void IniciarCutscene()
{
   SceneManager.LoadScene("05_Cutscene01");
}

public void Tutorial()
{
   SceneManager.LoadScene("04_Tutorial");
}

public void Creditos()
{
   SceneManager.LoadScene("03_Creditos");
}

public void Fechar() // metodo para o botão de sair
{
   Application.Quit();
}

}