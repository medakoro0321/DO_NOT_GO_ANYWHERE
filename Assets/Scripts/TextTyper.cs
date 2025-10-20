using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UdonSharp;
using UnityEngine;

namespace CustomDisplayText
{
    public class Text : UdonSharpBehaviour
    {
        /// <summary>
        /// テキストを一文字ずつ順に表示
        /// </summary>
        /// <param name="textUI">TextMeshProUGUI</param>
        /// <param name="text">順に表示したいテキスト</param>
        public void TextTyper(TextMeshProUGUI textUI, string text)
        {
            int textLength = text.Length;
            for (int t = 0; t < textLength; t++)
            {
                textUI.text = text.Substring(0, t);
                SendCustomEventDelayedSeconds(nameof(TextTyper), 0.1f);
            }
        }
    }
}