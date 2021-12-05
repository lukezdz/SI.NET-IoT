using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataExplorerApi.Model
{
    public class MessagesList
    {
        public List<Message> messages { get; set; }

        public MessagesList(List<Message> list)
        {
           this.messages = list;
        }
        
      

    }
}
