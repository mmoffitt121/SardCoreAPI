namespace SardCoreAPI.Models.Theming
{
    public class Theme
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool? IsDefault { get; set; }
        public string? PrimaryColor { get; set; }
        public string? PrimaryColorSelected { get; set; }
        public string? InvertedTextColor { get; set; }
        public string? InvertedTextColorDisabled { get; set; }
        public string? TextColor { get; set; }
        public string? TextColorDisabled { get; set; }
        public string? SecondaryTextColor { get; set; }
        public string? TertiaryTextColor { get; set; }
        public string? PrimaryAccentColor { get; set; }
        public string? PrimaryAccentColorDisabled { get; set; }
        public string? BackgroundColor { get; set; }
        public string? SecondaryBackgroundColor { get; set; }
        public string? FieldOverlayColor { get; set; }
        public string? FieldOverlayColorDark { get; set; }
        public string? DestructiveActionColor { get; set; }
        public string? PrimaryFont { get; set; }
        public string? FontWeightbold { get; set; }
        public string? DataPointValueFontSize { get; set; }
    }
}
