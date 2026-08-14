using UnityEngine;

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

public void Fechar() // metodo para o botão de sair
{
   Application.Quit();
}

}