using Test;

string filePath = "napisy.srt";
List<Subtitle> subtitlesWithZeroMiliseconds = new List<Subtitle>();
List<Subtitle> subtitlesWithoutZeroMiliseconds = new List<Subtitle>();
TimeSpan timeToMove = new TimeSpan(0, 0, 0, 5, 880);

try
{
    string[] srtLines = File.ReadAllLines(filePath);

    int zeroMiliRowNum = 1;
    int withMiliRowNum = 1;

    for (int i = 0; i < srtLines.Length;)
    {
        var subtitle = new Subtitle();

        if (int.TryParse(srtLines[i], out var lineNr))
        {
            subtitle.LineNr = lineNr;
        }
        var times = srtLines[i + 1].Split(" --> ");

        subtitle.From  = TimeSpan.ParseExact(times[0], @"hh\:mm\:ss\,fff", null) + timeToMove;
        subtitle.To = TimeSpan.ParseExact(times[1], @"hh\:mm\:ss\,fff", null) + timeToMove;

        subtitle.FirstLine = srtLines[i + 2];
        if (i +3 < srtLines.Length && !string.IsNullOrEmpty(srtLines[i + 3]))
        {
            subtitle.SecondLine = srtLines[i + 3];
            i += 5;
        }
        else
        {
            i += 4;
        }

        if (subtitle.From.Milliseconds == 0)
        {
            subtitle.LineNr = zeroMiliRowNum;
            subtitlesWithZeroMiliseconds.Add(subtitle);
            zeroMiliRowNum++;
        }
        else
        {
            subtitle.LineNr = withMiliRowNum;
            subtitlesWithoutZeroMiliseconds.Add(subtitle);
            withMiliRowNum++;
        }
    }

    SaveToFile("napisyOutNoMili.srt", subtitlesWithZeroMiliseconds);
    SaveToFile("napisyOutWithMili.srt", subtitlesWithoutZeroMiliseconds);
}
catch (FileNotFoundException)
{
    Console.WriteLine("SRT file not found.");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}

void SaveToFile(string fileName, List<Subtitle> subtitles)
{
    using TextWriter tw = new StreamWriter(fileName);
    foreach (var subtitle in subtitles)
    {
        tw.WriteLine(subtitle.LineNr);
        tw.WriteLine($"{subtitle.From:hh\\:mm\\:ss\\,fff} --> {subtitle.To:hh\\:mm\\:ss\\,fff}");
        tw.WriteLine(subtitle.FirstLine);
        if (!string.IsNullOrEmpty(subtitle.SecondLine))
        {
            tw.WriteLine(subtitle.SecondLine);
        }
        tw.WriteLine(string.Empty);
    }
}