using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIcrosoftGraphService.Model
{
    [Serializable]
    public class ResponseMessage
    {
        public ResponseMessage()
        {
            Type = ResponseType.OK;
        }
        public ResponseType Type { get; set; }
    }

    public class ErrorResponseMessage : ResponseMessage
    {
        public ErrorResponseMessage()
        {
            Type = ResponseType.Error;
            Errors = new List<string>();
        }
        public ErrorResponseMessage(string error)
        {
            Type = ResponseType.Error;
            Errors = new List<string>();
            Errors.Add(error);
        }
        public List<string> Errors { get; set; }
    }

    //.....

    public enum ResponseType
    {
        OK = 0,
        Error = 1,
        //...
    }
}
