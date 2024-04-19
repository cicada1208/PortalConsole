using Params;
using System;
using System.Globalization;
using System.Media;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace Lib.Wpf
{
    public class MediaUtil
    {
        public void SoundPlay(System.IO.UnmanagedMemoryStream wav)
        { // 使用方式：將OK.wav加入專案資源，SoundPlay(Properties.Resources.OK)
            SoundPlayer soundPlayer = new SoundPlayer();

            try
            {
                soundPlayer.Stream = wav;
                soundPlayer.Play();
            }
            catch (Exception) { }
            finally
            {
                soundPlayer?.Dispose();
                soundPlayer = null;
            }
        }

        /// <summary>
        /// msg sound for prompt
        /// </summary>
        public void MsgOKErrSoundPlay(MediaParam.MsgType msgType)
        {
            string uri = string.Empty;
            SoundPlayer soundPlayer = new SoundPlayer();

            try
            {
                switch (msgType)
                {
                    case MediaParam.MsgType.OK:
                        uri = System.Environment.CurrentDirectory + "/Resources/OK.wav";
                        break;
                    default:
                        uri = System.Environment.CurrentDirectory + "/Resources/Err.wav";
                        break;
                }

                soundPlayer.SoundLocation = uri;
                soundPlayer.Play();
            }
            catch (Exception) { }
            finally
            {
                soundPlayer?.Dispose();
                soundPlayer = null;
            }
        }

        /// <summary>
        /// msg sound for prompt
        /// </summary>
        public void MsgOKErrMediaPlay(MediaParam.MsgType msgType)
        {
            string uri = string.Empty;
            MediaPlayer mediaPlayer;

            try
            {
                switch (msgType)
                {
                    case MediaParam.MsgType.OK:
                        // can't use pack uri's as a Source for the MediaElement
                        // (apparently because of a Windows Media Player limitation).
                        // uri = "pack://application:,,,/Resources;component/Media/OK.wav";

                        // clickonce 無法播放!
                        // uri = "pack://siteoforigin:,,,/Media/OK.wav"
                        // uri = "pack://siteoforigin:,,,/Resources/OK.wav"

                        // 目前先在主專案加入資源
                        uri = "Resources/OK.wav"; // uri = System.Environment.CurrentDirectory + "/Resources/OK.wav";
                        break;
                    default:
                        uri = "Resources/Err.wav";
                        break;
                }

                mediaPlayer = new MediaPlayer();
                mediaPlayer.Open(new Uri(uri, UriKind.RelativeOrAbsolute));
                mediaPlayer.Play();
            }
            catch (Exception) { }
            //finally
            //{
            //    mediaPlayer = null;
            //}
        }

        /// <summary>
        /// output speech information about all of the installed voices
        /// </summary>
        public void SpeechVoiceInfo()
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();

            Console.WriteLine("Installed voices -");
            foreach (InstalledVoice voice in synth.GetInstalledVoices())
            {
                VoiceInfo info = voice.VoiceInfo;
                string AudioFormats = "";
                foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                {
                    AudioFormats += String.Format("{0}\n",
                    fmt.EncodingFormat.ToString());
                }

                Console.WriteLine(" Name:          " + info.Name);
                Console.WriteLine(" Culture:       " + info.Culture);
                Console.WriteLine(" Age:           " + info.Age);
                Console.WriteLine(" Gender:        " + info.Gender);
                Console.WriteLine(" Description:   " + info.Description);
                Console.WriteLine(" ID:            " + info.Id);
                Console.WriteLine(" Enabled:       " + voice.Enabled);
                if (info.SupportedAudioFormats.Count != 0)
                    Console.WriteLine(" Audio formats: " + AudioFormats);
                else
                    Console.WriteLine(" No supported audio formats found");

                string AdditionalInfo = "";
                foreach (string key in info.AdditionalInfo.Keys)
                {
                    AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                }

                Console.WriteLine(" Additional Info - " + AdditionalInfo);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// msg text to speech
        /// </summary>
        /// <remarks>win7 client 需安裝 
        /// Microsoft Server Speech Platform Runtime (x86)、
        /// Microsoft Server Speech Text to Speech Voice (zh-TW, HanHan)
        /// </remarks>
        public void MsgSpeech(string msgSpeech, bool show = true, bool closeSpeechAfterOK = false, string msgShow="")
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            
            try
            {
                synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0,
                    CultureInfo.GetCultureInfo("zh-TW")); // en-us、zh-TW、zh-CN
                //synth.SelectVoice("Microsoft Hanhan Desktop"); // 中文
                synth.Volume = 100;
                synth.SpeakAsync(msgSpeech);
            }
            catch (Exception) { }
            finally
            {
                if (show)
                {
                    if (msgShow.IsNullOrWhiteSpace()) msgShow = msgSpeech;
                    MessageBox.Show(msgShow, MsgParam.TitlePrompt);
                    if (closeSpeechAfterOK) synth.Dispose();
                }
            }
        }

        public string BedNoSpeech(string bedno)
        {
            Regex r = new Regex(@"\d*\D+"); // 12D123、NBC07
            string result = r.Match(bedno).Value + r.Replace(bedno, "", 1).ConcatSeparator(); // 12D1 2 3、NBC0 7
            return result;
        }

    }
}
