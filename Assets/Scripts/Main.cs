using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main : MonoBehaviour
{

    public string word = "    ";
    public bool wordValidated = false;
    public bool wordAllowed = false;
    public enum letterStates { Grey, Yellow, Green };
    public letterStates[] states = new letterStates[5];
    public GameObject[] letters = new GameObject[5];

    public string errorMessage = "";
    public TMP_Text errorText;

    public string[] allowedWords;
    public string[] possibleWords;
    public List<string> remainingWords;
    public List<string> remainingGuesses;


    // Start is called before the first frame update
    void Start()
    {
        errorMessage = "";
        allowedWords = WordList.allowedWords;
        possibleWords = WordList.possibleWords;
        remainingWords = new List<string>(possibleWords);
        remainingGuesses = new List<string>(allowedWords);
        for (int i = 0; i < states.Length; i++)
        {
            states[i] = letterStates.Grey;
            LetterColor(i, states[i]);
        }
    }

    public void Refresh()
    {
        Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (errorMessage != errorText.text)
        {
            errorText.text = errorMessage;
        }
    }

    public void ReadInput(string input)
    {
        word = input;
        wordValidated = false;
        wordAllowed = false;
    }

    public void AcceptWord()
    {
        ValidateWord();
        if (!wordValidated && word != "")
        {
            errorMessage = "Word not valid\nPlease remove any numbers\nAnd make the word 5 letters";
        }
        else
        {
            errorMessage = "";
        }
        wordAllowed = false;
        foreach (string word in allowedWords)
        {
            if (this.word != "" && word == this.word)
            {
                wordAllowed = true;
                break;
            }
        }
        if (!wordAllowed)
        {
            errorMessage = "Word not accepted by Wordle";
        }
    }

    public void ProcessWord()
    {
        AcceptWord();
        if (wordAllowed)
        {
            List<int> indexes = new List<int>();
            List<char> disallowedLetters = new List<char>();
            char[] greenLetters = new char[5];
            char[] yellowLetters = new char[5];
            char[] doubleLetters = { ' ', ' ' };
            char tripleLetter = ' ';
            char doubleYellow = ' ';
            for (int i = 0; i < states.Length; i++)
            {
                greenLetters[i] = ' ';
                yellowLetters[i] = ' ';
                if (states[i] == letterStates.Grey)
                {
                    disallowedLetters.Add(word[i]);
                }
                else if (states[i] == letterStates.Yellow)
                {
                    yellowLetters[i] = word[i];
                }
                else if (states[i] == letterStates.Green)
                {
                    greenLetters[i] = word[i];
                }
            }
            for (int i = 0; i < disallowedLetters.Count; i++)
            {
                for (int j = 0; j < disallowedLetters.Count; j++)
                {
                    if (i != j && disallowedLetters[i] == disallowedLetters[j])
                    {
                        indexes.Add(j);
                    }
                }
                indexes.Sort();
                indexes.Reverse();
                for (int j = 0; j < indexes.Count; j++)
                {
                    disallowedLetters.RemoveAt(indexes[j]);
                }
                indexes.Clear();
            }
            for (int i = 0; i < yellowLetters.Length; i++)
            {
                if (yellowLetters[i] != ' ')
                {
                    for (int j = 0; j < yellowLetters.Length; j++)
                    {
                        if (i != j && yellowLetters[i] == yellowLetters[j])
                        {
                            doubleYellow = yellowLetters[i];
                        }
                    }
                }
            }

            for (int i = 0; i < disallowedLetters.Count; i++)
            {
                int count = 1;
                for (int j = 0; j < word.Length; j++)
                {
                    if (disallowedLetters[i] == yellowLetters[j] || disallowedLetters[i] == greenLetters[j])
                    {
                        count++;
                    }
                }
                if (count == 2 && doubleLetters[0] == ' ')
                {
                    doubleLetters[0] = disallowedLetters[i];
                }
                else if (count == 2)
                {
                    doubleLetters[1] = disallowedLetters[i];
                }
                else if (count == 3)
                {
                    tripleLetter = disallowedLetters[i];
                }
            }
            Debug.Log(doubleLetters[0]);
            Debug.Log(doubleLetters[1]);
            Debug.Log(tripleLetter);
            Debug.Log(doubleYellow);
            for (int h = 0; h < word.Length; h++)
            {
                for (int i = 0; i < disallowedLetters.Count; i++)
                {
                    if (disallowedLetters[i] != doubleLetters[0] && disallowedLetters[i] != doubleLetters[1] && disallowedLetters[i] != tripleLetter)
                    {
                        for (int j = 0; j < remainingWords.Count; j++)
                        {
                            if (remainingWords[j][h] == disallowedLetters[i])
                            {
                                indexes.Add(j);
                            }
                        }
                    }
                }
                indexes.Sort();
                indexes.Reverse();
                for (int i = 0; i < indexes.Count; i++)
                {
                    remainingWords.RemoveAt(indexes[i]);
                }
                indexes.Clear();
            }
            for (int h = 0; h < remainingWords.Count; h++)
            {
                for (int i = 0; i < doubleLetters.Length; i++)
                {
                    int count = 0;
                    for (int j = 0; j < word.Length; j++)
                    {
                        if (remainingWords[h][j] == doubleLetters[i])
                        {
                            count++;
                        }
                    }
                    if (count > 1)
                    {
                        indexes.Add(h);
                        h--;
                    }
                    indexes.Sort();
                    indexes.Reverse();
                    for (int j = 0; j < indexes.Count; j++)
                    {
                        remainingWords.RemoveAt(indexes[j]);
                    }
                    indexes.Clear();
                }
            }
            for (int h = 0; h < remainingWords.Count; h++)
            {
                int count = 0;
                for (int j = 0; j < word.Length; j++)
                {
                    if (remainingWords[h][j] == tripleLetter)
                    {
                        count++;
                    }
                }
                if (count > 2)
                {
                    indexes.Add(h);
                    h--;
                }
                indexes.Sort();
                indexes.Reverse();
                for (int j = 0; j < indexes.Count; j++)
                {
                    remainingWords.RemoveAt(indexes[j]);
                }
                indexes.Clear();
            }
            for (int h = 0; h < word.Length; h++)
            {
                if (yellowLetters[h] != ' ')
                {
                    for (int j = 0; j < remainingWords.Count; j++)
                    {
                        if (remainingWords[j][h] == yellowLetters[h])
                        {
                            indexes.Add(j);
                        }
                    }
                }
                indexes.Sort();
                indexes.Reverse();
                for (int i = 0; i < indexes.Count; i++)
                {
                    remainingWords.RemoveAt(indexes[i]);
                }
                indexes.Clear();
            }

            for (int h = 0; h < word.Length; h++)
            {
                if (yellowLetters[h] != ' ' && yellowLetters[h] != doubleYellow)
                {
                    for (int j = 0; j < remainingWords.Count; j++)
                    {
                        bool isOK = false;
                        for (int k = 0; k < word.Length; k++)
                        {
                            if (greenLetters[k] == ' ')
                            {
                                if (remainingWords[j][k] == yellowLetters[h])
                                {
                                    isOK = true;
                                }
                            }
                        }
                        if (!isOK)
                        {
                            indexes.Add(j);
                        }
                    }
                }
                indexes.Sort();
                indexes.Reverse();
                for (int i = 0; i < indexes.Count; i++)
                {
                    remainingWords.RemoveAt(indexes[i]);
                }
                indexes.Clear();
            }

            if (doubleYellow != ' ')
            {
                for (int j = 0; j < remainingWords.Count; j++)
                {
                    int letterCount = 0;
                    for (int k = 0; k < word.Length; k++)
                    {
                        if (remainingWords[j][k] == doubleYellow)
                        {
                            letterCount++;
                        }
                    }
                    if (letterCount != 2)
                    {
                        indexes.Add(j);
                    }
                }
                indexes.Sort();
                indexes.Reverse();
                for (int i = 0; i < indexes.Count; i++)
                {
                    remainingWords.RemoveAt(indexes[i]);
                }
                indexes.Clear();
            }

            for (int h = 0; h < word.Length; h++)
            {
                if (greenLetters[h] != ' ')
                {
                    for (int j = 0; j < remainingWords.Count; j++)
                    {
                        if (remainingWords[j][h] != greenLetters[h])
                        {
                            indexes.Add(j);
                        }
                    }
                }
                indexes.Sort();
                indexes.Reverse();
                for (int i = 0; i < indexes.Count; i++)
                {
                    remainingWords.RemoveAt(indexes[i]);
                }
                indexes.Clear();
            }
        }
        
    }

    public void ValidateWord()
    {
        wordValidated = true;
        if (word.Length == 5)
        {
            foreach (char character in word)
            {
                if (character == '1' || character == '2' || character == '3' || character == '4' || character == '5' || character == '6' || character == '7' || character == '8' || character == '9' || character == '0')
                {
                    wordValidated = false;
                    break;
                }
            }
        }
        else
        {
            wordValidated = false;
        }
    }

    public void LetterColor(int button, letterStates state)
    {
        if (state == letterStates.Grey)
        {
            letters[button].GetComponent<Image>().color = new Color(0.23828125f, 0.25f, 0.328125f);
        }
        else if (state == letterStates.Yellow)
        {
            letters[button].GetComponent<Image>().color = new Color(0.94921875f, 0.7578125f, 0.21484375f);
        }
        else if (state == letterStates.Green)
        {
            letters[button].GetComponent<Image>().color = new Color(0.47265625f, 0.71875f, 0.31640625f);
        }
    }

    public void LetterClick(int button)
    {
        states[button]++;
        if (((int)states[button]) > 2)
        {
            states[button] = 0;
        }
        LetterColor(button, states[button]);
    }
}
