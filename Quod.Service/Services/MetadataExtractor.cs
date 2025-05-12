using Quod.Domain;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace Quod.Service
{
    public class MetadataExtractor : IMetadataExtractor
    {

        public MetadataExtractor()
        {;
        }

        public async Task<ImageMetadata> ExtractMetadataAsync(byte[] imageData)
        {
            var metadata = new ImageMetadata
            {
                DeviceInfo = new DeviceViewModel(),
                RawMetadata = new Dictionary<string, string>()
            };

            try
            {
                using var ms = new MemoryStream(imageData);
                var image = await Image.LoadAsync(ms);

                if (image.Metadata.ExifProfile != null)
                {
                    var exif = image.Metadata.ExifProfile;

                    if (exif.TryGetValue(ExifTag.DateTime, out IExifValue<string>? dateTimeTag))
                    {
                        if (dateTimeTag != null && DateTime.TryParse(dateTimeTag.Value, out var captureDate))
                        {
                            metadata.CaptureDate = captureDate;
                        }
                    }

                    if (exif.TryGetValue(ExifTag.Make, out IExifValue<string>? makeTag))
                    {
                        if (makeTag != null)
                        {
                            metadata.DeviceInfo.Manufacturer = makeTag.Value!;
                        }
                    }

                    if (exif.TryGetValue(ExifTag.Model, out IExifValue<string>? modelTag))
                    {
                        if (modelTag != null)
                        {
                            metadata.DeviceInfo.Model = modelTag.Value!;
                        }
                    }

                    if (!exif.TryGetValue(ExifTag.Make, out _) || !exif.TryGetValue(ExifTag.Model, out _))
                    {
                        metadata.IsSuspect = true;
                    }

                    if (exif.TryGetValue(ExifTag.DateTimeOriginal, out IExifValue<string>? originalDateTag) &&
                    exif.TryGetValue(ExifTag.DateTimeDigitized, out IExifValue<string>? digitizedDateTag) &&
                    DateTime.TryParse(originalDateTag.Value, out var originalDate) &&
                    DateTime.TryParse(digitizedDateTag.Value, out var digitizedDate))
                    {
                        if (Math.Abs((originalDate - digitizedDate).TotalHours) > 48) // Exemplo: Diferença grande
                        {
                            metadata.IsSuspect = true;
                        }
                    }

                    TryExtractGpsData(exif, out var location);
                    if (location != null)
                    {
                        metadata.Location = location;
                    }

                    foreach (var value in exif.Values)
                    {
                        metadata.RawMetadata[value.Tag.ToString()] = value.ToString() ?? string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BiometryException("Erro ao extrair metadados da imagem", ex);
            }

            return metadata;
        }

        private static bool TryExtractGpsData(ExifProfile exif, out GeoLocation? location)
        {
            location = null;
            bool hasGpsData = false;

            try
            {
                if (exif.TryGetValue(ExifTag.GPSLatitudeRef, out IExifValue<string>? latRef) &&
                    exif.TryGetValue(ExifTag.GPSLatitude, out IExifValue<Rational[]>? latValue) &&
                    latValue?.Value is Rational[] latRational && latRational.Length >= 3 &&
                    exif.TryGetValue(ExifTag.GPSLongitudeRef, out IExifValue<string>? longRef) &&
                    exif.TryGetValue(ExifTag.GPSLongitude, out IExifValue<Rational[]>? longValue) &&
                    longValue?.Value is Rational[] longRational && longRational.Length >= 3)
                {
                    double lat = latRational[0].ToDouble() + (latRational[1].ToDouble() / 60) + (latRational[2].ToDouble() / 3600);
                    if (latRef?.Value?.ToUpperInvariant() == "S")
                    {
                        lat = -lat;
                    }

                    double lon = longRational[0].ToDouble() + (longRational[1].ToDouble() / 60) + (longRational[2].ToDouble() / 3600);
                    if (longRef?.Value?.ToUpperInvariant() == "W")
                    {
                        lon = -lon;
                    }

                    location = new GeoLocation { Latitude = lat, Longitude = lon };
                    hasGpsData = true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return hasGpsData;
        }
    }
}