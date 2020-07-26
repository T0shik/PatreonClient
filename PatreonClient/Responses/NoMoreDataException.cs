using System;

namespace PatreonClient.Responses
{
    public class NoMoreDataException : Exception
    {
        public override string Message => "No more data to fetch";
    }
}