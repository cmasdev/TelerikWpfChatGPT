using System;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls.ConversationalUI;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows.Threading;
using System.Windows;
using Newtonsoft.Json;

namespace TelerikWpfChat
{
    public class ChatViewModel
    {
        public ObservableCollection<TextMessageModel> Messages { get; set; }
        public Author CurrentAuthor { get; private set; }
        public Author OtherAuthor { get; private set; }

        public ChatViewModel()
        {
            this.CurrentAuthor = new Author("Christian");
            this.OtherAuthor = new Author("GPT");
            if (this.Messages == null)
                this.Messages = new ObservableCollection<TextMessageModel>();
        }

        internal async Task SendCurrentMessage(string prompt)
        {
            //Agregamos nuestra interacción
            this.Messages.Add(new TextMessageModel() { Text = prompt, Author = CurrentAuthor, CreationDate = DateTime.Now });

            //Llamamos a la API de GPT
            var result = await sendToGpt(prompt);

            if (result == null) 
            {
                MessageBox.Show("Error en la Matrix!");
                return;
            }

            //Obtenemos los resultados y le pasamos a la interfaz en un hilo separado
            await Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => {
                this.Messages.Add(new TextMessageModel() { Text = result, Author = OtherAuthor, CreationDate = DateTime.Now });
            }));        
        }

        private async Task<string> sendToGpt(string prompt)
        {
            var apiKey = Properties.Settings.Default.ApiGPTChat;
            var model = Properties.Settings.Default.Model;
            var maxTokens = 500;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var url = "https://api.openai.com/v1/completions";


            var requestBody = new
            {
                prompt = prompt,
                model = model,
                temperature = 0.5,
                max_tokens = maxTokens,
                n = 1
            };

            var postJson = System.Text.Json.JsonSerializer.Serialize(requestBody);

            var response = await client.PostAsync(url,
                new StringContent(postJson,
                System.Text.Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();

            dynamic responseData = JsonConvert.DeserializeObject<dynamic>(responseBody);

            int lastChoice = responseData.choices.Count - 1;
            
            return responseData.choices[lastChoice].text;
        }
    }
}
