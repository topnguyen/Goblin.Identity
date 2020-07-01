using System;

namespace Goblin.Identity.Core
{
    public class TokenDataModel
    {
        public DateTimeOffset ExpireTime { get; set; }
        
        public DateTimeOffset CreatedTime { get; set; }
    }
    public class TokenDataModel<T> : TokenDataModel
    {
        public T Data { get; set; }
    }
}