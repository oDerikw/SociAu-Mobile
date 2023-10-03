using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CanvaSwitch : MonoBehaviour
{
    #region VARIABLES

    [Space()]
    [Header("UserInterfaceGeneral")] // Canvas
    [SerializeField] private List<GameObject> screens;

    [Space()]
    [Header("ButtonsInterfaceGeneral")] // Buttons
    [SerializeField] private Button backButton_Help;
    [SerializeField] private Button backButton_Memory;
    [SerializeField] private Button backButton_Material;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button helpButton;
    [SerializeField] private Button memoryButton;

    #endregion

    private void OnEnable()
    {
        // Back Buttons Handlers
        backButton_Help.onClick.AddListener(backButton_Handler);
        backButton_Memory.onClick.AddListener(backButton_Handler);

        // Exit - Help Buttons Handlers
        exitButton.onClick.AddListener(exitButton_Handler);
        helpButton.onClick.AddListener(helpButton_Handler);

        // Memory Buttons Handlers
        memoryButton.onClick.AddListener(memoryButton_Handler);
    }

    private void EnableScreen(string screenName) // Função para esconder e mostrar Canvas(Scenas) na Unity
    {
        screens.ForEach(screen => screen.SetActive(false));

        GameObject screenForEnable = screens.Find(screen => screen.name == screenName);
        screenForEnable.SetActive(true);
    }

    // Botão inicial -> Exit Handler
    private void exitButton_Handler()
    {
        //EnableScreen("Question_Panel");
    }

    // Botão inicial -> Back Handler
    private void backButton_Handler()
    {
        EnableScreen("Main_Panel");
    }

    // Botão inicial -> Help Handler
    private void helpButton_Handler()
    {
        EnableScreen("Help_Panel");
    }

    // Botão inicial -> Memory Game Handler
    private void memoryButton_Handler()
    {
        EnableScreen("Memory_Menu_Panel");
    }
}
