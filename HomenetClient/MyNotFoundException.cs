﻿using System;

namespace HomenetBase
{
    public class MyNotFoundException : Exception
    {
        public MyNotFoundException()
        { }

        public MyNotFoundException(string message)
            : base(message)
        { }

        public MyNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
