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
            // Create a new paragraph to hold the message.
            Paragraph paragraph = new Paragraph();

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

            // Add a line break after the message to separate it from future messages.
            paragraph.Inlines.Add(new LineBreak());
        }
    }
}