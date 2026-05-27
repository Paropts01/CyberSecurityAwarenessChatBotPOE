using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Windows;

namespace CyberSecurityAwarenessChatBot
{
    // Simple audio utility that plays a welcome sound when the chat bot starts.
    public class AudioPlayer
    {
        // Plays a greeting sound from a file named "Welcome.wav".
        public static void PlayGreeting()
        {
            try
            {
                // Load the audio file (assumed to be in the application's working directory).
                SoundPlayer player = new SoundPlayer("Welcome.wav");

                // Play the sound asynchronously (does not block the UI).
                player.Play();
            }
            // Catch any errors (e.g., missing file, unsupported format) and show a message box.
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Audio could not play.\n" + ex.Message,
                    "Audio Error");
            }
        }
    }
}
