using Quiz.ScriptableObjects;
using Quiz.Prefab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Quiz.Manager
{
    public class GameManager : MonoBehaviour
    {
        #region VARIABLES

        private List<int> questionsSelected = new(); // Questões Selecionadas
        private const string POINTS_PLAYERPREFS_NAME = "POINTS_PLAYERPREFS_NAME"; // Pontos
        private int counterQuestion; // Contador de Questões

        [Space()]
        [Header("Parameters")]
        [SerializeField] private int limitMaxQuestions = 5; // Limite de questões do jogo

        [Space()]
        [Header("Questions")]
        [SerializeField] private QuestionScriptableObject[] questionScriptableObjects; // Scripts das questões como objeto
        
        [Space()]
        [Header("UserGeneral")]
        [SerializeField] private List<GameObject> screens; // Canvas
        [SerializeField] private AnswerPrefab answerPrefab; // PreFab das questões que serão mostradas
        [SerializeField] private Button easyGameButton; // Botão Start easy
        [SerializeField] private Button midGameButton; // Botão Start medium
        [SerializeField] private Button hardGameButton; // Botão Start hard
        [SerializeField] private Button nextQuestionButton; // Botão para prox questão
        
        [Space()]
        [Header("QuestionScreen")] // TELA DAS QUESTÕES
        [SerializeField] private TMP_Text questionIdText;
        [SerializeField] private TMP_Text questionDescription;
        [SerializeField] private Transform rootAnswers;
        
        [Space()]
        [Header("AfterScreen")] // TELA DE EXPLICAÇÃO
        [SerializeField] private TMP_Text afterQuestionIdText;
        [SerializeField] private TMP_Text afterQuestionDescription;
        [SerializeField] private TMP_Text afterQuestionDescriptionRigth;
        
        [Space()]
        [Header("ResultScreen")] // TELA DO RESULTADO
        [SerializeField] private TMP_Text countAnswersRigth; // Quantidade de pontos atingido
        [SerializeField] private Button homeButton; // Botão para home

        #endregion

        private void OnEnable()
        {
            AnswerPrefab.OnAnswerClick += OnAnswerClick_Handler;
            nextQuestionButton.onClick.AddListener(() => NextQuestionButton_Handler(false));

            // Buttons Handlers
            easyGameButton.onClick.AddListener(EasyGameButton_Handler);
            midGameButton.onClick.AddListener(MidGameButton_Handler);
            hardGameButton.onClick.AddListener(HardGameButton_Handler);
            homeButton.onClick.AddListener(HomeButton_Handler);
        }

        private void OnDisable()
        {
            AnswerPrefab.OnAnswerClick -= OnAnswerClick_Handler;
        }

        private void GetQuestions(int startQ, int limitQ)
        {
            questionsSelected.Clear(); // Limpa as questões
            Debug.Log("startQ = " + startQ);
            Debug.Log("limitQ = " + limitQ);
            Debug.Log("Tamanho das questões = " + questionScriptableObjects.Length);

            for (int index = startQ; index <= limitQ; index++)
            {   // Gera as questões dentro o limite max de questões e de forma aleatória dentro do range total de questões
                int newQuestion = Random.Range(startQ, limitQ);

                if (!questionsSelected.Contains(newQuestion)) // Caso ela não tenha sido selecionada anteriormente
                {
                    questionsSelected.Add(newQuestion);
                    Debug.Log("Questão escolhida = " + newQuestion);
                }
            }

            if(questionsSelected.Count < limitMaxQuestions) // Chama a propria função enquanto a quantidade de questions for menor que a max
            {
                GetQuestions(startQ, limitQ);
            }
        }

        private void AfterScreen() // Apenas define as informações para mostrar na tela
        {
            int questionCurrentIndex = questionsSelected[counterQuestion];

            afterQuestionIdText.text = $"{GetCurrentQuestion()}";
            afterQuestionDescription.text = questionScriptableObjects[questionCurrentIndex].question;
            afterQuestionDescriptionRigth.text = questionScriptableObjects[questionCurrentIndex].questionRightDescription;
            EnableScreen("AfterQuestion_Panel");
        }

        private bool VerifyIsLastQuestion() // Verifica se é a ultima questão, se sim leva o usuário para a tela final com o resultado
        {
            if(GetCurrentQuestion() == limitMaxQuestions)
            {
                countAnswersRigth.text = PlayerPrefs.GetInt(POINTS_PLAYERPREFS_NAME).ToString();
                EnableScreen("Result_Panel");
                return true;
            }
            return false;
        }

        private void EnableScreen(string screenName) // Função para esconder e mostrar Canvas(Scenas) na Unity
        {
            screens.ForEach(screen => screen.SetActive(false));

            GameObject screenForEnable = screens.Find(screen => screen.name == screenName);
            screenForEnable.SetActive(true);
        }

        private void SetQuestion()
        {
            int questionCurrentIndex = questionsSelected[counterQuestion];
            QuestionScriptableObject questionCurrent = questionScriptableObjects[questionCurrentIndex];

            // Limpa as perguntas anteriores
            for (int index = 0; index < rootAnswers.transform.childCount; index++)
            {
                Transform children = rootAnswers.transform.GetChild(index);
                Destroy(children.gameObject);
            }

            // Manda as informações para a UI
            questionIdText.text = $"{GetCurrentQuestion()}";
            questionDescription.text = questionCurrent.question;


            // Add as respostas
            for (int index = 0; index < questionCurrent.answers.Length; index++)
            {
                AnswerPrefab answerPrefabCreated = Instantiate(answerPrefab, rootAnswers);
                answerPrefabCreated.SetText(questionCurrent.answers[index]);

                if(index == questionCurrent.answerRightIndex)
                {
                    answerPrefabCreated.SetRigth(true);
                }
            }
        }

        private int GetCurrentQuestion()
        {
            return counterQuestion + 1;
        }

        // Botão das questões ->
        private void OnAnswerClick_Handler(bool isRigth)
        {   
            // Se for a alternativa correta
            if (isRigth)
            {
                int pointsCurrent = PlayerPrefs.GetInt(POINTS_PLAYERPREFS_NAME); // Pontos ++
                int newPoints = pointsCurrent + 1;
                PlayerPrefs.SetInt(POINTS_PLAYERPREFS_NAME, newPoints);
                NextQuestionButton_Handler(true);
            }
            else // Caso não, vai para a tela de explicação da alternativa
            {
                AfterScreen();
            }

            Debug.Log($"Reposta = {isRigth} | Pontos = {PlayerPrefs.GetInt(POINTS_PLAYERPREFS_NAME)}"); // Console log -> vdd / pontos
        }

        private void NextQuestionButton_Handler(bool isAuto) // Botão de prox questão
        {
            if (VerifyIsLastQuestion()) // Verifica a ultima questão
            {
                return;
            }

            counterQuestion++; // Add ao contador e add a prox pergunta indo a tela de questões novamente
            SetQuestion();

            if (!isAuto)
            {
                EnableScreen("Question_Panel");
            }
        }

        // Botão inicial -> Easy Handler
        private void EasyGameButton_Handler()
        {
            // Seta os valores de questões e pontos em 0 e define as questões
            counterQuestion = 0;
            GetQuestions(40,59);
            PlayerPrefs.SetInt(POINTS_PLAYERPREFS_NAME, 0);
            EnableScreen("Question_Panel");
            SetQuestion();
        }

        // Botão inicial -> Medium Handler
        private void MidGameButton_Handler()
        {
            // Seta os valores de questões e pontos em 0 e define as questões
            counterQuestion = 0;
            GetQuestions(20,39);
            PlayerPrefs.SetInt(POINTS_PLAYERPREFS_NAME, 0);
            EnableScreen("Question_Panel");
            SetQuestion();
        }

        // Botão inicial -> Hard Handler
        private void HardGameButton_Handler()
        {
            // Seta os valores de questões e pontos em 0 e define as questões
            counterQuestion = 0;
            GetQuestions(0,19);
            PlayerPrefs.SetInt(POINTS_PLAYERPREFS_NAME, 0);
            EnableScreen("Question_Panel");
            SetQuestion();
        }

        // Botão para voltar a tela inicial
        private void HomeButton_Handler()
        {
            EnableScreen("Menu_Panel");
        }
    }
}