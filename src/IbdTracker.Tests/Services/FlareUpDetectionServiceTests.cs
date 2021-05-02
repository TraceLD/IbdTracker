using System;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Patients.FlareUps;
using IbdTracker.Infrastructure.Services;
using Xunit;

namespace IbdTracker.Tests.Services
{
    public class FlareUpDetectionServiceTests : TestBase
    {
        private readonly IFlareUpDetectionService _flareUpService;
        
        public FlareUpDetectionServiceTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
            _flareUpService = GetService<IFlareUpDetectionService>();
        }

        [Fact]
        public void ShouldReturnTrueIfAllParametersAboveThresholds()
        {
            // arrange;
            const int timePeriodInDays = 14;
            var painEventsCount = _flareUpService.PainEventsPerDayThreshold * timePeriodInDays + 1;
            var bmsTotalCount = _flareUpService.BmsPerDayThreshold * timePeriodInDays + 1;
            var bmsBloodyCount = _flareUpService.BloodyBmsPercentageThreshold + 1;

            // act;
            var res = _flareUpService.AnalyseLatestDailyAverages(TestUserHelper.TestPatientId, timePeriodInDays,
                painEventsCount, bmsTotalCount, bmsBloodyCount);
            
            // assert;
            res.IsInFlareUp.Should().BeTrue();
        }

        [Fact]
        public void ShouldThrowIfDaysPeriodIsZero()
        {
            // arrange;
            const int timePeriodInDays = 0; // the only parameter that matters;

            // act;
            Func<FlareUpDetectionResult> act = () =>
                _flareUpService.AnalyseLatestDailyAverages(TestUserHelper.TestPatientId, timePeriodInDays, 1, 1, 1);
            
            // assert;
            act
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldThrowIfDaysPeriodIsLessThanZero()
        {
            // arrange;
            const int timePeriodInDays = -1; // the only parameter that matters;

            // act;
            Func<FlareUpDetectionResult> act = () =>
                _flareUpService.AnalyseLatestDailyAverages(TestUserHelper.TestPatientId, timePeriodInDays, 1, 1, 1);
            
            // assert;
            act
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldReturnTrueIfOneParameterIsAboveThreshold()
        {
            // arrange;
            const int timePeriodInDays = 14;
            var painEventsCount = _flareUpService.PainEventsPerDayThreshold * timePeriodInDays + 1;
            var bmsTotalCount = _flareUpService.BmsPerDayThreshold * timePeriodInDays - 1;
            var bmsBloodyCount = _flareUpService.BloodyBmsPercentageThreshold - 1;

            // act;
            var res = _flareUpService.AnalyseLatestDailyAverages(TestUserHelper.TestPatientId, timePeriodInDays,
                painEventsCount, bmsTotalCount, bmsBloodyCount);
            
            // assert;
            res.IsInFlareUp.Should().BeTrue();
        }

        [Fact]
        public void ShouldThrowIfAnyOfTheCountsAreLessThanZero()
        {
            // arrange;
            const int painEventsCount = -10; // the only parameter that matters;

            // act;
            Func<FlareUpDetectionResult> act = () =>
                _flareUpService.AnalyseLatestDailyAverages(TestUserHelper.TestPatientId, 14, painEventsCount, 1, 1);
            
            // assert;
            act
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("None of the counts can be less than 0");
        }

        [Fact]
        public void ShouldCalculatePercentageCorrectly()
        {
            // arrange;
            const int timePeriodInDays = 14;
            var painEventsCount = _flareUpService.PainEventsPerDayThreshold * timePeriodInDays - 1; // so that it doesn't trip;
            const int bmsTotalCount = 10;
            const int bmsBloodyCount = 5;
            
            // act;
            var res = _flareUpService.AnalyseLatestDailyAverages(TestUserHelper.TestPatientId, timePeriodInDays,
                painEventsCount, bmsTotalCount, bmsBloodyCount);

            res.IsInFlareUp.Should().Be(true);
            res.BloodyBmsPercentage.ActualValue.Should().Be(50);
        }

        [Fact]
        public void ShouldCalculatePainEventsAvgCorrectly()
        {
            // arrange;
            const int timePeriodInDays = 14;
            const int painEventsCount = 7;
            const int bmsTotalCount = 14;
            const int bmsBloodyCount = 0;
            
            // act;
            var res = _flareUpService.AnalyseLatestDailyAverages(TestUserHelper.TestPatientId, timePeriodInDays,
                painEventsCount, bmsTotalCount, bmsBloodyCount);

            res.IsInFlareUp.Should().Be(false);
            res.PainEventsPerDay.ActualValue.Should().Be(0.5);
        }

        [Fact]
        public void ShouldCalculateBmsAvgCorrectly()
        {
            // arrange;
            const int timePeriodInDays = 14;
            const int painEventsCount = 14;
            const int bmsTotalCount = 7;
            const int bmsBloodyCount = 0;
            
            // act;
            var res = _flareUpService.AnalyseLatestDailyAverages(TestUserHelper.TestPatientId, timePeriodInDays,
                painEventsCount, bmsTotalCount, bmsBloodyCount);

            res.IsInFlareUp.Should().Be(false);
            res.BmsPerDay.ActualValue.Should().Be(0.5);
        }
    }
}