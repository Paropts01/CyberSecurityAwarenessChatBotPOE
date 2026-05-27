using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CyberSecurityAwarenessChatBot
{
    // Provides a visual "thinking" effect to show the bot is processing the user's request.
    // Displays a random status message followed by an animated ellipsis (...).
    public class ThinkingManager
    {
        // Random number generator used to pick a random thinking message from the array.
        static Random random = new Random();

        // Predefined list of messages that appear while the bot "thinks".
        static string[] thinkingMessages =
        {
            "Thinking",
            "Analyzing your request",
            "Checking cybersecurity database",
            "Processing your question"
        };

        // Asynchronously displays a thinking message and a dot animation in the chat display.
        // Parameter: chatDisplay - a TextBox control where the thinking status will be appended.
        public static async Task ShowThinking(TextBox chatDisplay)
        {
            // Select a random index from the thinkingMessages array.
            int index = random.Next(thinkingMessages.Length);

            // Retrieve the randomly chosen message.
            string message = thinkingMessages[index];

            // Append the message to the chat display with a "Bot: " prefix and a newline.
            chatDisplay.AppendText("\nBot: " + message);

            // Animate three dots with a 400ms delay between each dot to simulate processing.
            for (int i = 0; i < 3; i++)
            {
                await Task.Delay(400);
                chatDisplay.AppendText(".");
            }

            // Add a final newline after the animation.
            chatDisplay.AppendText("\n");
        }
    }
}