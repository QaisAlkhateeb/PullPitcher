using System.Collections.Generic;

namespace PullPitcher.Pulls
{
    public class PullRequestDto
    {
        public string OwnerId { get; protected set; }
        public string Repository { get; protected set; }
        public string Link { get; protected set; }
        public string Number { get; protected set; }
        public List<PullReviewerDto> Reviewers { get; protected set; }
    }
}