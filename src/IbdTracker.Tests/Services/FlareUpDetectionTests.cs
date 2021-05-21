using System;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using Xunit;

namespace IbdTracker.Tests.Services
{
    public class FlareUpDetectionTests : TestBase
    {
        private readonly IFlareUpDetectionService _service;

        public FlareUpDetectionTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
            _service = GetService<IFlareUpDetectionService>();
        }

        [Fact]
        public void ShouldThrowIfTimePeriodLessThan0()
        {
            // arrange;
            var patientId = TestUserHelper.TestPatientId;
            const int timePeriodInDays = -1;
            const int painEventsCount = 0;
            const int bmsTotalCount = 0;
            const int bmsBloodyCount = 0;
            
            // act;
            Func<FlareUpDetectionResult> act = () => _service.AnalyseLatestDailyAverages(patientId, timePeriodInDays, painEventsCount, bmsTotalCount,
                bmsBloodyCount); 
            
            // assert;
            act.Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void ShouldThrowIfTimePeriodEqualTo0()
        {
            // arrange;
            var patientId = TestUserHelper.TestPatientId;
            const int timePeriodInDays = 0;
            const int painEventsCount = 0;
            const int bmsTotalCount = 0;
            const int bmsBloodyCount = 0;
            
            // act;
            Func<FlareUpDetectionResult> act = () => _service.AnalyseLatestDailyAverages(patientId, timePeriodInDays, painEventsCount, bmsTotalCount,
                bmsBloodyCount); 
            
            // assert;
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldThrowIfPainEventsCountLessThan0()
        {
            // arrange;
            var patientId = TestUserHelper.TestPatientId;
            const int timePeriodInDays = 14;
            const int painEventsCount = -1;
            const int bmsTotalCount = 0;
            const int bmsBloodyCount = 0;
            
            // act;
            Func<FlareUpDetectionResult> act = () => _service.AnalyseLatestDailyAverages(patientId, timePeriodInDays, painEventsCount, bmsTotalCount,
                bmsBloodyCount); 
            
            // assert;
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldThrowIfBmsTotalCountLessThan0()
        {
            // arrange;
            var patientId = TestUserHelper.TestPatientId;
            const int timePeriodInDays = 14;
            const int painEventsCount = 0;
            const int bmsTotalCount = -1;
            const int bmsBloodyCount = 0;
            
            // act;
            Func<FlareUpDetectionResult> act = () => _service.AnalyseLatestDailyAverages(patientId, timePeriodInDays, painEventsCount, bmsTotalCount,
                bmsBloodyCount); 
            
            // assert;
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldThrowIfBmsBloodyCountLessThan0()
        {
            // arrange;
            var patientId = TestUserHelper.TestPatientId;
            const int timePeriodInDays = 14;
            const int painEventsCount = 0;
            const int bmsTotalCount = 0;
            const int bmsBloodyCount = -1;
            
            // act;
            Func<FlareUpDetectionResult> act = () => _service.AnalyseLatestDailyAverages(patientId, timePeriodInDays, painEventsCount, bmsTotalCount,
                bmsBloodyCount); 
            
            // assert;
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldReturnFalseIfAllParametersNormal()
        {
            // arrange;
            var patientId = TestUserHelper.TestPatientId;
            const int timePeriodInDays = 14;
            const int painEventsCount = 1;
            const int bmsTotalCount = 10;
            const int bmsBloodyCount = 1;
            
            // act;
            var res = _service.AnalyseLatestDailyAverages(patientId, timePeriodInDays, painEventsCount, bmsTotalCount,
                bmsBloodyCount); 
            
            // assert;
            res.IsInFlareUp.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnTrueIfBloodyPercentageAboveThreshold()
        {
            // arrange;
            var patientId = TestUserHelper.TestPatientId;
            const int timePeriodInDays = 14;
            const int painEventsCount = 1;
            const int bmsTotalCount = 1;
            const int bmsBloodyCount = 1;
            
            // act;
            var res = _service.AnalyseLatestDailyAverages(patientId, timePeriodInDays, painEventsCount, bmsTotalCount,
                bmsBloodyCount); 
            
            // assert;
            res.IsInFlareUp.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnTrueIfPainEventsAboveThreshold()
        {
            // arrange;
            var patientId = TestUserHelper.TestPatientId;
            const int timePeriodInDays = 14;
            const int painEventsCount = 56;
            const int bmsTotalCount = 1;
            const int bmsBloodyCount = 1;
            
            // act;
            var res = _service.AnalyseLatestDailyAverages(patientId, timePeriodInDays, painEventsCount, bmsTotalCount,
                bmsBloodyCount); 
            
            // assert;
            res.IsInFlareUp.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnTrueIfBmsPerDayAboveThreshold()
        {
            // arrange;
            var patientId = TestUserHelper.TestPatientId;
            const int timePeriodInDays = 14;
            const int painEventsCount = 1;
            const int bmsTotalCount = 56;
            const int bmsBloodyCount = 1;
            
            // act;
            var res = _service.AnalyseLatestDailyAverages(patientId, timePeriodInDays, painEventsCount, bmsTotalCount,
                bmsBloodyCount); 
            
            // assert;
            res.IsInFlareUp.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnTrueIfAllAboveThreshold()
        {
            // arrange;
            var patientId = TestUserHelper.TestPatientId;
            const int timePeriodInDays = 14;
            const int painEventsCount = 56;
            const int bmsTotalCount = 56;
            const int bmsBloodyCount = 56;
            
            // act;
            var res = _service.AnalyseLatestDailyAverages(patientId, timePeriodInDays, painEventsCount, bmsTotalCount,
                bmsBloodyCount); 
            
            // assert;
            res.IsInFlareUp.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnTrueIfBothBmParametersAboveThreshold()
        {
            // arrange;
            var patientId = TestUserHelper.TestPatientId;
            const int timePeriodInDays = 14;
            const int painEventsCount = 1;
            const int bmsTotalCount = 56;
            const int bmsBloodyCount = 56;
            
            // act;
            var res = _service.AnalyseLatestDailyAverages(patientId, timePeriodInDays, painEventsCount, bmsTotalCount,
                bmsBloodyCount); 
            
            // assert;
            res.IsInFlareUp.Should().BeTrue();
        }
    }
}