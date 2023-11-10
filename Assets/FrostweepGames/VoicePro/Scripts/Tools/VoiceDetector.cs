namespace FrostweepGames.VoicePro
{
    public class VoiceDetector
    {
        /// <summary>
        /// Filters data based on threshold
        /// </summary>
        /// <param name="data">input bytes data</param>
        /// <param name="averageVoiceLevel">ref value of current voice level</param>
        /// <param name="threshold">threshold filter</param>
        /// <returns></returns>
        public static bool IsVoiceDetected(byte[] data, ref float averageVoiceLevel, double threshold = 0.02d)
        {
            bool detected = false;
            double sumTwo = 0;
            double tempValue;

            for (int index = 0; index < data.Length; index += 2)
            {
                tempValue = (short)((data[index + 1] << 8) | data[index + 0]);

                tempValue /= 32768.0d;

                sumTwo += tempValue * tempValue;

                if (tempValue > threshold)
                    detected = true;
            }

            sumTwo /= (data.Length / 2);

            averageVoiceLevel = (averageVoiceLevel + (float)sumTwo) / 2f;

            if (detected || sumTwo > threshold)
                return true;
            else
                return false;
        }
    }
}