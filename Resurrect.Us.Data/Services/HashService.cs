using System;
using System.Collections.Generic;
using System.Text;

namespace Resurrect.Us.Data.Services
{
    public class HashService : IHashService
    {
        private readonly IHashStrategy hashStrategy;

        public HashService(IHashStrategy hashStrategy)
        {
            this.hashStrategy = hashStrategy;
        }

        public string GetHash(long input)
        {
            return this.hashStrategy.EncodeHash(input);
        }

        public long GetRecordId(string input)
        {
            return this.hashStrategy.DecodeHash(input);
        }
    }
}
