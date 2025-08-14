using System;
using TWPFX.Service;

namespace TWPFX_Gallery.Services
{
    /// <summary>
    /// 电影台词生成服务
    /// </summary>
    public static class MovieQuoteService
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// 获取随机电影台词
        /// </summary>
        /// <returns>本地化的电影台词</returns>
        public static string GetRandomMovieQuote()
        {
            // 生成1到50的随机数
            int quoteNumber = _random.Next(1, 51);
            string quoteKey = $"Movies_{quoteNumber}";
            
            // 获取本地化的电影台词
            return TLocalizationService.GetLocalizedString(quoteKey, $"Movie Quote {quoteNumber}");
        }

        /// <summary>
        /// 获取指定编号的电影台词
        /// </summary>
        /// <param name="quoteNumber">台词编号（1-50）</param>
        /// <returns>本地化的电影台词</returns>
        public static string GetMovieQuote(int quoteNumber)
        {
            if (quoteNumber < 1 || quoteNumber > 50)
                throw new ArgumentOutOfRangeException(nameof(quoteNumber), "Quote number must be between 1 and 50");

            string quoteKey = $"Movies_{quoteNumber}";
            return TLocalizationService.GetLocalizedString(quoteKey, $"Movie Quote {quoteNumber}");
        }
    }
} 