namespace Theater.Common.Extensions;

public static class BucketIdentifierExtensions
{
    public static string ToBucketName(this BucketIdentifier type)
    {
        return type switch
        {
            //only lowercase names
            BucketIdentifier.Piece => "piece",
            BucketIdentifier.User => "user",
            BucketIdentifier.TheaterWorker => "theater-worker",
            _ => ""
        };
    }
}