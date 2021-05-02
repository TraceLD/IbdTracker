namespace IbdTracker.Core.CommonDtos
{
    public record FlareUpDetectionResult(
        string PatientId,
        bool IsInFlareUp,
        ThresholdWithValue PainEventsPerDay,
        ThresholdWithValue BmsPerDay,
        ThresholdWithValue BloodyBmsPercentage
    );
    
    public record ThresholdWithValue(double Threshold, double ActualValue);
}