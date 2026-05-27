using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Windows;

namespace CyberSecurityAwarenessChatBot
{
    public class AudioPlayer
    {
        public static void PlayGreeting()
        {
            try
            {
                // LOAD AUDIO FILE
                SoundPlayer player =
                    new SoundPlayer("Welcome.wav");

                // PLAY AUDIO
                player.Play();
            }

            // EXCEPTION HANDLING
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Audio could not play.\n" + ex.Message,
                    "Audio Error");
            }
        }
    }
}
