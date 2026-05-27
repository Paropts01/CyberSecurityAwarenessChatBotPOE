using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CyberSecurityAwarenessChatBot
{
    public class TypingStyle
    {
        public static async Task TypeText(RichTextBox chatDisplay, string sender, string message, Brush color)
        {
            Paragraph paragraph = new Paragraph();

            Run senderRun = new Run(sender + ": ");
            senderRun.Foreground = color;
            senderRun.FontWeight = FontWeights.Bold;
            paragraph.Inlines.Add(senderRun);

            Run messageRun = new Run("");
            messageRun.Foreground = color;   // whole message same colour
            paragraph.Inlines.Add(messageRun);

            chatDisplay.Document.Blocks.Add(paragraph);

            foreach (char c in message)
            {
                messageRun.Text += c;
                await Task.Delay(20);
                chatDisplay.ScrollToEnd();
            }

            // Optional line break (no divider)
            paragraph.Inlines.Add(new LineBreak());
        }
    }
}
