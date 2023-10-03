using UnityEngine;

namespace Quiz.ScriptableObjects
{
    [CreateAssetMenu(fileName = "QuestionScriptableObject", menuName = "ScriptableObjects/Questions")]
    public class QuestionScriptableObject : ScriptableObject
    {
        #region VARIABLES
        // Definição das questões
        [TextArea]
        public string question; // Pergunta
        [TextArea]
        public string category; // Categoria
        [TextArea]
        public string[] answers; // Quantidade de Respostas
        public int answerRightIndex; // Repostas
        [TextArea(1, 100)]
        public string questionRightDescription; // Explicação para a resposta correta

        #endregion
    }
}