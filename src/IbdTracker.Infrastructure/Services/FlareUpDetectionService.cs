using System;
using IbdTracker.Core.CommonDtos;

namespace IbdTracker.Infrastructure.Services
{
    /// <summary>
    /// Service that compares patient data to clinical thresholds to determine if it is likely
    /// that they are in a flare up.
    /// </summary>
    public interface IFlareUpDetectionService
    {
        /// <summary>
        /// The avg pain events per day threshold beyond which an abnormality is noted. 
        /// </summary>
        public int PainEventsPerDayThreshold { get; }
        
        /// <summary>
        /// The avg BMs per day threshold beyond which an abnormality is noted. 
        /// </summary>
        public int BmsPerDayThreshold { get; }
        
        /// <summary>
        /// The percentage of total BMs that are bloody beyond which an abnormality is noted.
        /// </summary>
        public int BloodyBmsPercentageThreshold { get; }
        
        /// <summary>
        /// Analyses the latest daily averages of several clinical parameters
        /// to determine if it is likely that they are in a flare up.
        /// </summary>
        /// 
        /// <param name="patientId">The ID of the patient that is being analysed.</param>
        /// <param name="timePeriodInDays">The length of the time period being considered, in days.</param>
        /// <param name="painEventsCount">Total pain events count over the considered period.</param>
        /// <param name="bmsTotalCount">Total BMs count over the considered period.</param>
        /// <param name="bmsBloodyCount">Total bloody BMs count over the considered period.</param>
        /// <returns>Result of the analysis.</returns>
        FlareUpDetectionResult AnalyseLatestDailyAverages(string patientId, int timePeriodInDays, int painEventsCount,
            int bmsTotalCount, int bmsBloodyCount);
    }
    
    /// <inheritdoc />
    public class FlareUpDetectionService : IFlareUpDetectionService
    {
        public FlareUpDetectionService()
        {
            PainEventsPerDayThreshold = 3;
            BmsPerDayThreshold = 3;
            BloodyBmsPercentageThreshold = 15;
        }

        public int PainEventsPerDayThreshold { get; }
        public int BmsPerDayThreshold { get; }
        public int BloodyBmsPercentageThreshold { get; }

        /// <inheritdoc />
        public FlareUpDetectionResult AnalyseLatestDailyAverages(string patientId, int timePeriodInDays,
            int painEventsCount, int bmsTotalCount, int bmsBloodyCount)
        {
            if (timePeriodInDays <= 0)
            {
                throw new ArgumentException("Time period can not be less than or equal to 0.",
                    nameof(timePeriodInDays));
            }

            if (painEventsCount < 0 || bmsTotalCount < 0 || bmsBloodyCount < 0)
            {
                throw new ArgumentException("None of the counts can be less than 0");
            }
            
            var pesAverageAmountPerDay = (double) painEventsCount / timePeriodInDays;
            var bmsAverageAmountPerDay = (double) bmsTotalCount / timePeriodInDays;
            var bloodyBmsPercentage = bmsTotalCount == 0 ? 0 : (double) bmsBloodyCount / bmsTotalCount * 100;

            if (
                pesAverageAmountPerDay > PainEventsPerDayThreshold
                || bmsAverageAmountPerDay > BmsPerDayThreshold
                || bloodyBmsPercentage > BloodyBmsPercentageThreshold
            )
            {
                return new(patientId,
                    true,
                    new(PainEventsPerDayThreshold, pesAverageAmountPerDay),
                    new(BmsPerDayThreshold, bmsAverageAmountPerDay),
                    new(BloodyBmsPercentageThreshold, bloodyBmsPercentage));
            }

            return new(patientId,
                false,
                new(PainEventsPerDayThreshold, pesAverageAmountPerDay),
                new(BmsPerDayThreshold, bmsAverageAmountPerDay),
                new(BloodyBmsPercentageThreshold, bloodyBmsPercentage));
        }
    }
}