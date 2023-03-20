using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ConversationalUI;

namespace TelerikWpfChat
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void chat_SendMessage(object sender, SendMessageEventArgs e)
        {
            e.Handled = true;
            var chat = sender as RadChat;
            var model = (ChatViewModel)chat.DataContext;
            var mensaje = e.Message as TextMessage;

            chat.TypingIndicatorVisibility = Visibility.Visible;
            chat.TypingIndicatorText = "Esperando respuesta de " + model.OtherAuthor.Name;
            
            await model.SendCurrentMessage(mensaje.Text);

            chat.TypingIndicatorVisibility = Visibility.Collapsed;
            chat.TypingIndicatorText =string.Empty;

        }
    }
}
