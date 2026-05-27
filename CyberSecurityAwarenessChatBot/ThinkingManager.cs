using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CyberSecurityAwarenessChatBot
{
    class ThinkingManager
    {
        // RANDOM OBJECT
        static Random random =
            new Random();

        // THINKING MESSAGES
        static string[] thinkingMessages =
        {
            "Thinking",
            "Analyzing your request",
            "Checking cybersecurity database",
            "Processing your question"
        };

        // DISPLAY THINKING EFFECT
        public static async Task ShowThinking(
            TextBox chatDisplay)
        {
            // RANDOM MESSAGE
            int index =
                random.Next(thinkingMessages.Length);

            string message =
                thinkingMessages[index];

            // DISPLAY MESSAGE
            chatDisplay.AppendText(
                "\nBot: " + message);

            // ADD DOT ANIMATION
            for (int i = 0; i < 3; i++)
            {
                await Task.Delay(400);

                chatDisplay.AppendText(".");
            }

            chatDisplay.AppendText("\n");
        }
    }
}
