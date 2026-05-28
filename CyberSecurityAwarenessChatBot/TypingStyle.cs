using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CyberSecurityAwarenessChatBot
{
    // Provides a typing animation effect for messages displayed in the chat.
    // Messages appear character-by-character with a small delay between each character.
    public class TypingStyle
    {
        // Displays a message in the RichTextBox with a typing animation.
        // Parameters:
        //   chatDisplay: The RichTextBox control where the message will appear.
        //   sender: The name of the message sender (e.g., "Bot" or "You").
        //   message: The actual text content to display.
        //   color: The color of the entire message text.
        public static async Task TypeText(RichTextBox chatDisplay, string sender, string message, Brush color)
        {
            // 1. Determine alignment and margins based on the sender
            bool isUser = (sender == "You");
            TextAlignment alignment = isUser ? TextAlignment.Right : TextAlignment.Left;

            // Give a side margin so long texts don't span the absolute entire width of the window
            Thickness messageMargin = isUser ? new Thickness(60, 4, 10, 2) : new Thickness(10, 4, 60, 2);

            // 2. Create a new paragraph to hold the message with alignment and margins applied
            Paragraph paragraph = new Paragraph
            {
                TextAlignment = alignment,
                Margin = messageMargin
            };

            // Create and format the sender label (e.g., "Bot: ") in bold and the specified color.
            Run senderRun = new Run(sender + ": ");
            senderRun.Foreground = color;
            senderRun.FontWeight = FontWeights.Bold;
            paragraph.Inlines.Add(senderRun);

            // Create an empty Run for the message text (will be built character by character).
            Run messageRun = new Run("");
            messageRun.Foreground = color;   // Message text uses the same color as the sender label.
            paragraph.Inlines.Add(messageRun);

            // Add the paragraph to the chat display document.
            chatDisplay.Document.Blocks.Add(paragraph);

            // Simulate typing: add each character one by one with a 40ms delay.
            foreach (char c in message)
            {
                messageRun.Text += c;          // Append the next character.
                await Task.Delay(40);           // Pause to mimic typing speed.
                chatDisplay.ScrollToEnd();      // Auto-scroll to show the latest character.
            }

            // 3. Create a separate paragraph for the timestamp directly underneath the text
            Thickness timeMargin = isUser ? new Thickness(60, 0, 10, 12) : new Thickness(10, 0, 60, 12);
            Paragraph timeParagraph = new Paragraph
            {
                TextAlignment = alignment,
                Margin = timeMargin,
                FontSize = 10,                 // Make the timestamp text subtly smaller
                Foreground = Brushes.Gray       // Muted text color for the time
            };

            // Get current time formatted (e.g., "1:45 PM")
            string currentTime = DateTime.Now.ToString("h:mm tt");
            timeParagraph.Inlines.Add(new Run(currentTime));

            // Add timestamp block to the document
            chatDisplay.Document.Blocks.Add(timeParagraph);
            chatDisplay.ScrollToEnd();
        }
    }
}
