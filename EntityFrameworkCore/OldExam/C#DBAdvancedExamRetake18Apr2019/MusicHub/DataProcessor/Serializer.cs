namespace MusicHub.DataProcessor
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Data;
    using ImportDtos;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(x => x.ProducerId == producerId)
                .Select(x => new
                {
                    AlbumName = x.Name,
                    ReleaseDate = x.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = x.Producer.Name,
                    Songs = x.Songs.
                        Select(s => new
                        {
                            SongName = s.Name,
                            Price = s.Price.ToString("f2"),
                            Writer = s.Writer.Name
                        })
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.Writer)
                        .ToArray(),
                    AlbumPrice = x.Songs.Sum(s => s.Price).ToString("f2")
                })
                .OrderByDescending(x => decimal.Parse(x.AlbumPrice))
                .ToArray();

            var result = Engine.JsonSerializer(albums);

            return result;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .Where(x => x.Duration.TotalSeconds > duration)
                .Select(x => new ExportSongsAboveDurationDto
                {
                    SongName = x.Name,
                    WriterName = x.Writer.Name,
                    PerformerFullName = x.SongPerformers
                        .Select(p => p.Performer.FirstName + " " + p.Performer.LastName)
                        .FirstOrDefault(),
                    ProducerName = x.Album.Producer.Name,
                    Duration = x.Duration.ToString("c", CultureInfo.InvariantCulture)
                })
                .OrderBy(x => x.SongName)
                .ThenBy(x => x.WriterName)
                .ThenBy(x => x.PerformerFullName)
                .ToArray();

            var result = Engine.XmlSerializer(songs, "Songs");

            return result;
        }
    }
}