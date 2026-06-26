using System;
using System.Media;
using System.Windows;

namespace CyberSecurityAwarenessChatBot
{
    public static class AudioPlayer
    {
        public static void PlayGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("Welcome.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Audio could not play.\n" + ex.Message, "Audio Error");
            }
        }

        // New: Play button click sound
       

        // New: Play clapping sound when quiz is completed
        public static void PlayClap()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("clapping.wav");
                player.Play();
            }
            catch
            {
                // Silently fail
            }
        }
    }
}
