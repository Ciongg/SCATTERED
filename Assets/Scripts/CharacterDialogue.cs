using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CharacterDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public GameObject characterPanel; 
    public Image characterImage;  


    public Sprite idleSprite;
    public Sprite [] smilesSprites;         
    public Sprite smileSprite;           
    public Sprite smile1Sprite;           
    public Sprite smile2Sprite;           
    public Sprite frownSprite;             
    public Sprite assuranceSprite;             
    public Sprite talkSprite;
    [HideInInspector]

    public string [] trashFallText = {"Couldn't catch that one!",
                                       "Oops!",
                                       "Aww! couldn't grab that one."};
    [HideInInspector]
    public string [] wrongAnswersText = {"Oops! Almost had it!",
                                          "Nice effort, but not quite right!",
                                          "So close! Try again!",
                                          "Hmm, that doesn't seem to fit here!",
                                          "You're learning! Try again!"};
    [HideInInspector]
    public string [] rightAnswersText = {"Great job! You got it!",
                                          "Spot on!",
                                          "Well done!",
                                          "Awesome! You're a pro!",
                                          "You're crushing it! Keep sorting!",
                                          "Lets save the world!"};
    [HideInInspector]
    public string [] leafCollectText = {"This leaf looks pretty!",
                                        "I should collect more!",
                                        "These leaves will help us grow more plants!",
                                        "Keep collecting to enhance our garden!",
                                        "The more leaves, the better!"};
    [HideInInspector]
    public string [] healthSpecialText = {"I feel energized!",
                                          "This should help!",
                                          };
    [HideInInspector]
    public string [] healthFullSpecialText = {"no need I am energized already!",
                                          "I'm as healthy as can be!",
                                          "I'm feeling great already!",
                                          };
    [HideInInspector]
    public string [] timeSpecialText = {"sorting seems easiuer now!",
                                          "This should help!",
                                          
                                          };
    [HideInInspector]
     public string [] smogSpecialText = {"Here it comes, the harmful smog",
                                          "Smog does not help anyone!",
                                          "Smog clouds our world!",
                                          "Pollution fuels this smog!"
                                          
                                          };


                                
     void Start()
    {
        dialoguePanel.SetActive(false); 
         characterImage.sprite = idleSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

      public void ShowDialogue(string message, Sprite characterSprite = null, float duration = 2f)
    {
        characterImage.sprite = characterSprite; 

        
        float chance = Random.Range(0f, 1f); 
        if (chance <= 0.9f) // % chance
        {
            dialogueText.text = message; 
            dialoguePanel.SetActive(true); 
            CancelInvoke(); 
            Invoke("HideDialogue", duration); 
        }
        else
        {
           
            CancelInvoke();
            Invoke("HideDialogue", duration);
        }



    }


     public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        characterImage.sprite = idleSprite;
    }

      public void ShowSmileDialogue(string message, float duration = 2f)
    {
        int smileIndex = Random.Range(0, smilesSprites.Length);
        ShowDialogue(message, smilesSprites[smileIndex], duration);
        
    }

    public void ShowAssuranceDialogue(string message, float duration = 2f)
    {
        ShowDialogue(message, assuranceSprite, duration);
    }

    public void ShowTalkDialogue(string message, float duration = 2f)
    {
        ShowDialogue(message, talkSprite, duration);

    
    }

    public void ShowHealthDialogue(float duration = 2f){

        int dialogueToShowIndex = Random.Range(0, healthSpecialText.Length);
        
        string dialogueToShow = healthSpecialText[dialogueToShowIndex];

         int smileIndex = Random.Range(0, smilesSprites.Length);
        ShowDialogue(dialogueToShow, smilesSprites[smileIndex], duration);
        
    }

    public void ShowTrashFallDialogue(float duration = 2f){

        int dialogueToShowIndex = Random.Range(0, trashFallText.Length);
        
        string dialogueToShow = trashFallText[dialogueToShowIndex];

      
        ShowDialogue(dialogueToShow, frownSprite, duration);
        
    }


    public void ShowHealthFullDialogue(float duration = 2f){

        int dialogueToShowIndex = Random.Range(0, healthFullSpecialText.Length);
        
        string dialogueToShow = healthFullSpecialText[dialogueToShowIndex];

        int smileIndex = Random.Range(0, smilesSprites.Length);
        ShowDialogue(dialogueToShow, assuranceSprite, duration);
        
    }



    public void ShowTimeDialogue(float duration = 2f){
         int dialogueToShowIndex = Random.Range(0, timeSpecialText.Length);
        
        string dialogueToShow = timeSpecialText[dialogueToShowIndex];

        ShowDialogue(dialogueToShow, talkSprite, duration);
        
    }

    public void ShowSmogDialogue(float duration = 2f){
         int dialogueToShowIndex = Random.Range(0, smogSpecialText.Length);
        
        string dialogueToShow = smogSpecialText[dialogueToShowIndex];

        ShowDialogue(dialogueToShow, frownSprite, duration);
       

        
    }
}
