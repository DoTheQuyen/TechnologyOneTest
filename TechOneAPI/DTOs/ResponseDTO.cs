namespace TechOneAPI.DTOs
{

    /// <summary>
    /// build a generic response DTO to handle the response of the API, so that the response can be standardized and frontend can handle the response easily.
    /// this is particularly useful for APIs that return different types of data, as it allows for a consistent response structure regardless of the data type being returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseDTO<T>
    {
        public bool IsSuccess { get; private set; }

        public T? Result { get; private set; }

        public string? ErrMsg { get; private set; }

        private ResponseDTO() { }

        public static ResponseDTO<T> Success(T result)
        {
            return new ResponseDTO<T> { IsSuccess = true, Result = result };
        }

        public static ResponseDTO<T> Failure(string errMsg)
        {
            return new ResponseDTO<T> { IsSuccess = false, ErrMsg = errMsg };
        }
    }
}
