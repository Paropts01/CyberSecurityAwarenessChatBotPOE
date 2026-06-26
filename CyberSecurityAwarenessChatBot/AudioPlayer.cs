using System;
using System.Media;
using System.Windows;

namespace CyberSecurityAwarenessChatBot
{
  
    //Provides static methods to play audio files (greeting and clapping).
   
    public static class AudioPlayer
    {

        // Plays the welcome sound (Welcome.wav). Shows a message box on error.

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

       
        // Plays the clapping sound (clapping.wav) when a quiz is completed.
        // Errors are silently ignored.
        
        public static void PlayClap()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("clapping.wav");
                player.Play();
            }
            catch
            {
                // Silently fail – no alert needed for a celebratory sound.
            }
        }
    }
}