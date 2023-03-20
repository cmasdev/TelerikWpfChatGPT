using Telerik.Windows.Controls.ConversationalUI;

namespace TelerikWpfChat
{
    public class MessageConverter : IMessageConverter
    {
        public MessageBase ConvertItem(object item)
        {
            var messageModel = (TextMessageModel)item;
            return new TextMessage(messageModel.Author, messageModel.Text, messageModel.CreationDate);
        }

        public object ConvertMessage(MessageBase message)
        {
            var textMessage = (TextMessage)message;
            return new TextMessageModel()
            {
                Author = textMessage.Author,
                Text = textMessage.Text,
                CreationDate = textMessage.CreationDate
            };
        }
    }
}
