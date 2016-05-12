using Ocr.Demo.Core;
using System;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Ocr.Demo.Ui
{
    /// <summary>
    /// Página principal da aplicação.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Members
        private OcrManager _manager;

        // Constructor
        /// <summary>
        /// Inicializa componentes e carrega uma instância da classe de entrada da Core.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            // Salva uma instância da classe de entrada da Core.
            this._manager = new OcrManager();
        }

        // Private methods
        /// <summary>
        /// Chama método para carregar imagem.
        /// </summary>
        /// <param name="sender">Botão de carregar imagem.</param>
        /// <param name="e">Argumentos do evento.</param>
        private void uxButtonInputFile_Click(object sender, RoutedEventArgs e)
        {
            this.LoadImage();
        }
        /// <summary>
        /// Método que recupera imagem no storage do equipamento e coloca na tela.
        /// </summary>
        private async void LoadImage()
        {
            StorageFile file = await this._manager.GetImageFromStorage();

            if (file == null) // Se o usuário não escolheu nenhuma imagem da biblioteca.
            {
                return;
            }
            else
            {
                // Coloca nome e caminho do arquivo no textbox de input file.
                this.uxTextBoxInputFile.Text = file.Path + file.Name;

                // Abre arquivo carregado.
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    // Converte para bitmap.
                    BitmapImage bitmap = new BitmapImage();
                    await bitmap.SetSourceAsync(stream);
                    // Define como source para imagem na tela.
                    this.uxImageInput.Source = bitmap;
                }
            }
        }
    }
}
