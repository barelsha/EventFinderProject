using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WCFServiceWebRole2.DB
{
    public class ResponseObject<T>
    {
        private T _data;
        private bool _success = true;
        private string _message;

        public T data
        {
            get { return _data; }
            set { _data = value; }
        }

        public bool success
        {
            get { return _success; }
            set { _success = value; }
        }


        public string message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}