using System;
using System.Collections.Generic;

namespace Karenia.Visby.Result
{
    public class Result<T>
    {
        public int Code { set; get; }
        public string Message { set; get; }
        public T Data { set; get; }

        public Result(int code, string message, T data)
        {
            Code = code;
            Message = message;
            Data = data;
        }
    }

    public class ResultList<T>
    {
        public int Code { set; get; }
        public string Message { set; get; }
        public List<T> Data { set; get; }
        public bool HasNext { set; get; }
        public int TotalCount { set; get; }
        public string NextUrl { set; get; }

        public ResultList(int code, string message, List<T> data, bool hasNext, int count, string nextUrl)
        {
            Code = code;
            Data = data;
            Message = message;
            HasNext = hasNext;
            TotalCount = count;
            NextUrl = nextUrl;
        }
    }
}