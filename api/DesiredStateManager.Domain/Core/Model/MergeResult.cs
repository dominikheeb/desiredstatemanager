namespace DesiredStateManager.Domain.Core.Model
{
    public class MergeResult<T> where T : IDscResource
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public T Value { get; set; }

        public (T, MergeResult<T>) ConflicTuple { get; set; }
    }
}