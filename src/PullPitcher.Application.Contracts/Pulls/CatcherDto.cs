namespace PullPitcher.Pulls
{
    public class CatcherDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ExternalId { get; set; }
        public string Repository { get; set; }
        public string ChannelId { get; set; }
        public ChannelDto Channel { get; set; }
    }
}