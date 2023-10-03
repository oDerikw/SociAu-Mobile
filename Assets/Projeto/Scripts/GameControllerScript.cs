using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    #region VARIABLES

    [Space()]
    [Header("UserInterfaceGeneral")]
    [SerializeField] private MainImageScript startObject; // Objeto de inicio
    [SerializeField] private List<GameObject> screens; // Canvas
    [SerializeField] private Sprite[] images; // Imagens

    [Space()]
    [Header("Parameters")]
    [SerializeField] private int x; // Número de valores diferentes a serem gerados (repetidos duas vezes)
    [SerializeField] private int rows; // Linhas
    [SerializeField] private int columns; // Colunas

    [Space()]
    [Header("ImageParameters")]
    [SerializeField] private float width; // Largura da imagem
    [SerializeField] private float height; // Altura da imagem
    [SerializeField] private float Xspace; // Xspace
    [SerializeField] private float Yspace; // Yspace

    [Space()]
    [Header("TextParameters")]
    [SerializeField] private TMP_Text scoreText; // Pontos
    [SerializeField] private TMP_Text attemptsText; // Tentativas

    private MainImageScript firstOpen;
    private MainImageScript secondOpen;

    private int score = 0;
    private int attempts = 0;

    #endregion

    static int[] GenerateImageRandomArray(int x) // Gera o Array de forma aleatoria com as imagens fornecidas
    {
        int[] locations = new int[x * 2]; // O dobro de x para garantir que cada valor seja repetido duas vezes
        List<int> uniqueNumbers = new List<int>();

        while (uniqueNumbers.Count < x)
        {
            int randomNumber = Random.Range(0, 19); // Gera um número aleatório entre 0 e 19 (num de imagens)

            // Verifica se o número gerado já existe na lista de números únicos
            if (!uniqueNumbers.Contains(randomNumber))
            {
                uniqueNumbers.Add(randomNumber);
            }
        }

        // Adiciona os valores únicos ao array, repetindo cada um duas vezes
        for (int i = 0; i < x; i++)
        {
            locations[i] = uniqueNumbers[i];
            locations[i + x] = uniqueNumbers[i];
        }

        return locations;
    }

    private void Start() // Main - Start
    {
        int[] locations = GenerateImageRandomArray(x); // Pega as imagens de forma aleatoria

        Debug.Log("Locations = " );
        foreach (int location in locations)
        {
            Debug.Log(location + " ");
        }

        Vector3 startPosition = startObject.transform.position;

        // Embaralha o array de forma aleatória
        System.Random random = new System.Random();
        for (int i = x * 2 - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            int temp = locations[i];
            locations[i] = locations[j];
            locations[j] = temp;
        }

        // Coloca as imagens nas posições
        for(int i = 0; i < columns; i++){
            for(int j = 0; j < rows; j++){
                MainImageScript gameImage;
                
                if(i == 0 && j == 0){
                    gameImage = startObject;
                }else{
                    gameImage = Instantiate(startObject) as MainImageScript;
                }

                int index = j * columns + i;
                int id = locations[index];
                gameImage.ChangeSprite(id, images[id]);

                float positionX = (Xspace * i) + startPosition.x;
                float positionY = (Yspace * j) + startPosition.y;

                gameImage.transform.position = new Vector3(positionX, positionY, startPosition.z);

                // Altera o tamanho da sprite para novas largura e altura aleatórias
                gameImage.ChangeSize(width, height);
            }
        }
    }

    public bool canOpen{ // Busca se a imagem pode ser aberta
        get {return secondOpen == null; }
    }

    public void imageOpened(MainImageScript startObject){ // Base das imagens para o startObject
        if(firstOpen == null){
            firstOpen = startObject;
        }
        else{
            secondOpen = startObject;
            StartCoroutine(CheckGuessed());
        }
    }

    private IEnumerator CheckGuessed(){ // Verifica a jogada, atribui os pontos e tentativas
        if(firstOpen.spriteId == secondOpen.spriteId){
            score++;
            score++;
            scoreText.text = score + " Pontos!";
            Debug.Log("Pontos: " + score);
        }
        else{
            yield return new WaitForSeconds(0.5f);

            firstOpen.Close();
            secondOpen.Close();
        }
        attempts++;
        attemptsText.text = "Em " + attempts + " Tentativas!";
        Debug.Log("Tentativas: " + attempts);

        firstOpen = null;
        secondOpen = null;

        VerifyIsLast();
    }

    private void VerifyIsLast() // Verifica se é a ultima questão, se sim leva o usuário para a tela final com o resultado
    {
        if(score == columns * rows)
        {
            var clones = GameObject.FindGameObjectsWithTag ("clone");
            foreach (var clone in clones){
                Destroy(clone);
            }
            EnableScreen("Result_Panel");
        }
    }
    

    private void EnableScreen(string screenName) // Função para esconder e mostrar Canvas(Scenas) na Unity
    {
        screens.ForEach(screen => screen.SetActive(false));

        GameObject screenForEnable = screens.Find(screen => screen.name == screenName);
        screenForEnable.SetActive(true);
    }

    public void Restart(){ // Restart
        SceneManager.LoadScene("MainScene");
    }
}
