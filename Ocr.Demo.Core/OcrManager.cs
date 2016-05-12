using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Ocr.Demo.Core
{
    /// <summary>
    /// Classe de entrada do Core. Fornece acesso funções por ele implementadas.
    /// </summary>
    public class OcrManager
    {
        // Public methods
        /// <summary>
        /// Resulta em um StorageFile de uma imagem.
        /// </summary>
        /// <returns>Tarefa cujo resultado é um StorageFile.</returns>
        public async Task<StorageFile> GetImageFromStorage()
        {
            // Cria instância de picker para documentos no storage.
            FileOpenPicker picker = new FileOpenPicker();
            // Exibe os documentos em thumbnail.
            picker.ViewMode = PickerViewMode.Thumbnail;
            // Inicia na pasta de imagens.
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            // Filtra para extensões .jpg, .jpeg e .png.
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            // Aguarda retorno da escolha de um arquivo.
            StorageFile file = await picker.PickSingleFileAsync();

            // Devolve storage file para a chamada do método.
            return file;
        }
    }
}
