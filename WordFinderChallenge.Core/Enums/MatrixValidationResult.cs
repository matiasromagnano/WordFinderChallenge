namespace WordFinderChallenge.Core.Enums;

public enum MatrixValidationResult
{
    Valid,
    NullOrEmpty,
    ExceedsMaxSize,
    InconsistentRowLengths,
    RowExceedsMaxLength
}
