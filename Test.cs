 DateTime parsedDate;
                var isValidDate = DateTime.TryParseExact(
                    dto.DateTime, "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture, 
                        DateTimeStyles.None, out parsedDate); // parse to DateTime
                if (!isValidDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var date = parsedDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture); // convert DateTime to string
                
                var projection = new Projection()
                {
                    Hall = context.Halls.FirstOrDefault(x => x.Id == dto.HallId),
                    Movie = context.Movies.FirstOrDefault(x => x.Id == dto.MovieId),
                    DateTime = DateTime.Parse(date) // convert string  to DateTime 
                };