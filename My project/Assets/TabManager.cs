using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics; // Added for BigInteger

public class TabManager : MonoBehaviour
{
    public GameObject[] tabs;
    public Button[] tabButtons;

    // Caesar Cipher output source
    public TMP_Text caesarCipherOutputText;

    // Binary Converter output source
    public TMP_Text binaryConverterOutputText;

    // Caesar Cipher variables
    public TMP_InputField textInputField;
    public TMP_InputField shiftInput;
    public Button decodeButton;

    // Binary converter variables
    public TMP_InputField numberInputField; // Input field for number
    public Button convertButton;
    public Button copyButton; // Copy button for binary output

    void Start()
    {
        // Initialize tab switching
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i;
            tabButtons[i].onClick.AddListener(() => SwitchTab(index));
        }

        // Add listeners for encoding and conversion functions
        decodeButton.onClick.AddListener(EncodeCaesarCipher);
        convertButton.onClick.AddListener(ConvertBinary);
        copyButton.onClick.AddListener(CopyToClipboard); // Add listener for copy button

        // Show the first tab by default
        SwitchTab(0);
    }

    void SwitchTab(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(i == index);
            tabButtons[i].GetComponent<Image>().color = (i == index) ? Color.white : Color.gray;
        }
    }

    void EncodeCaesarCipher()
    {
        string textToEncode = textInputField.text;
        int shift;
        if (int.TryParse(shiftInput.text, out shift))
        {
            string encodedText = CaesarCipherEncode(textToEncode, shift);
            caesarCipherOutputText.text = "Encoded Text: \n" + encodedText;
        }
        else
        {
            caesarCipherOutputText.text = "Invalid shift value.";
        }
    }

    string CaesarCipherEncode(string input, int shift)
    {
        string encoded = "";
        foreach (char c in input)
        {
            if (char.IsLetter(c))
            {
                char offset = char.IsUpper(c) ? 'A' : 'a';
                encoded += (char)(((c - offset + shift) % 26) + offset);
            }
            else
            {
                encoded += c;
            }
        }
        return encoded;
    }

    void ConvertBinary()
    {
        string inputText = numberInputField.text;
        if (BigInteger.TryParse(inputText, out BigInteger number))
        {
            string binaryString = ConvertToBinary(number);
            string outputString = BinaryToString(binaryString);
            binaryConverterOutputText.text = outputString;
        }
        else
        {
            binaryConverterOutputText.text = "Invalid number input.";
        }
    }

    string ConvertToBinary(BigInteger number)
    {
        // Manually convert BigInteger to binary string
        if (number == 0)
        {
            return "0";
        }

        string binary = "";
        while (number > 0)
        {
            binary = (number % 2).ToString() + binary;
            number /= 2;
        }
        return binary;
    }

    string BinaryToString(string binary)
    {
        string result = "";
        // Ensure the binary string length is a multiple of 8
        int remainder = binary.Length % 8;
        if (remainder != 0)
        {
            binary = new string('0', 8 - remainder) + binary; // Prepend leading zeros if needed
        }

        for (int i = 0; i < binary.Length; i += 8) // Process binary in chunks of 8 bits (1 byte)
        {
            string byteString = binary.Substring(i, 8); // 8 bits for each character
            int byteValue = Convert.ToInt32(byteString, 2);
            result += (char)byteValue; // Convert byte to corresponding ASCII character
        }
        return result;
    }

    void CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = binaryConverterOutputText.text; // Copies the output text to clipboard
    }
}





