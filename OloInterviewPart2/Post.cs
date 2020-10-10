namespace OloInterviewPart2
{
    internal class Post
    {
        public int Id;
        public int UserId;
        public string Title;
        public string Body;

        public override bool Equals(object obj)
        {
            if (obj is Post asPost)
            {
                return Id == asPost.Id &&
                    UserId == asPost.UserId &&
                    Title == asPost.Title &&
                    Body == asPost.Body;
            }
            return false;
        }
    }
}