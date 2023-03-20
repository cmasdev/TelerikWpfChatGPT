using System;
using Telerik.Windows.Controls.ConversationalUI;

namespace TelerikWpfChat
{
    public class TextMessageModel
    {
        public string Text { get; set; }
        public Author Author { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
