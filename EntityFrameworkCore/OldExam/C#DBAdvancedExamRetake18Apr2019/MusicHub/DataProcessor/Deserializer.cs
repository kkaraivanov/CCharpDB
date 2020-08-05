namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using ImportDtos;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter 
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone 
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong 
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var writer = Engine.JsonDeserializer<ImportWriterDto[]>(jsonString);
            var importWriters = new List<Writer>();

            foreach (var dto in writer)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var import = new Writer()
                {
                    Name = dto.Name,
                    Pseudonym = dto.Pseudonym
                };

                importWriters.Add(import);
                var printResult = string.Format(SuccessfullyImportedWriter, import.Name);
                sb.AppendLine(printResult);
            }
            context.Writers.AddRange(importWriters);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var producersDto = Engine.JsonDeserializer<ImportProducersAlbumsDto[]>(jsonString);
            var producers = new List<Producer>();

            foreach (var dto in producersDto)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var producer = new Producer()
                {
                    Name = dto.Name,
                    Pseudonym = dto.Pseudonym,
                    PhoneNumber = dto.PhoneNumber
                };

                var albumIsValid = true;
                foreach (var dtoAlbum in dto.Albums)
                {
                    if (!IsValid(dtoAlbum))
                    {
                        albumIsValid = false;
                        break;
                    }

                    DateTime parseDate;
                    var dateIsValid = DateTime.TryParseExact(
                        dtoAlbum.ReleaseDate,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out parseDate);
                    if (!dateIsValid)
                    {
                        albumIsValid = false;
                        break;
                    }

                    var album = new Album()
                    {
                        Name = dtoAlbum.Name,
                        ReleaseDate = parseDate
                    };

                    producer.Albums.Add(album);
                }

                if (!albumIsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                producers.Add(producer);
                string  printResult;
                if (producer.PhoneNumber != null)
                    printResult = string.Format(
                        SuccessfullyImportedProducerWithPhone, 
                        producer.Name, 
                        producer.PhoneNumber,
                        producer.Albums.Count);
                else
                    printResult = string.Format(
                        SuccessfullyImportedProducerWithNoPhone,
                        producer.Name,
                        producer.Albums.Count);
                sb.AppendLine(printResult);
            }

            context.Producers.AddRange(producers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var rootAttribute = "Songs";
            var songsDto = Engine.XmlDeserializer<ImportSongsDto>(xmlString, rootAttribute);
            var songs = new List<Song>();

            foreach (var dto in songsDto)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime parseDate;
                var dateIsValid = DateTime.TryParseExact(dto.CreatedOn,"dd/MM/yyyy",
                    CultureInfo.InvariantCulture,DateTimeStyles.None, out parseDate);

                if (!dateIsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                TimeSpan parseSpan;
                var spanIsValid = TimeSpan.TryParseExact(dto.Duration, "c",
                    CultureInfo.InvariantCulture, TimeSpanStyles.AssumeNegative, out parseSpan);

                if (!spanIsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Genre genre;
                var genreIsValid = Enum.TryParse(dto.Genre, out genre);

                if (!genreIsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (dto.WriterId == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var album = dto.AlbumId != null ? context.Albums.FirstOrDefault(x => x.Id == dto.AlbumId) : null;
                var writer = context.Writers.FirstOrDefault(x => x.Id == dto.WriterId);

                if (writer == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (dto.AlbumId != null && album == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var song = new Song()
                {
                    Name = dto.Name,
                    Duration = parseSpan,
                    CreatedOn = parseDate,
                    Genre = genre,
                    AlbumId = album?.Id,
                    WriterId = writer.Id,
                    Price = dto.Price
                };

                songs.Add(song);
                var printResult = string.Format(SuccessfullyImportedSong,
                    song.Name, song.Genre, song.Duration);
                sb.AppendLine(printResult);
            }

            context.Songs.AddRange(songs);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var masterAttribut = "Performers";
            var performersDto = Engine.XmlDeserializer<ImportSongPerformersDto>(xmlString, masterAttribut);
            var performers = new List<Performer>();

            foreach (var dto in performersDto)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                var performer = new Performer()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age,
                    NetWorth = dto.NetWorth
                };

                var songIsValid = true;
                foreach (var performerSong in dto.PerformerSongs)
                {
                    if (performerSong.PerformerSongId == null)
                    {
                        songIsValid = false;
                        break;
                    }

                    var song = context.Songs.FirstOrDefault(x => x.Id == performerSong.PerformerSongId);
                    
                    if (song == null)
                    {
                        songIsValid = false;
                        break;
                    }

                    var newPerformerSong = new SongPerformer()
                    {
                        Song = song,
                        Performer = performer
                    };

                    performer.PerformerSongs.Add(newPerformerSong);
                }

                if (!songIsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                performers.Add(performer);
                var printResult = string.Format(SuccessfullyImportedPerformer,
                    performer.FirstName, performer.PerformerSongs.Count);
                sb.AppendLine(printResult);
            }

            context.Performers.AddRange(performers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}