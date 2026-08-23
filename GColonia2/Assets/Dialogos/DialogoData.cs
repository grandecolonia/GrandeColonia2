using UnityEngine;

[CreateAssetMenu(fileName = "NovoDialogo", menuName = "Dialogos/Dialogo")]
public class DialogoData : ScriptableObject
{
    [System.Serializable]
    public class Fala
    {
        public string nomePersonagem;
        public Sprite rosto;

        [TextArea(3, 6)]
        public string texto;
    }

    public Fala[] falas;
}