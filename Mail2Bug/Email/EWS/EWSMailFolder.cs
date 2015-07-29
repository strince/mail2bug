﻿ #define DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Exchange.WebServices.Data;

namespace Mail2Bug.Email.EWS
{
   
    class EWSMailFolder : IMailFolder
    {
        private readonly Folder _folder;

        public EWSMailFolder(Folder folder)
        {
            _folder = folder;
        }

        public int GetTotalCount()
        {
            return _folder.TotalCount;
        }

        public IEnumerable<IIncomingEmailMessage> GetMessages()
        {
            var itemCount = _folder.TotalCount;
            if (itemCount <= 0)
            {
                return new List<IIncomingEmailMessage>();
            }

            var view = new ItemView(itemCount);

         #if DEBUG
            var items = _folder.FindItems("subject:(Mail2Bug Mailbox)", view);
         #else
            var items = _folder.FindItems(view);
         #endif
             return items
                    .Where(item => item is EmailMessage) // Return only email message items - ignore any other items
                    .Select(item => new EWSIncomingMessage((EmailMessage) item)); // And wrap them with EWSIncomingMessage
        }
    }
}
