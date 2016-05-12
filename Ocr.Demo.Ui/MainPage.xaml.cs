using Ocr.Demo.Core;
using System;
using Windows.Graphics.Imaging;
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
        /// <summary>
        /// Instancia de entrada para biblioteca Core.
        /// </summary>
        private OcrManager _manager;
        /// <summary>
        /// Software bitmap da imagem carregada, necessio para processar OCR.
        /// </summary>
        private SoftwareBitmap _softbitmap;

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
        /// Recupera imagem no storage do equipamento e coloca na tela.
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
                this.uxTextBoxInputFile.Text = file.Path;

                // Abre arquivo carregado.
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    // Cria um decoder a partir do stream.
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                    // Recupera um Software Bitmap do arquivo.
                    this._softbitmap = await decoder.GetSoftwareBitmapAsync();

                    // Converte para bitmap.
                    BitmapImage bitmap = new BitmapImage();
                    await bitmap.SetSourceAsync(stream);
                    // Define como source para imagem na tela.
                    this.uxImageInput.Source = bitmap;
                }
            }
        }
        /// <summary>
        /// Chama método para processar por OCR a imagem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxButtonOutputFile_Click(object sender, RoutedEventArgs e)
        {
            this.ProcessImage();
        }
        /// <summary>
        /// Processa OCR encima da imagem carregada.
        /// </summary>
        private async void ProcessImage()
        {
            // Passa software bitmap para processamento de OCR na Core.
            string result = await this._manager.GetTextFromImage(this._softbitmap);
            // Exibe texto recuperado na tela.
            this.uxTextBlockOutput.Text = result;

            // Define nome do arquivo.
            string filename = this.uxTextBoxInputFile.Text;
            filename = filename.Substring(filename.LastIndexOf(@"\") + 1, filename.LastIndexOf(@".") - filename.LastIndexOf(@"\")) + "txt";
            this.uxTextBoxOutputFile.Text = filename;

            // Salva arquivo .txt do arquivo.
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync(filename, 
                Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(file, result);
        }
    }
}
