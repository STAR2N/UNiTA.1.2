using System;

namespace FrostweepGames.VoicePro
{
    /// <summary>
    /// Basic audio converter from - to
    /// </summary>
    public sealed class AudioConverter
    {
        /// <summary>
        /// Rescale factor for converting audio from - to
        /// </summary>
        private const int RescaleFactor = 32767;

        /// <summary>
        /// Converts list of samples to bytes array by using 32767 rescale factor
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        public static byte[] FloatToByte(float[] samples)
        {
            short[] intData = new short[samples.Length];

            byte[] bytesData = new byte[samples.Length * 2];

            for (int i = 0; i < samples.Length; i++)
            {
                intData[i] = (short)(samples[i] * RescaleFactor);
                byte[] byteArr = BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(bytesData, i * 2);
            }

            return bytesData;
        }

        /// <summary>
        /// Converts list of bytes to float array by using 32767 rescale factor
        /// </summary>
        /// <param name="bytesData"></param>
        /// <returns></returns>
        public static float[] ByteToFloat(byte[] bytesData)
        {
            int length = bytesData.Length / 2;
            float[] samples = new float[length];

            for (int i = 0; i < length; i++)
                samples[i] = (float)BitConverter.ToInt16(bytesData, i * 2) / RescaleFactor;

            return samples;
        }
    }
}