using System.Globalization;

namespace Pls.SimpleMongoDb.Utils
{
    public interface ISimoPluralizer
    {
        CultureInfo Culture { get; }
        bool IsEnabled { get; }
        void AdjustPluralization(string value, string pluralized);
        void ClearPluralizationAdjustments();
        string Pluralize(string value);
        void Disable();
        void Enable();
    }
}